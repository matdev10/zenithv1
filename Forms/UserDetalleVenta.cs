using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using zenith_v1.Services;

namespace zenith_v1.Forms
{
    public class UserDetalleVentas : UserControl
    {
        private DataGridView tabla;
        private Label lblEstado;
        private VentaService ventaService;

        private readonly Color Fondo = Color.FromArgb(5, 7, 13);
        private readonly Color Card = Color.FromArgb(17, 24, 39);
        private readonly Color Borde = Color.FromArgb(31, 41, 55);
        private readonly Color Texto = Color.FromArgb(248, 250, 252);
        private readonly Color TextoSuave = Color.FromArgb(148, 163, 184);
        private readonly Color Morado = Color.FromArgb(124, 58, 237);

        public UserDetalleVentas()
        {
            Dock = DockStyle.Fill;
            BackColor = Fondo;
            ventaService = new VentaService();

            ConstruirUI();
            _ = CargarDetalleVentas();
        }

        private void ConstruirUI()
        {
            Controls.Clear();

            Label titulo = new Label
            {
                Text = "Detalle de ventas",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 55
            };

            lblEstado = new Label
            {
                Text = "Cargando ventas...",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10.5F),
                Dock = DockStyle.Top,
                Height = 32
            };

            Panel panel = CrearPanelRedondeado();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(22);

            tabla = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Card,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tabla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 28, 43);
            tabla.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            tabla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            tabla.DefaultCellStyle.BackColor = Color.FromArgb(11, 18, 32);
            tabla.DefaultCellStyle.ForeColor = Texto;
            tabla.DefaultCellStyle.SelectionBackColor = Morado;
            tabla.DefaultCellStyle.SelectionForeColor = Color.White;
            tabla.GridColor = Borde;

            panel.Controls.Add(tabla);

            Controls.Add(panel);
            Controls.Add(lblEstado);
            Controls.Add(titulo);
        }

        private async Task CargarDetalleVentas()
        {
            try
            {
                string json = await ventaService.ObtenerDetalleVentasAsync();
                JArray ventas = JArray.Parse(json);

                DataTable dt = new DataTable();
                dt.Columns.Add("Venta ID");
                dt.Columns.Add("Fecha");
                dt.Columns.Add("Documento");
                dt.Columns.Add("Cliente");
                dt.Columns.Add("Producto");
                dt.Columns.Add("Cantidad");
                dt.Columns.Add("Precio");
                dt.Columns.Add("Subtotal");
                dt.Columns.Add("Total Venta");

                foreach (var v in ventas)
                {
                    dt.Rows.Add(
                        v["venta_id"]?.ToString(),
                        v["fecha"]?.ToString(),
                        v["tipo_documento"]?.ToString(),
                        v["cliente"]?.ToString(),
                        v["producto"]?.ToString(),
                        v["cantidad"]?.ToString(),
                        "$" + Convert.ToInt32(v["precio_unitario"]).ToString("N0"),
                        "$" + Convert.ToInt32(v["subtotal"]).ToString("N0"),
                        "$" + Convert.ToInt32(v["total_venta"]).ToString("N0")
                    );
                }

                tabla.DataSource = dt;
                lblEstado.Text = $"Ventas cargadas: {ventas.Count}";
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error al cargar ventas.";
                MessageBox.Show("Error al cargar detalle ventas: " + ex.Message);
            }
        }

        private Panel CrearPanelRedondeado()
        {
            Panel panel = new Panel
            {
                BackColor = Card
            };

            panel.Paint += (s, e) =>
            {
                int radio = 18;
                Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
                    path.AddArc(rect.Right - radio, rect.Y, radio, radio, 270, 90);
                    path.AddArc(rect.Right - radio, rect.Bottom - radio, radio, radio, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radio, radio, radio, 90, 90);
                    path.CloseFigure();

                    panel.Region = new Region(path);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    using (Pen pen = new Pen(Borde, 1))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            return panel;
        }
    }
}