using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrailersApi.DTOs;
using TrailersApi.Models;

namespace TrailersApi.Controllers {
    [Route(("api/usuarios"))]
    [ApiController]
    public class AuthController : CustomBaseController {

        #region Constructor
        private readonly UserManager<UserTrailer> _userManager;
        private readonly SignInManager<UserTrailer> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly TrailersDbContext _context;
        private readonly IMapper _mapper;

        public AuthController(
            UserManager<UserTrailer> userManager,
            SignInManager<UserTrailer> signInManager,
            IConfiguration configuration,
            TrailersDbContext context, 
            IMapper mapper) : base(context, mapper) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Endpoint -> Registrar ("registrar")
        [HttpPost("registrar")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model) {
            BaseResponse res = new BaseResponse();

            var user = new UserTrailer { FirstName = model.FirstName, LastName = model.LastName, UserName = model.FirstName, Email = model.Email };


            var result = await _userManager.CreateAsync(user, model.Password);

            var entidad = _mapper.Map<UserTrailer>(model);
            _context.Add(entidad);

            await _context.SaveChangesAsync();

            if (result.Succeeded) {
                res.Message = "Registro correcto.";
                res.Data = new {
                    Name = model.FirstName,
                    Email = model.Email,
                    Token = BuildToken(model)
                };
                res.Ok = true;

                return Ok(res);
            } else {
                res.Message = "Registro incorrecto";
                res.Data = new {
                    FirstName = "Escribe tu nombre",
                    LastName = "Escribe tu apellido",
                    Email = "Escribe tu email",
                    Password = "Escribe una constreña que contenga caracteres especiales, al menos una mayuscula, numeros y mayor 8 caracteres"
                };
                res.Ok = false;
                return BadRequest(res);
            }

        }

        #endregion

        #region Endpoint -> Login ("login")
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model) {
            BaseResponse res = new BaseResponse();

            UserTrailer existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser == null) {
                res.Message = "Este email no existe.";
                return Unauthorized(res);
            }

            Microsoft.AspNetCore.Identity.SignInResult isValidPassword = await _signInManager.CheckPasswordSignInAsync(existingUser, model.Password, false);

            if (!isValidPassword.Succeeded) {
                res.Message = "Contraseña incorrecta";
                return BadRequest(res);
            }

            await _signInManager.SignInAsync(existingUser, true);

            res.Message = "Inicio de sesión correcto.";
            res.Data = new {
                Name = existingUser.FirstName,
                Email = existingUser.Email,
                Token = BuildToken(model)
            };
            res.Ok = true;

            return Ok(res);
        }

        #endregion

        #region Endpoint -> Logout ("logout")
        [HttpPost]
        [Route("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> LogoutAsync() {
            await _signInManager.SignOutAsync();

            return Ok(new BaseResponse {
                Ok = true,
                Message = "Se ha cerrado sesión correctamente."
            });
        }
        #endregion

        #region Endpoint -> El usuario esta autenticado? ("userAuth")
        [HttpGet]
        [Route("userAuth")]
        public IActionResult UserAuth() {

            ClaimsPrincipal userAuth = HttpContext.User;

            return Ok(new BaseResponse {
                Ok = true,
                Data = new {
                    UserName = userAuth?.Identity?.Name,
                    IsAuthentication = userAuth?.Identity?.IsAuthenticated,
                    Claims = userAuth.Claims.Select(x => new { x.Type, x.Value })
                                            .GroupBy(x => x.Type)
                                            .ToDictionary(c => c.Key, func)
                }
            });
        }
        private Func<IGrouping<string, object>, object> func = (c) => (c.Count() > 1) ? c.ToList() : c.FirstOrDefault();
        #endregion

        #region BuildToken
        private UserToken BuildToken(UserInfo userInfo) {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("miValor", "Lo que yo quiera"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de dos dias
            var expiration = DateTime.UtcNow.AddDays(5);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken() {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };


        }
        #endregion

    }
}
