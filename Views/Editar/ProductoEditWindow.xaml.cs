using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TVPGestion_IPO.Views;

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Lógica de interacción para ProductoEditWindow.xaml
    /// </summary>
    public partial class ProductoEditWindow : Window
    {
        private ProductoViewModel productoViewModel;

        public ProductoEditWindow(ProductoViewModel producto)
        {
            InitializeComponent();
            productoViewModel = producto;
            this.DataContext = productoViewModel;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Los cambios han sido guardados correctamente", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void TxtPrecio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string separadorDecimal = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            
            // Permite números, un solo separador decimal (coma o punto según cultura)
            string text = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            
            // Regex que permite números decimales positivos
            Regex regex = new Regex(@"^[0-9]*[" + Regex.Escape(separadorDecimal) + @"]?[0-9]*$");
            
            // Si no coincide con el patrón, cancela la entrada
            e.Handled = !regex.IsMatch(text);
        }

        private void TxtPrecio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Permite teclas de control (Backspace, Delete, Tab, etc.)
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void BtnAgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            string nuevoIngrediente = TxtNuevoIngrediente.Text.Trim();
            
            if (!string.IsNullOrWhiteSpace(nuevoIngrediente))
            {
                // Verifica que no exista ya el ingrediente
                if (!productoViewModel.Ingredientes.Contains(nuevoIngrediente))
                {
                    productoViewModel.Ingredientes.Add(nuevoIngrediente);
                    TxtNuevoIngrediente.Clear();
                    TxtNuevoIngrediente.Focus();
                }
                else
                {
                    MessageBox.Show("El ingrediente ya existe en la lista", "Ingrediente duplicado", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Por favor, introduce un ingrediente válido", "Campo vacío", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEliminarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                string ingrediente = btn.Tag.ToString();
                productoViewModel.Ingredientes.Remove(ingrediente);
            }
        }
    }
}
