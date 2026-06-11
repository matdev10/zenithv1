using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using zenith_v1.Models;

namespace zenith_v1.Services
{
    internal class ClienteService
    {
        private readonly HttpClient client;

        private readonly string baseUrl =
            "http://127.0.0.1:8000/api/clientes/";

        public ClienteService()
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            try
            {
                string url = baseUrl + "?t=" + DateTime.Now.Ticks;

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new List<Cliente>();

                string json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<Cliente>>(json);
            }
            catch
            {
                return new List<Cliente>();
            }
        }

        public async Task<string> CrearClienteAsync(object clienteData)
        {
            try
            {
                var json = JsonConvert.SerializeObject(clienteData);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(
                    baseUrl + "crear/",
                    content
                );

                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return "{\"error\":\"Error de conexión\"}";
            }
        }

        public async Task<Cliente> BuscarClientePorRutAsync(string rut)
        {
            try
            {
                string url =
                    baseUrl +
                    "buscar/?rut=" +
                    Uri.EscapeDataString(rut.Trim());

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                string json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Cliente>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}