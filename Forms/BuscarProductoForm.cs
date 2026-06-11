using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using zenith_v1.Models;

namespace zenith_v1.Forms
{
    public partial class BuscarProductoForm : Form
    {
        public Producto ProductoSeleccionado { get; private set; }

        private readonly List<Producto> productos;
        private TextBox txtBuscar;
        private DataGridView dgvProductos;

        private const string PlaceholderBuscar = "Ej: mouse, teclado, 12, BMW...";

        private readonly Color Fondo = Color.FromArgb(5, 7, 13);
        private readonly Color Card = Color.FromArgb(17, 24, 39);
        private readonly Color Borde = Color.FromArgb(31, 41, 55);
        private readonly Color Texto = Color.FromArgb(248, 250, 252);
        private readonly Color TextoSuave = Color.FromArgb(148, 163, 184);
        private readonly Color Celeste = Color.FromArgb(15, 207, 217);
        private readonly Color Morado = Color.FromArgb(124, 58, 237);
        private readonly Color Rojo = Color.FromArgb(239, 68, 68);

        public BuscarProductoForm(List<Producto> productos)
        {
            this.productos = productos ?? new List<Producto>();

            Text = "Buscar producto";
            Width = 900;
            Height = 600;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Fondo;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            ConstruirUI();
            CargarProductos(this.productos);
        }

        private void ConstruirUI()
        {
            Panel contenedor = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(28),
                BackColor = Fondo
            };

            Controls.Add(contenedor);

            Label titulo = new Label
            {
                Text = "Buscar producto",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 55
            };

            Label subtitulo = new Label
            {
                Text = "Busca por nombre, ID, marca o descripción.",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10.5F),
                Dock = DockStyle.Top,
                Height = 35
            };

            txtBuscar = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 45,
                Font = new Font("Segoe UI", 12F),
                BackColor = Color.FromArgb(11, 18, 32),
                ForeColor = TextoSuave,
                BorderStyle = BorderStyle.FixedSingle,
                Text = PlaceholderBuscar
            };

            txtBuscar.GotFocus += (s, e) =>
            {
                if (txtBuscar.Text == PlaceholderBuscar)
                {
                    txtBuscar.Text = "";
                    txtBuscar.ForeColor = Texto;
                }
            };

            txtBuscar.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    txtBuscar.Text = PlaceholderBuscar;
                    txtBuscar.ForeColor = TextoSuave;
                }
            };

            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            dgvProductos = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Card,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 28, 43);
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            dgvProductos.DefaultCellStyle.BackColor = Color.FromArgb(11, 18, 32);
            dgvProductos.DefaultCellStyle.ForeColor = Texto;
            dgvProductos.DefaultCellStyle.SelectionBackColor = Morado;
            dgvProductos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProductos.GridColor = Borde;

            dgvProductos.Columns.Add("Id", "ID");
            dgvProductos.Columns.Add("Nombre", "Producto");
            dgvProductos.Columns.Add("Marca", "Marca");
            dgvProductos.Columns.Add("Stock", "Stock");
            dgvProductos.Columns.Add("Precio", "Precio");

            dgvProductos.Columns["Id"].FillWeight = 12;
            dgvProductos.Columns["Nombre"].FillWeight = 42;
            dgvProductos.Columns["Marca"].FillWeight = 18;
            dgvProductos.Columns["Stock"].FillWeight = 12;
            dgvProductos.Columns["Precio"].FillWeight = 16;

            dgvProductos.CellDoubleClick += DgvProductos_CellDoubleClick;

            FlowLayoutPanel acciones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 75,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 18, 0, 0)
            };

            Button btnSeleccionar = CrearBoton("Seleccionar producto", Celeste);
            btnSeleccionar.Click += BtnSeleccionar_Click;

            Button btnCancelar = CrearBoton("Cancelar", Rojo);
            btnCancelar.Click += (s, e) => Close();

            acciones.Controls.Add(btnSeleccionar);
            acciones.Controls.Add(btnCancelar);

            contenedor.Controls.Add(dgvProductos);
            contenedor.Controls.Add(acciones);
            contenedor.Controls.Add(txtBuscar);
            contenedor.Controls.Add(subtitulo);
            contenedor.Controls.Add(titulo);
        }

        private Button CrearBoton(string texto, Color color)
        {
            Button btn = new Button
            {
                Text = texto,
                Width = 180,
                Height = 48,
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(12, 0, 0, 0)
            };

            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void CargarProductos(List<Producto> lista)
        {
            dgvProductos.Rows.Clear();

            foreach (var p in lista)
            {
                dgvProductos.Rows.Add(
                    p.id,
                    p.nombre,
                    p.marca,
                    p.stock,
                    "$" + p.precio.ToString("N0")
                );
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (txtBuscar.Text == PlaceholderBuscar)
            {
                CargarProductos(productos);
                return;
            }

            string texto = txtBuscar.Text.Trim().ToLower();

            var filtrados = productos.Where(p =>
                p.id.ToString().Contains(texto) ||
                (!string.IsNullOrEmpty(p.nombre) && p.nombre.ToLower().Contains(texto)) ||
                (!string.IsNullOrEmpty(p.marca) && p.marca.ToLower().Contains(texto)) ||
                (!string.IsNullOrEmpty(p.descripcion) && p.descripcion.ToLower().Contains(texto))
            ).ToList();

            CargarProductos(filtrados);
        }

        private void BtnSeleccionar_Click(object sender, EventArgs e)
        {
            SeleccionarProducto();
        }

        private void DgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SeleccionarProducto();
        }

        private void SeleccionarProducto()
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un producto.");
                return;
            }

            int id = Convert.ToInt32(dgvProductos.CurrentRow.Cells["Id"].Value);

            ProductoSeleccionado = productos.FirstOrDefault(p => p.id == id);

            if (ProductoSeleccionado == null)
            {
                MessageBox.Show("No se pudo seleccionar el producto.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}