using System;
using System.Collections.Generic;
using System.Text;

namespace BombonesApp2026.Servicios.DTOs.Bombon
{
    public class BombonCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int TipoBombonId { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public bool TieneAzucar { get; set; }
        public int PesoEnGramos { get; set; }
        public bool Activo { get; set; }

    }
}
