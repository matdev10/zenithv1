using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using zenith_v1.Models;

namespace zenith_v1.Forms
{
    public partial class BuscarClienteForm : Form
    {
        public Cliente ClienteSeleccionado { get; private set; }

        private readonly List<Cliente> clientes;

        private TextBox txtBuscar;
        private DataGridView dgvClientes;

        private readonly Color Fondo = Color.FromArgb(5, 7, 13);
        private readonly Color Card = Color.FromArgb(17, 24, 39);
        private readonly Color Borde = Color.FromArgb(31, 41, 55);
        private readonly Color Texto = Color.FromArgb(248, 250, 252);
        private readonly Color TextoSuave = Color.FromArgb(148, 163, 184);
        private readonly Color Celeste = Color.FromArgb(15, 207, 217);
        private readonly Color Morado = Color.FromArgb(124, 58, 237);

        public BuscarClienteForm(List<Cliente> clientes)
        {
            this.clientes = clientes ?? new List<Cliente>();

            Width = 950;
            Height = 620;

            StartPosition = FormStartPosition.CenterParent;

            BackColor = Fondo;

            FormBorderStyle = FormBorderStyle.FixedDialog;

            Text = "Buscar cliente";

            ConstruirUI();

            CargarClientes(this.clientes);
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
                Text = "Buscar cliente",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 55
            };

            txtBuscar = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(11, 18, 32),
                ForeColor = Texto,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Buscar por nombre o RUT"
            };

            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            dgvClientes = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Card,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(20, 28, 43);

            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor =
                Texto;

            dgvClientes.DefaultCellStyle.BackColor =
                Color.FromArgb(11, 18, 32);

            dgvClientes.DefaultCellStyle.ForeColor = Texto;

            dgvClientes.DefaultCellStyle.SelectionBackColor = Morado;

            dgvClientes.GridColor = Borde;

            dgvClientes.Columns.Add("Rut", "RUT");
            dgvClientes.Columns.Add("Nombre", "Nombre");
            dgvClientes.Columns.Add("Telefono", "Teléfono");
            dgvClientes.Columns.Add("Email", "Email");

            dgvClientes.CellDoubleClick += DgvClientes_CellDoubleClick;

            contenedor.Controls.Add(dgvClientes);
            contenedor.Controls.Add(txtBuscar);
            contenedor.Controls.Add(titulo);
        }

        private void CargarClientes(List<Cliente> lista)
        {
            dgvClientes.Rows.Clear();

            foreach (var c in lista)
            {
                dgvClientes.Rows.Add(
                    c.rut,
                    c.nombre + " " + c.apellido,
                    c.telefono,
                    c.email
                );
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            string texto = txtBuscar.Text.ToLower();

            var filtrados = clientes.Where(c =>
                (!string.IsNullOrEmpty(c.nombre) &&
                 c.nombre.ToLower().Contains(texto))
                ||
                (!string.IsNullOrEmpty(c.rut) &&
                 c.rut.ToLower().Contains(texto))
            ).ToList();

            CargarClientes(filtrados);
        }

        private void DgvClientes_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e
        )
        {
            if (dgvClientes.CurrentRow == null)
                return;

            string rut =
                dgvClientes.CurrentRow.Cells["Rut"].Value.ToString();

            ClienteSeleccionado =
                clientes.FirstOrDefault(c => c.rut == rut);

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}