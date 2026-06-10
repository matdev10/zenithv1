using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace zenith_v1.Forms
{
    public class UserClientes : UserControl
    {
        private DataGridView tablaClientes;
        private Label lblEstado;

        private readonly HttpClient httpClient = new HttpClient();

        private readonly Color Fondo = Color.FromArgb(5, 7, 13);
        private readonly Color Card = Color.FromArgb(17, 24, 39);
        private readonly Color Borde = Color.FromArgb(31, 41, 55);
        private readonly Color Texto = Color.FromArgb(248, 250, 252);
        private readonly Color TextoSuave = Color.FromArgb(148, 163, 184);
        private readonly Color Morado = Color.FromArgb(124, 58, 237);

        public UserClientes()
        {
            Dock = DockStyle.Fill;
            BackColor = Fondo;

            ConstruirUI();
            _ = CargarClientes();
        }

        private void ConstruirUI()
        {
            Controls.Clear();

            Label titulo = new Label
            {
                Text = "Clientes",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 55
            };

            lblEstado = new Label
            {
                Text = "Cargando clientes...",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10.5F),
                Dock = DockStyle.Top,
                Height = 32
            };

            Panel panel = CrearPanelRedondeado();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(22);

            tablaClientes = new DataGridView
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

            tablaClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 28, 43);
            tablaClientes.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            tablaClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            tablaClientes.DefaultCellStyle.BackColor = Color.FromArgb(11, 18, 32);
            tablaClientes.DefaultCellStyle.ForeColor = Texto;
            tablaClientes.DefaultCellStyle.SelectionBackColor = Morado;
            tablaClientes.DefaultCellStyle.SelectionForeColor = Color.White;
            tablaClientes.GridColor = Borde;

            panel.Controls.Add(tablaClientes);

            Controls.Add(panel);
            Controls.Add(lblEstado);
            Controls.Add(titulo);
        }

        private async Task CargarClientes()
        {
            try
            {
                string url = "http://127.0.0.1:8000/api/clientes/";

                string json = await httpClient.GetStringAsync(url);

                JArray clientes = JArray.Parse(json);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("Documento");
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Apellido");
                dt.Columns.Add("RUT");
                dt.Columns.Add("Email");
                dt.Columns.Add("Teléfono");
                dt.Columns.Add("Comuna");

                foreach (var c in clientes)
                {
                    dt.Rows.Add(
                        c["id"]?.ToString(),
                        c["numero_documento"]?.ToString(),
                        c["nombre"]?.ToString(),
                        c["apellido"]?.ToString(),
                        c["rut"]?.ToString(),
                        c["email"]?.ToString(),
                        c["telefono"]?.ToString(),
                        c["comuna"]?.ToString()
                    );
                }

                tablaClientes.DataSource = dt;
                lblEstado.Text = $"Clientes cargados: {clientes.Count}";
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error al cargar clientes.";
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
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