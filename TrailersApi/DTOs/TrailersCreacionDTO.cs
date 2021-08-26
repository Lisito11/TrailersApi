using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrailersApi.DTOs {
    public class TrailersCreacionDTO {
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
