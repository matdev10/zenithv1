using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace zenith_v1.Services
{
    internal class VentaService
    {
        private readonly HttpClient _httpClient;

        // API LOCAL DJANGO
        private readonly string _url =
            "http://127.0.0.1:8000/api/ventas/";

        public VentaService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> CrearVentaAsync(object ventaData)
        {
            try
            {
                string json =
                    JsonConvert.SerializeObject(ventaData);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response =
                    await _httpClient.PostAsync(_url, content);

                string respuesta =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        "Error de API: " + respuesta
                    );
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error al crear venta: " + ex.Message
                );
            }
        }

        public async Task<string> ObtenerDetalleVentasAsync()
        {
            try
            {
                string url =
                    "http://127.0.0.1:8000/api/detalle-ventas/";

                HttpResponseMessage response =
                    await _httpClient.GetAsync(url);

                string resultado =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        "Error API: " + resultado
                    );
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error al obtener detalle ventas: " + ex.Message
                );
            }
        }
    }
}