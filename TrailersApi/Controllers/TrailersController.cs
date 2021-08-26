using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrailersApi.DTOs;

namespace TrailersApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TrailersController : CustomBaseController {
        private readonly TrailersDbContext context;
        private readonly IMapper mapper;

        public TrailersController(TrailersDbContext context, IMapper mapper) : base(context, mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        //Metodo Get
        [HttpGet]
        public async Task<ActionResult<List<TrailersDTO>>> Get() {
            return await Get<Trailer, TrailersDTO>();
        }

        //Metodo Get(id)
        [HttpGet("{id:int}", Name = "obtenerTrailer")]
        public async Task<ActionResult<TrailersDTO>> Get(int id) {
            return await Get<Trailer, TrailersDTO>(id);
        }

        //Metodo Post
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TrailersCreacionDTO trailersCreacionDTO) {
            return await Post<TrailersCreacionDTO, Trailer, TrailersDTO>(trailersCreacionDTO, "obtenerTrailer");
        }

        //Metodo Put
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] TrailersCreacionDTO trailersCreacionDTO) {
            return await Put<TrailersCreacionDTO, Trailer>(id, trailersCreacionDTO);
        }

        //Metodo Patch
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Trailer> patchDoc) {
            return await Patch<Trailer, TrailersDTO>(id, patchDoc);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            return await Delete<Trailer>(id);
        }

    }
}
