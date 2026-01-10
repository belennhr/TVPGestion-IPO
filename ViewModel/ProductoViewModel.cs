using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    public class ProductoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string nombre;
        private string categoria;
        private string subcategoria;
        private string foto;
        private string precio;
        private string alergenosString;
        private ObservableCollection<string> ingredientes;

        public string Nombre
        {
            get => nombre;
            set
            {
                nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public string Categoria
        {
            get => categoria;
            set
            {
                categoria = value;
                OnPropertyChanged(nameof(Categoria));
            }
        }

        public string Subcategoria
        {
            get => subcategoria;
            set
            {
                subcategoria = value;
                OnPropertyChanged(nameof(Subcategoria));
            }
        }

        public string Foto
        {
            get => foto;
            set
            {
                foto = value;
                OnPropertyChanged(nameof(Foto));
            }
        }

        public string Precio
        {
            get => precio;
            set
            {
                precio = value;
                OnPropertyChanged(nameof(Precio));
            }
        }

        public string AlergenosString
        {
            get => alergenosString;
            set
            {
                alergenosString = value;
                OnPropertyChanged(nameof(AlergenosString));
            }
        }

        public ObservableCollection<string> Ingredientes
        {
            get => ingredientes;
            set
            {
                ingredientes = value;
                OnPropertyChanged(nameof(Ingredientes));
                OnPropertyChanged(nameof(IngredientesString));
            }
        }

        public string IngredientesString
        {
            get => Ingredientes != null && Ingredientes.Count > 0
                ? string.Join(", ", Ingredientes)
                : "";
            set
            {
                Ingredientes = !string.IsNullOrWhiteSpace(value)
                    ? new ObservableCollection<string>(value.Split(',').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    : new ObservableCollection<string>();
            }
        }

        public static List<string> CategoriasDisponibles =>
            Enum.GetNames(typeof(CategoriaProducto)).ToList();

        public ProductoViewModel()
        {
            Ingredientes = new ObservableCollection<string>();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
                    ? new ObservableCollection<string>(producto.Ingredientes)
                    : new ObservableCollection<string>()
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
                Ingredientes = this.Ingredientes != null ? this.Ingredientes.ToList() : new List<string>()
            };
        }
    }
}