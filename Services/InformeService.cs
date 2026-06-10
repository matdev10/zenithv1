using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace zenith_v1.Services
{
    internal class InformeService
    {
        public async Task<string> CrearInformeAsync(object data)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://zeezton.cl/api/informes/", content);

                return await response.Content.ReadAsStringAsync();
            }
        }
        public async Task<string> ObtenerInformesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync("https://zeezton.cl/api/informes/listar/");
            }
        }
    }
}
