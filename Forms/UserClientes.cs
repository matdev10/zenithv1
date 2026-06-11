using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zenith_v1.Forms
{
    public class UserClientes : UserControl
    {
        private DataGridView tablaClientes;
        private Label lblEstado;

        private TextBox txtDocumento;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtRut;
        private TextBox txtEmail;
        private TextBox txtTelefono;
        private TextBox txtComuna;

        private readonly HttpClient httpClient = new HttpClient();

        private readonly Color Fondo = Color.FromArgb(5, 7, 13);
        private readonly Color Card = Color.FromArgb(17, 24, 39);
        private readonly Color Borde = Color.FromArgb(31, 41, 55);
        private readonly Color Texto = Color.FromArgb(248, 250, 252);
        private readonly Color TextoSuave = Color.FromArgb(148, 163, 184);
        private readonly Color Morado = Color.FromArgb(124, 58, 237);
        private readonly Color Celeste = Color.FromArgb(15, 207, 217);
        private readonly Color Verde = Color.FromArgb(34, 197, 94);

        private readonly string apiClientes = "http://127.0.0.1:8000/api/clientes/";

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

            Panel contenedor = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(32, 24, 32, 32),
                BackColor = Fondo
            };

            Controls.Add(contenedor);

            Label titulo = new Label
            {
                Text = "Clientes",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 58
            };

            lblEstado = new Label
            {
                Text = "Cargando clientes...",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 11F),
                Dock = DockStyle.Top,
                Height = 38
            };

            Panel layout = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 24, 0, 0),
                BackColor = Fondo
            };

            contenedor.Controls.Add(layout);
            contenedor.Controls.Add(lblEstado);
            contenedor.Controls.Add(titulo);

            Panel panelRegistro = CrearPanelRedondeado();
            panelRegistro.Dock = DockStyle.Left;
            panelRegistro.Width = 410;
            panelRegistro.Padding = new Padding(26);
            panelRegistro.AutoScroll = true;

            Panel separador = new Panel
            {
                Dock = DockStyle.Left,
                Width = 24,
                BackColor = Fondo
            };

            Panel panelListado = CrearPanelRedondeado();
            panelListado.Dock = DockStyle.Fill;
            panelListado.Padding = new Padding(26);

            layout.Controls.Add(panelListado);
            layout.Controls.Add(separador);
            layout.Controls.Add(panelRegistro);

            ConstruirPanelRegistro(panelRegistro);
            ConstruirPanelListado(panelListado);
        }

        private void ConstruirPanelRegistro(Panel panel)
        {
            Label titulo = CrearTituloSeccion("Registrar cliente");
            Label ayuda = CrearTextoAyuda("Crea clientes para ventas, boletas e historial.");

            txtDocumento = CrearTextBox("Número documento");
            txtNombre = CrearTextBox("Nombre");
            txtApellido = CrearTextBox("Apellido");
            txtRut = CrearTextBox("RUT");
            txtEmail = CrearTextBox("Email");
            txtTelefono = CrearTextBox("Teléfono");
            txtComuna = CrearTextBox("Comuna");

            Button btnGuardar = CrearBoton("Registrar cliente", Verde);
            btnGuardar.Dock = DockStyle.Top;
            btnGuardar.Height = 54;
            btnGuardar.Click += BtnGuardar_Click;

            panel.Controls.Add(btnGuardar);
            panel.Controls.Add(CrearSeparador(16));
            panel.Controls.Add(txtComuna);
            panel.Controls.Add(txtTelefono);
            panel.Controls.Add(txtEmail);
            panel.Controls.Add(txtRut);
            panel.Controls.Add(txtApellido);
            panel.Controls.Add(txtNombre);
            panel.Controls.Add(txtDocumento);
            panel.Controls.Add(ayuda);
            panel.Controls.Add(titulo);
        }


















        private void ConstruirPanelListado(Panel panel)
        {
            Label titulo = CrearTituloSeccion("Listado de clientes");

            FlowLayoutPanel toolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 58,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 8, 0, 0)
            };

            TextBox txtBuscarCliente = CrearTextBox("Buscar cliente...");
            txtBuscarCliente.Width = 260;
            txtBuscarCliente.Dock = DockStyle.None;

            Button btnNuevo = CrearBoton("➕ Nuevo", Celeste);
            btnNuevo.Width = 120;
            btnNuevo.Click += (s, e) => LimpiarFormulario();

            Button btnEditar = CrearBoton("✏️ Editar", Morado);
            btnEditar.Width = 120;

            Button btnEliminar = CrearBoton("🗑️ Eliminar", Color.FromArgb(239, 68, 68));
            btnEliminar.Width = 130;

            toolbar.Controls.Add(txtBuscarCliente);
            toolbar.Controls.Add(btnNuevo);
            toolbar.Controls.Add(btnEditar);
            toolbar.Controls.Add(btnEliminar);

            tablaClientes = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Card,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                ScrollBars = ScrollBars.Both,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            tablaClientes.ColumnHeadersHeight = 42;
            tablaClientes.RowTemplate.Height = 38;

            tablaClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 28, 43);
            tablaClientes.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            tablaClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            tablaClientes.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            tablaClientes.DefaultCellStyle.BackColor = Color.FromArgb(11, 18, 32);
            tablaClientes.DefaultCellStyle.ForeColor = Texto;
            tablaClientes.DefaultCellStyle.SelectionBackColor = Morado;
            tablaClientes.DefaultCellStyle.SelectionForeColor = Color.White;
            tablaClientes.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            tablaClientes.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            tablaClientes.GridColor = Borde;

            panel.Controls.Add(tablaClientes);
            panel.Controls.Add(toolbar);
            panel.Controls.Add(titulo);
        }





















        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string documento = ObtenerValor(txtDocumento, "Número documento");
                string nombre = ObtenerValor(txtNombre, "Nombre");
                string apellido = ObtenerValor(txtApellido, "Apellido");
                string rut = ObtenerValor(txtRut, "RUT");
                string email = ObtenerValor(txtEmail, "Email");
                string telefono = ObtenerValor(txtTelefono, "Teléfono");
                string comuna = ObtenerValor(txtComuna, "Comuna");

                if (string.IsNullOrWhiteSpace(documento))
                {
                    MessageBox.Show("Ingresa el número de documento.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    MessageBox.Show("Ingresa el nombre del cliente.");
                    return;
                }

                var clienteData = new
                {
                    numero_documento = documento,
                    nombre = nombre,
                    apellido = apellido,
                    rut = rut,
                    email = email,
                    telefono = telefono,
                    comuna = comuna
                };

                string json = JsonConvert.SerializeObject(clienteData);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response =
                    await httpClient.PostAsync(apiClientes + "crear/", content);

                string respuesta = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al registrar cliente: " + respuesta);
                    return;
                }

                MessageBox.Show("Cliente registrado correctamente.");

                LimpiarFormulario();
                await CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar cliente: " + ex.Message);
            }
        }

        private async Task CargarClientes()
        {
            try
            {
                string json = await httpClient.GetStringAsync(
                    apiClientes + "?t=" + DateTime.Now.Ticks
                );

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

                // Tamaños de columnas
                tablaClientes.Columns["ID"].Width = 60;
                tablaClientes.Columns["Documento"].Width = 140;
                tablaClientes.Columns["Nombre"].Width = 180;
                tablaClientes.Columns["Apellido"].Width = 180;
                tablaClientes.Columns["RUT"].Width = 140;
                tablaClientes.Columns["Email"].Width = 260;
                tablaClientes.Columns["Teléfono"].Width = 140;
                tablaClientes.Columns["Comuna"].Width = 160;

                // Altura de filas y encabezados
                tablaClientes.RowTemplate.Height = 38;
                tablaClientes.ColumnHeadersHeight = 42;

                // Espaciado interno
                tablaClientes.DefaultCellStyle.Padding =
                    new Padding(8, 0, 8, 0);

                tablaClientes.ColumnHeadersDefaultCellStyle.Padding =
                    new Padding(8, 0, 8, 0);

                lblEstado.Text =
                    $"Clientes cargados: {clientes.Count}";
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error al cargar clientes.";
                MessageBox.Show(
                    "Error al cargar clientes: " + ex.Message
                );
            }
        }

        private string ObtenerValor(TextBox txt, string placeholder)
        {
            if (txt.Text == placeholder)
                return "";

            return txt.Text.Trim();
        }

        private void LimpiarFormulario()
        {
            ResetTextBox(txtDocumento, "Número documento");
            ResetTextBox(txtNombre, "Nombre");
            ResetTextBox(txtApellido, "Apellido");
            ResetTextBox(txtRut, "RUT");
            ResetTextBox(txtEmail, "Email");
            ResetTextBox(txtTelefono, "Teléfono");
            ResetTextBox(txtComuna, "Comuna");
        }

        private void ResetTextBox(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = TextoSuave;
        }

        private TextBox CrearTextBox(string placeholder)
        {
            TextBox txt = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(11, 18, 32),
                ForeColor = TextoSuave,
                BorderStyle = BorderStyle.FixedSingle,
                Text = placeholder
            };

            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Texto;
                }
            };

            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = TextoSuave;
                }
            };

            return txt;
        }

        private Button CrearBoton(string texto, Color color)
        {
            Button btn = new Button
            {
                Text = texto,
                Height = 48,
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private Label CrearTituloSeccion(string texto)
        {
            return new Label
            {
                Text = texto,
                ForeColor = Texto,
                Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 44
            };
        }

        private Label CrearTextoAyuda(string texto)
        {
            return new Label
            {
                Text = texto,
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 9.5F),
                Dock = DockStyle.Top,
                Height = 34
            };
        }

        private Panel CrearSeparador(int alto)
        {
            return new Panel
            {
                Dock = DockStyle.Top,
                Height = alto,
                BackColor = Card
            };
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