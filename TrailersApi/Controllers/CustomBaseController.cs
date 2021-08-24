using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrailersApi.Models;

namespace TrailersApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CustomBaseController : ControllerBase {

        protected readonly TrailersDbContext _context;
        protected readonly IMapper _mapper;

        public CustomBaseController(TrailersDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/[controller]
        [HttpGet]
        protected async Task<List<TDTO>> Get<TEntidad, TDTO>() where TEntidad : class {
            var entidades = await _context.Set<TEntidad>().AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<List<TDTO>>(entidades);
            return dtos;
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id) where TEntidad : class, IId {
            var entidad = await _context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null) {
                return NotFound();
            }

            return _mapper.Map<TDTO>(entidad);
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        protected async Task<ActionResult> Put<TCreacion, TEntidad>(int id, TCreacion creacionDTO) where TEntidad : class, IId {
            try {

                var entidad = _mapper.Map<TEntidad>(creacionDTO);
                entidad.Id = id;
                _context.Entry(entidad).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();

            } catch (DbUpdateConcurrencyException ex) {

                return BadRequest($"Lo sentimos algo salio mal: {ex.Message}");

            }
        }

        // POST: api/[controller]
        [HttpPost]
        protected async Task<ActionResult> Post<TCreacion, TEntidad, TLectura>(TCreacion creacionDTO, string nombreRuta) where TEntidad : class, IId {
            try {
                var entidad = _mapper.Map<TEntidad>(creacionDTO);
                _context.Add(entidad);
                await _context.SaveChangesAsync();
                var dtoLectura = _mapper.Map<TLectura>(entidad);

                return new CreatedAtRouteResult(nombreRuta, new { id = entidad.Id }, dtoLectura);
            } catch (Exception e) {
                return Content(e.ToString());
            }


        }
        // PATCH: api/[controller]/5
        [HttpPatch]
        protected async Task<ActionResult> Patch<TEntidad, TDTO>(int id, JsonPatchDocument<TEntidad> patchDoc) where TDTO : class where TEntidad : class, IId {
            if (patchDoc == null) {
                return BadRequest("No es un formato válido.");
            }

            var entidadEdit = await _context.Set<TEntidad>().FirstOrDefaultAsync(x => x.Id == id);

            if (entidadEdit == null) {
                return NotFound();
            }

            patchDoc.ApplyTo(entidadEdit, ModelState);

            var isValid = TryValidateModel(entidadEdit);

            if (!isValid) {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return NoContent();

        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        protected async Task<ActionResult> Delete<TEntidad>(int id) where TEntidad : class, IId, new() {
            var existe = await _context.Set<TEntidad>().AnyAsync(x => x.Id == id);
            if (!existe) {
                return NotFound();
            }

            _context.Remove(new TEntidad() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
