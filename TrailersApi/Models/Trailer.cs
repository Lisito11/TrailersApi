using System;
using System.Collections.Generic;
using TrailersApi.Models;

#nullable disable

namespace TrailersApi
{
    public partial class Trailer : IId {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Url { get; set; }
        public string Sipnosis { get; set; }
        public string Director { get; set; }
        public string Genero { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Puntaje { get; set; }
        public int? Status { get; set; }
    }
}
