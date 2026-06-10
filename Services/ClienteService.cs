using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace zenith_v1.Services
{
    internal class ClienteService
    {
        private readonly HttpClient client;

        public ClienteService()
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
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
                    "https://zeezton.cl/api/clientes/crear/",
                    content
                );

                string respuesta = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return "{\"error\":\"Error al crear cliente\"}";
                }

                return respuesta;
            }
            catch
            {
                return "{\"error\":\"Error de conexión\"}";
            }
        }

        public async Task<string> BuscarClientePorRutAsync(string rut)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rut))
                    return "{\"error\":\"RUT vacío\"}";

                string url =
                    "https://zeezton.cl/api/clientes/buscar/?rut="
                    + Uri.EscapeDataString(rut.Trim());

                var response = await client.GetAsync(url);

                string contenido = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return "{\"error\":\"Cliente no encontrado\"}";
                }

                return contenido;
            }
            catch (TaskCanceledException)
            {
                return "{\"error\":\"Tiempo de espera agotado\"}";
            }
            catch
            {
                return "{\"error\":\"Error de conexión\"}";
            }
        }
    }
}