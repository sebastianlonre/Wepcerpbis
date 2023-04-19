using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views.Accessibility;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Wepcerpbis
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //cuidado con los metodos protegidos
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            EditText txtId = FindViewById<EditText>(Resource.Id.txtId);
            EditText txtNombre = FindViewById<EditText>(Resource.Id.txtNombre);
            EditText txtDescripcion = FindViewById<EditText>(Resource.Id.txtDescripcion);
            Button btnConsultar = FindViewById<Button>(Resource.Id.btnConsultar);
            Button btnEnviar = FindViewById<Button>(Resource.Id.btnEnviar);

            string uriServicio = "https://jsonplaceholder.typicode.com/posts";

            //sobreescribir un boton desde un metodo protegido
            btnConsultar.Click += async (sender, e) =>
            {
                try
                {
                    cliente cliente = new cliente();
                    if (!string.IsNullOrWhiteSpace(txtId.Text))
                    {
                        int bandera = 0;
                        //convertir de string a entero
                        if (int.TryParse(txtId.Text.Trim(), out bandera))
                        {
                            var resultado = await cliente.Get<entrada>(uriServicio + "/" + txtId.Text);

                            if (cliente.codigoHTTP == 200)
                            {
                                txtNombre.Text = resultado.tittle;
                                txtDescripcion.Text = resultado.body;
                                Toast.MakeText(this, "consulta realizada con exito", ToastLength.Long).Show();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "error:" + ex, ToastLength.Short).Show();
                }
            };

            btnEnviar.Click += async (sender, e) =>
            {
                try
                {
                    cliente cliente = new cliente();
                    entrada item = new entrada();
                    item.tittle = txtNombre.Text;
                    item.body = txtDescripcion.Text;
                    var resultado = await cliente.Post<entrada>(item, uriServicio);

                    if (cliente.codigoHTTP == 201)
                    {
                        Toast.MakeText(this, "POST realizado con exito", ToastLength.Long).Show();
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "error:" + ex, ToastLength.Short).Show();
                }
            };
        }

      
    }
}