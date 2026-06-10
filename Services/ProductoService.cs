using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using zenith_v1.Models;
using Newtonsoft.Json;

namespace zenith_v1.Services
{
    internal class ProductoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url = "https://zeezton.cl/api/productos/";

        public ProductoService()
        {
            _httpClient = new HttpClient();
        }

        // 🔥 Obtener TODOS los productos
        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_url);

                if (!response.IsSuccessStatusCode)
                    return new List<Producto>();

                string json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<Producto>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener productos: " + ex.Message);
            }
        }

        // 🔥 Buscar producto por ID (desde la lista)
        public async Task<Producto> ObtenerProductoPorIdAsync(int id)
        {
            try
            {
                var productos = await ObtenerProductosAsync();

                foreach (var producto in productos)
                {
                    if (producto.id == id)
                        return producto;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar producto: " + ex.Message);
            }
        }
    }
}
