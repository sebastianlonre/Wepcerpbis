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



        public class entrada
        {
            //constructor para iniciar valores

            public entrada()
            {
                userId = 1;
                id = 0;
                tittle = "";
                body = "";
            }

            public int userId { get; set; }
            public int id { get; set; }
            public string tittle { get; set; }
            public string body { get; set; }
        }

        // cuando una peticion se envia hay un cliente (nuestro equipo) y se le envia al servidor (web) retorna un codigo como respuesta
        public class cliente
        {
            public cliente()
            {
                //respuestas positivas 200, 400 (problemas cliente) 0 500 (problemas servidor)

                //consulta exitosa 200
                codigoHTTP = 200;
            }

            public int codigoHTTP { get; set; }

            //Get recibir informacion

            public async Task<T> Get<T>(string url)
            {
                HttpClient cliente = new HttpClient();
                //guardar la respuesta
                var response = await cliente.GetAsync(url);
                //traducir la respuesta
                var json = await response.Content.ReadAsStringAsync();
                codigoHTTP = (int)response.StatusCode;
                return JsonConvert.DeserializeObject<T>(json);

            }

            //post enviar
            public async Task<T> Post<T>(entrada item, string url)
            {
                HttpClient cliente = new HttpClient();
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await cliente.PostAsync(url, content);
                json = await response.Content.ReadAsStringAsync();
                codigoHTTP = (int)response.StatusCode;
                return JsonConvert.DeserializeObject<T>(json);
            }

        }
    }
}