using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Wepcerpbis
{
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