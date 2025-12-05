using System;
using System.Collections.Generic;
using System.Linq;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    public class ProductoViewModel
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public string Foto { get; set; }
        public decimal Precio { get; set; }
        public string AlergenosString { get; set; }
        public string IngredientesString { get; set; }

        // Lista estática para ComboBox
        public static List<string> CategoriasDisponibles => 
            Enum.GetNames(typeof(CategoriaProducto)).ToList();
    }
}