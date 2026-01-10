using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    public class ProductoCantidadViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string nombre;
        private decimal precio;
        private int cantidad;

        public string Nombre
        {
            get => nombre;
            set
            {
                nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public decimal Precio
        {
            get => precio;
            set
            {
                precio = value;
                OnPropertyChanged(nameof(Precio));
            }
        }

        public int Cantidad
        {
            get => cantidad;
            set
            {
                cantidad = value;
                OnPropertyChanged(nameof(Cantidad));
            }
        }

        public string DisplayText => $"{Nombre} - {Precio:C}";

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PedidoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string id;
        private string fechaHoraRealizacion;
        private string medio;
        private string modalidad;
        private string fechaHoraRecogida;
        private string clienteEmail;
        private string productosString;
        private decimal importeTotal;
        private string formaPago;
        private string estado;
        private string direccionEntrega;
        private decimal costeEnvio;
        private bool envioGratisCanjeado;
        private bool acumularPuntos;
        private int puntosGanados;
        private ObservableCollection<ProductoCantidadViewModel> productos;
        private ObservableCollection<ProductoCantidadViewModel> productosDisponibles;

        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string FechaHoraRealizacion
        {
            get => fechaHoraRealizacion;
            set
            {
                fechaHoraRealizacion = value;
                OnPropertyChanged(nameof(FechaHoraRealizacion));
            }
        }

        public string Medio
        {
            get => medio;
            set
            {
                medio = value;
                OnPropertyChanged(nameof(Medio));
            }
        }

        public string Modalidad
        {
            get => modalidad;
            set
            {
                modalidad = value;
                OnPropertyChanged(nameof(Modalidad));
            }
        }

        public string FechaHoraRecogida
        {
            get => fechaHoraRecogida;
            set
            {
                fechaHoraRecogida = value;
                OnPropertyChanged(nameof(FechaHoraRecogida));
            }
        }

        public string ClienteId
        {
            get => clienteEmail;
            set
            {
                clienteEmail = value;
                OnPropertyChanged(nameof(ClienteId));
            }
        }

        public string ClienteEmail
        {
            get => clienteEmail;
            set
            {
                clienteEmail = value;
                OnPropertyChanged(nameof(ClienteEmail));
            }
        }

        public string ProductosString
        {
            get => productosString;
            set
            {
                productosString = value;
                OnPropertyChanged(nameof(ProductosString));
            }
        }

        public decimal ImporteTotal
        {
            get => importeTotal;
            set
            {
                importeTotal = value;
                OnPropertyChanged(nameof(ImporteTotal));
            }
        }

        public string FormaPago
        {
            get => formaPago;
            set
            {
                formaPago = value;
                OnPropertyChanged(nameof(FormaPago));
            }
        }

        public string Estado
        {
            get => estado;
            set
            {
                estado = value;
                OnPropertyChanged(nameof(Estado));
            }
        }

        public string DireccionEntrega
        {
            get => direccionEntrega;
            set
            {
                direccionEntrega = value;
                OnPropertyChanged(nameof(DireccionEntrega));
            }
        }

        public decimal CosteEnvio
        {
            get => costeEnvio;
            set
            {
                costeEnvio = value;
                OnPropertyChanged(nameof(CosteEnvio));
            }
        }

        public bool EnvioGratisCanjeado
        {
            get => envioGratisCanjeado;
            set
            {
                envioGratisCanjeado = value;
                OnPropertyChanged(nameof(EnvioGratisCanjeado));
            }
        }

        public bool AcumularPuntos
        {
            get => acumularPuntos;
            set
            {
                acumularPuntos = value;
                OnPropertyChanged(nameof(AcumularPuntos));
            }
        }

        public int PuntosGanados
        {
            get => puntosGanados;
            set
            {
                puntosGanados = value;
                OnPropertyChanged(nameof(PuntosGanados));
            }
        }

        // Lista editable de productos y cantidades
        public ObservableCollection<ProductoCantidadViewModel> Productos
        {
            get => productos;
            set
            {
                if (productos != null)
                {
                    productos.CollectionChanged -= Productos_CollectionChanged;
                }

                productos = value;

                if (productos != null)
                {
                    productos.CollectionChanged += Productos_CollectionChanged;
                }

                OnPropertyChanged(nameof(Productos));
                ActualizarProductosString();
            }
        }

        // Lista de todos los productos disponibles para el ComboBox
        public ObservableCollection<ProductoCantidadViewModel> ProductosDisponibles
        {
            get => productosDisponibles;
            set
            {
                productosDisponibles = value;
                OnPropertyChanged(nameof(ProductosDisponibles));
            }
        }

        // Listas estáticas para ComboBoxes
        public static List<string> MediosDisponibles => 
            Enum.GetNames(typeof(MedioPedido)).ToList();

        public static List<string> ModalidadesDisponibles => 
            Enum.GetNames(typeof(ModalidadEntrega)).ToList();

        public static List<string> EstadosDisponibles => 
            Enum.GetNames(typeof(EstadoPedido)).ToList();

        public PedidoViewModel()
        {
            Productos = new ObservableCollection<ProductoCantidadViewModel>();
            ProductosDisponibles = new ObservableCollection<ProductoCantidadViewModel>();
            AcumularPuntos = true;
        }

        private void Productos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Suscribirse a cambios en las propiedades de los productos añadidos
            if (e.NewItems != null)
            {
                foreach (ProductoCantidadViewModel producto in e.NewItems)
                {
                    producto.PropertyChanged += Producto_PropertyChanged;
                }
            }

            // Desuscribirse de productos eliminados
            if (e.OldItems != null)
            {
                foreach (ProductoCantidadViewModel producto in e.OldItems)
                {
                    producto.PropertyChanged -= Producto_PropertyChanged;
                }
            }

            ActualizarProductosString();
        }

        private void Producto_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Actualizar cuando cambie la cantidad de un producto
            if (e.PropertyName == nameof(ProductoCantidadViewModel.Cantidad))
            {
                ActualizarProductosString();
            }
        }

        private void ActualizarProductosString()
        {
            if (Productos != null && Productos.Count > 0)
            {
                ProductosString = string.Join(", ", Productos.Select(p => $"{p.Nombre} x{p.Cantidad}"));
            }
            else
            {
                ProductosString = "";
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Método de conversión Model → ViewModel
        public static PedidoViewModel FromPedido(Pedido pedido)
        {
            var vm = new PedidoViewModel
            {
                Id = pedido.Id,
                FechaHoraRealizacion = pedido.FechaHoraRealizacion.ToString("g"),
                Medio = pedido.Medio.ToString(),
                Modalidad = pedido.Modalidad.ToString(),
                FechaHoraRecogida = pedido.FechaHoraRecogida?.ToString("g") ?? "",
                ClienteEmail = pedido.ClienteEmail,
                ImporteTotal = pedido.ImporteTotal,
                FormaPago = pedido.FormaPago,
                Estado = pedido.Estado.ToString(),
                DireccionEntrega = pedido.DireccionEntrega,
                CosteEnvio = pedido.CosteEnvio,
                EnvioGratisCanjeado = pedido.EnvioGratisCanjeado,
                AcumularPuntos = pedido.AcumularPuntos,
                PuntosGanados = pedido.CalcularPuntosGanados()
            };

            foreach (var prod in pedido.Productos)
            {
                vm.Productos.Add(new ProductoCantidadViewModel
                {
                    Nombre = prod.Key.Nombre,
                    Precio = prod.Key.Precio,
                    Cantidad = prod.Value
                });
            }

            return vm;
        }

        // Método de conversión ViewModel → Model (requiere catálogo de productos)
        public Pedido ToPedido(List<Producto> catalogoProductos)
        {
            var pedido = new Pedido
            {
                Id = this.Id,
                FechaHoraRealizacion = DateTime.Parse(this.FechaHoraRealizacion),
                Medio = (MedioPedido)Enum.Parse(typeof(MedioPedido), this.Medio),
                Modalidad = (ModalidadEntrega)Enum.Parse(typeof(ModalidadEntrega), this.Modalidad),
                FechaHoraRecogida = string.IsNullOrEmpty(this.FechaHoraRecogida) ? (DateTime?)null : DateTime.Parse(this.FechaHoraRecogida),
                ClienteEmail = this.ClienteEmail,
                FormaPago = this.FormaPago,
                Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), this.Estado),
                DireccionEntrega = this.DireccionEntrega,
                CosteEnvio = this.CosteEnvio,
                EnvioGratisCanjeado = this.EnvioGratisCanjeado,
                AcumularPuntos = this.AcumularPuntos
            };

            // Convertir productos
            foreach (var prodVM in this.Productos)
            {
                var producto = catalogoProductos.FirstOrDefault(p => p.Nombre == prodVM.Nombre);
                if (producto != null)
                {
                    pedido.Productos[producto] = prodVM.Cantidad;
                }
            }

            return pedido;
        }
    }
}