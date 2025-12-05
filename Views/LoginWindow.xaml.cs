using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary

    public partial class LoginWindow : Window
    {
        public Gestor admin = new Gestor { GestorId = "admin", Nombre = "Pepe", Contraseña = "123", FotoPerfil = "Test"};
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(credentialInput.Text) || String.IsNullOrEmpty(passwordInput.Password))
            {
                errorMsg.Content = "Error: Introduzca el usuario y la contraseña";
            }
            else
            {
                if (credentialInput.Text.Equals(admin.GestorId)
                && passwordInput.Password.Equals(admin.Contraseña))
                {
                    var ventana = new navBar(); 
                    ventana.Show();
                    this.Close();
                }
                else
                {
                    errorMsg.Content = "Error: Usuario o contraseña incorrectos";
                }
            }
        }
    }
}
