using System;
using System.Collections.Generic;

namespace TVPGestion_IPO.Models
{
    public enum CategoriaProducto
    {
        Entrante,
        Plato,
        Postre,
        Bebida
    }

    public class Producto
    {
        public string Nombre { get; set; }
        public List<string> Ingredientes { get; set; }
        public CategoriaProducto Categoria { get; set; }
        public string Foto { get; set; }
        public decimal Precio { get; set; }
        public List<string> Alergenos { get; set; }
        public string Subcategoria { get; set; }
    }
}
