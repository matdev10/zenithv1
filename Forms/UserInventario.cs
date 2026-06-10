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
    public class UserInventario : UserControl
    {
        private DataGridView tablaInventario;
        private Label lblEstado;
        private ProductoService productoService;

        private readonly Color Fondo = Color.FromArgb(5, 7, 13);
        private readonly Color Card = Color.FromArgb(17, 24, 39);
        private readonly Color Borde = Color.FromArgb(31, 41, 55);
        private readonly Color Texto = Color.FromArgb(248, 250, 252);
        private readonly Color TextoSuave = Color.FromArgb(148, 163, 184);
        private readonly Color Morado = Color.FromArgb(124, 58, 237);

        public UserInventario()
        {
            Dock = DockStyle.Fill;
            BackColor = Fondo;
            productoService = new ProductoService();

            ConstruirUI();
            _ = CargarInventario();
        }

        private void ConstruirUI()
        {
            Controls.Clear();

            Label titulo = new Label
            {
                Text = "Inventario",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 55
            };

            lblEstado = new Label
            {
                Text = "Cargando productos...",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10.5F),
                Dock = DockStyle.Top,
                Height = 32
            };

            Panel panel = CrearPanelRedondeado();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(22);

            tablaInventario = new DataGridView
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

            tablaInventario.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 28, 43);
            tablaInventario.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            tablaInventario.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            tablaInventario.DefaultCellStyle.BackColor = Color.FromArgb(11, 18, 32);
            tablaInventario.DefaultCellStyle.ForeColor = Texto;
            tablaInventario.DefaultCellStyle.SelectionBackColor = Morado;
            tablaInventario.DefaultCellStyle.SelectionForeColor = Color.White;
            tablaInventario.GridColor = Borde;

            tablaInventario.CellFormatting += TablaInventario_CellFormatting;

            panel.Controls.Add(tablaInventario);

            Controls.Add(panel);
            Controls.Add(lblEstado);
            Controls.Add(titulo);
        }

        private async Task CargarInventario()
        {
            try
            {
                var productos = await productoService.ObtenerProductosAsync();

                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("Producto");
                dt.Columns.Add("Marca");
                dt.Columns.Add("Precio");
                dt.Columns.Add("Stock");
                dt.Columns.Add("Oferta");
                dt.Columns.Add("Destacado");

                foreach (var p in productos)
                {
                    dt.Rows.Add(
                        p.id,
                        p.nombre,
                        p.marca,
                        "$" + p.precio.ToString("N0"),
                        p.stock,
                        p.oferta ? "Sí" : "No",
                        p.destacado ? "Sí" : "No"
                    );
                }

                tablaInventario.DataSource = dt;
                lblEstado.Text = $"Productos cargados: {productos.Count}";
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error al cargar inventario.";
                MessageBox.Show("Error al cargar inventario: " + ex.Message);
            }
        }

        private void TablaInventario_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tablaInventario.Columns[e.ColumnIndex].Name == "Stock" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int stock))
                {
                    if (stock <= 2)
                    {
                        tablaInventario.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                            Color.FromArgb(74, 29, 29);
                    }
                }
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