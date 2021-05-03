using PruebaFirebaseStorage.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruebaFirebaseStorage
{
    public partial class MainPage : ContentPage
    {
        FireBaseHelper PH = new FireBaseHelper();
        public MainPage()
        {
            InitializeComponent();         
            CargarTodo();
            

            ICommand refreshCommand = new Command(() =>
            {
                refreshView.IsRefreshing = true;
                CargarTodo();
                
                refreshView.IsRefreshing = false;
            });
            refreshView.Command = refreshCommand;
          
        }
       
        public async void CargarTodo()
        {
          
            var allProducts = await PH.GetProductoPorCategoria("BestSeller");
            lstProductos.ItemsSource = allProducts;
            var allProducts2 = await PH.GetProductoPorCategoria("Gala");
            lstProductos2.ItemsSource = allProducts2;
            // var allProducts = await PH.GetAllProducts();
            // lstProductos.ItemsSource = allProducts;

        }   
        public async void CargarProducto(int id)
        {
            
            int ProductoId; string Nombre; string Precio; string Descripcion; string Imagen; string Categoria;
           // var producto = await PH.GetProductoPorCategoria("BestSeller");
            var producto = await PH.GetProducto(id);
            if (producto != null)
            {
                ProductoId = producto.ProductoId;
                Nombre = producto.Nombre;
                Precio = producto.Precio;
                Descripcion = producto.Descripcion;
                Imagen = producto.Imagen;
                Categoria = producto.Categoria;
                await Navigation.PushAsync(new Descripcion(producto.ProductoId, producto.Nombre, producto.Precio, producto.Descripcion, producto.Imagen, producto.Categoria));
               // await DisplayAlert("Success", "Product Retrive Successfully", "OK");
            }
            else
            {       
                await DisplayAlert("Success", "No Product Available", "OK");              
            }
        }
 
        private async void OnItemSelected(Object sender, ItemTappedEventArgs e)
        {


            var mydetails = e.Item as Producto;
         
            await Navigation.PushAsync(new Descripcion(mydetails.ProductoId, mydetails.Nombre, mydetails.Precio, mydetails.Descripcion, mydetails.Imagen, mydetails.Categoria));
        }
        private async void OnUploadClicked(Object sender, EventArgs e)
        {
            await PH.AddProducto(Convert.ToInt32(productoid.Text), nombre.Text, precio.Text, descripcion.Text, imagen.Text, Categoria.Text);   
            productoid.Text = string.Empty;
            nombre.Text = string.Empty;
            precio.Text = string.Empty;
            descripcion.Text = string.Empty;
            imagen.Text = string.Empty;
            Categoria.Text = string.Empty;
            await DisplayAlert("Success", "Product Added Successfully", "OK");
             var allProducts = await PH.GetAllProducts();
             lstProductos.ItemsSource = allProducts;
           // var allProducts = await PH.GetProductoPorCategoria("BestSeller");
           // lstProductos.ItemsSource = allProducts;
        }

        private void lstProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // int previous =Convert.ToInt32( (e.PreviousSelection.FirstOrDefault() as Producto)?.ProductoId);
            int current =Convert.ToInt32( (e.CurrentSelection.FirstOrDefault() as Producto)?.ProductoId);
            CargarProducto(current);
        }
    }
}
