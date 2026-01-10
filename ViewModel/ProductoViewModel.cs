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
        public string Precio { get; set; }
        public string AlergenosString { get; set; }

        // NUEVO: colección para mostrar en lista
        public List<string> Ingredientes { get; set; } = new List<string>();

        // Conserva el string si se edita como texto en otras pantallas
        public string IngredientesString
        {
            get => Ingredientes != null && Ingredientes.Count > 0
                ? string.Join(", ", Ingredientes)
                : "";
            set
            {
                Ingredientes = !string.IsNullOrWhiteSpace(value)
                    ? value.Split(',').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToList()
                    : new List<string>();
            }
        }

        // Lista estatica para ComboBox
        public static List<string> CategoriasDisponibles =>
            Enum.GetNames(typeof(CategoriaProducto)).ToList();

        public static ProductoViewModel FromProducto(Producto producto)
        {
            return new ProductoViewModel
            {
                Nombre = producto.Nombre,
                Categoria = producto.Categoria.ToString(),
                Subcategoria = producto.Subcategoria,
                Foto = producto.Foto,
                Precio = producto.Precio.ToString("F2"),
                AlergenosString = producto.Alergenos != null && producto.Alergenos.Count > 0
                    ? string.Join(", ", producto.Alergenos)
                    : "",
                Ingredientes = producto.Ingredientes != null
                    ? new List<string>(producto.Ingredientes)
                    : new List<string>()
            };
        }

        public Producto ToProducto()
        {
            return new Producto
            {
                Nombre = this.Nombre ?? "",
                Categoria = !string.IsNullOrEmpty(this.Categoria)
                    ? (CategoriaProducto)Enum.Parse(typeof(CategoriaProducto), this.Categoria)
                    : CategoriaProducto.Plato,
                Subcategoria = this.Subcategoria ?? "",
                Foto = this.Foto ?? "/Assets/Icons/comidaIcon.png",
                Precio = !string.IsNullOrEmpty(this.Precio) ? decimal.Parse(this.Precio) : 0m,
                Alergenos = !string.IsNullOrEmpty(this.AlergenosString)
                    ? this.AlergenosString.Split(',').Select(a => a.Trim()).Where(a => !string.IsNullOrEmpty(a)).ToList()
                    : new List<string>(),
                // Usa la colección directamente
                Ingredientes = this.Ingredientes ?? new List<string>()
            };
        }
    }
}