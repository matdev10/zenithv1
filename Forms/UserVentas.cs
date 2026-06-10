using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using zenith_v1.Models;
using zenith_v1.Services;




namespace zenith_v1.Forms
{
    public class UserVentas : UserControl
    {

        private Producto productoActual;
        private List<DetalleVentaItem> carrito;
        private ProductoService productoService;
        private VentaService ventaService;

        private TextBox txtCodigo;
        private TextBox txtCantidad;
        private Label lblProductoSeleccionado;
        private DataGridView tablaVenta;
        private Label lblTotal;

        private TextBox txtRutCliente;
        private TextBox txtNombreCliente;
        private ComboBox cmbTipoDocumento;


        private readonly Color Fondo = Color.FromArgb(5, 7, 13);
        private readonly Color Card = Color.FromArgb(17, 24, 39);
        private readonly Color Borde = Color.FromArgb(31, 41, 55);
        private readonly Color Texto = Color.FromArgb(248, 250, 252);
        private readonly Color TextoSuave = Color.FromArgb(148, 163, 184);
        private readonly Color Celeste = Color.FromArgb(15, 207, 217);
        private readonly Color Morado = Color.FromArgb(124, 58, 237);
        private readonly Color Verde = Color.FromArgb(34, 197, 94);
        private readonly Color Rojo = Color.FromArgb(239, 68, 68);

        public UserVentas()
        {
            Dock = DockStyle.Fill;
            BackColor = Fondo;


            productoService = new ProductoService();
            ventaService = new VentaService();
            carrito = new List<DetalleVentaItem>();


            ConstruirUI();
        }

        private void ConstruirUI()
        {
            Controls.Clear();

            Label titulo = new Label
            {
                Text = "Nueva venta",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50
            };

            Label subtitulo = new Label
            {
                Text = "Busca productos, agrega cantidades y registra ventas.",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10.5F),
                Dock = DockStyle.Top,
                Height = 32
            };

            Controls.Add(subtitulo);
            Controls.Add(titulo);

            Panel layout = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 20, 0, 0),
                BackColor = Fondo
            };

            Controls.Add(layout);
            layout.BringToFront();

            Panel izquierda = CrearPanelRedondeado();
            izquierda.Dock = DockStyle.Left;
            izquierda.Width = 390;
            izquierda.Padding = new Padding(22);
            layout.Controls.Add(izquierda);

            Panel derecha = CrearPanelRedondeado();
            derecha.Dock = DockStyle.Fill;
            derecha.Padding = new Padding(22);
            derecha.Margin = new Padding(20, 0, 0, 0);
            layout.Controls.Add(derecha);

            ConstruirPanelBusqueda(izquierda);
            ConstruirPanelDetalleVenta(derecha);

        }







        private void ConstruirPanelBusqueda(Panel panel)
        {
            panel.Controls.Clear();

            Button btnAgregar = CrearBoton("Agregar al carrito", Morado);
            btnAgregar.Dock = DockStyle.Top;
            btnAgregar.Height = 48;
            btnAgregar.Click += BtnAgregar_Click;

            txtCantidad = CrearTextBox("Cantidad");
            txtCantidad.Dock = DockStyle.Top;

            cmbTipoDocumento = new ComboBox
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(11, 18, 32),
                ForeColor = Texto,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbTipoDocumento.Items.Add("Boleta");
            cmbTipoDocumento.Items.Add("Factura");
            cmbTipoDocumento.SelectedIndex = 0;

            txtNombreCliente = CrearTextBox("Nombre completo del cliente");
            txtNombreCliente.Dock = DockStyle.Top;

            txtRutCliente = CrearTextBox("RUT cliente");
            txtRutCliente.Dock = DockStyle.Top;

            Label tituloCliente = new Label
            {
                Text = "Datos del cliente",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 42
            };

            lblProductoSeleccionado = new Label
            {
                Text = "Producto seleccionado:\n\nSin producto seleccionado.",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10.5F),
                Dock = DockStyle.Top,
                Height = 115,
                Padding = new Padding(0, 18, 0, 0)
            };

            Button btnBuscar = CrearBoton("Buscar producto", Celeste);
            btnBuscar.Dock = DockStyle.Top;
            btnBuscar.Height = 48;
            btnBuscar.Click += BtnBuscar_Click;

            txtCodigo = CrearTextBox("Código o ID del producto");
            txtCodigo.Dock = DockStyle.Top;

            Label tituloBuscar = new Label
            {
                Text = "Buscar producto",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 42
            };

            panel.Controls.Add(btnAgregar);
            panel.Controls.Add(txtCantidad);
            panel.Controls.Add(cmbTipoDocumento);
            panel.Controls.Add(txtNombreCliente);
            panel.Controls.Add(txtRutCliente);
            panel.Controls.Add(tituloCliente);
            panel.Controls.Add(lblProductoSeleccionado);
            panel.Controls.Add(btnBuscar);
            panel.Controls.Add(txtCodigo);
            panel.Controls.Add(tituloBuscar);
        }






        private async void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo.Text.Trim();

                if (codigo == "Código o ID del producto" || string.IsNullOrWhiteSpace(codigo))
                {
                    MessageBox.Show("Ingresa un ID de producto.");
                    return;
                }

                if (!int.TryParse(codigo, out int productoId))
                {
                    MessageBox.Show("El ID del producto debe ser numérico.");
                    return;
                }

                var productos = await productoService.ObtenerProductosAsync();

                productoActual = productos.FirstOrDefault(p => p.id == productoId);

                if (productoActual == null)
                {
                    lblProductoSeleccionado.Text = "Producto seleccionado:\n\nProducto no encontrado.";
                    return;
                }

                lblProductoSeleccionado.Text =
                    $"Producto seleccionado:\n\n" +
                    $"{productoActual.nombre}\n" +
                    $"Precio: ${productoActual.precio:N0}\n" +
                    $"Stock: {productoActual.stock}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar producto: " + ex.Message);
            }
        }






        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (productoActual == null)
            {
                MessageBox.Show("Primero busca un producto.");
                return;
            }

            string cantidadTexto = txtCantidad.Text.Trim();

            if (cantidadTexto == "Cantidad" || string.IsNullOrWhiteSpace(cantidadTexto))
            {
                MessageBox.Show("Ingresa una cantidad.");
                return;
            }

            if (!int.TryParse(cantidadTexto, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a 0.");
                return;
            }

            if (cantidad > productoActual.stock)
            {
                MessageBox.Show("No hay stock suficiente.");
                return;
            }

            int subtotal = productoActual.precio * cantidad;

            tablaVenta.Rows.Add(
               productoActual.id,
               productoActual.nombre,
               productoActual.precio.ToString("N0"),
                cantidad,
                subtotal.ToString("N0")
             );

            ActualizarTotal();

            txtCantidad.Text = "Cantidad";
            txtCantidad.ForeColor = TextoSuave;
        }



        private void ActualizarTotal()
        {
            int total = 0;

            foreach (DataGridViewRow row in tablaVenta.Rows)
            {
                if (row.Cells["Subtotal"].Value != null)
                {
                    string valor = row.Cells["Subtotal"].Value.ToString().Replace(".", "").Replace(",", "");

                    if (int.TryParse(valor, out int subtotal))
                    {
                        total += subtotal;
                    }
                }
            }

            lblTotal.Text = $"Total: ${total:N0}";
        }




        private void ConstruirPanelDetalleVenta(Panel panel)
        {
            Label titulo = new Label
            {
                Text = "Detalle de venta",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40
            };

            panel.Controls.Add(titulo);

            tablaVenta = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 280,
                BackgroundColor = Card,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tablaVenta.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 28, 43);
            tablaVenta.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            tablaVenta.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            tablaVenta.DefaultCellStyle.BackColor = Color.FromArgb(11, 18, 32);
            tablaVenta.DefaultCellStyle.ForeColor = Texto;
            tablaVenta.DefaultCellStyle.SelectionBackColor = Morado;
            tablaVenta.DefaultCellStyle.SelectionForeColor = Color.White;
            tablaVenta.GridColor = Borde;

            tablaVenta.Columns.Clear();

            tablaVenta.Columns.Add("ProductoId", "ProductoId");
            tablaVenta.Columns["ProductoId"].Visible = false;

            tablaVenta.Columns.Add("Producto", "Producto");
            tablaVenta.Columns.Add("Precio", "Precio");
            tablaVenta.Columns.Add("Cantidad", "Cantidad");
            tablaVenta.Columns.Add("Subtotal", "Subtotal");

            tablaVenta.Columns["Producto"].FillWeight = 45;
            tablaVenta.Columns["Precio"].FillWeight = 20;
            tablaVenta.Columns["Cantidad"].FillWeight = 15;
            tablaVenta.Columns["Subtotal"].FillWeight = 20;

            panel.Controls.Add(tablaVenta);

            lblTotal = new Label
            {
                Text = "Total: $0",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 70,
                TextAlign = ContentAlignment.MiddleRight
            };

            panel.Controls.Add(lblTotal);

            FlowLayoutPanel acciones = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 75,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };

            panel.Controls.Add(acciones);

            Button btnRegistrar = CrearBotonAncho("Registrar venta", Verde);
            btnRegistrar.Click += BtnRegistrar_Click;

            Button btnLimpiar = CrearBotonAncho("Limpiar", Rojo);
            btnLimpiar.Click += BtnLimpiar_Click;

            acciones.Controls.Add(btnRegistrar);
            acciones.Controls.Add(btnLimpiar);
        }







        private TextBox CrearTextBox(string placeholder)
        {
            TextBox txt = new TextBox
            {
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

        private Button CrearBotonAncho(string texto, Color color)
        {
            Button btn = CrearBoton(texto, color);
            btn.Width = 160;
            btn.Height = 52;
            btn.Margin = new Padding(12, 12, 0, 0);
            return btn;
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


        private async void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tablaVenta.Rows.Count == 0)
                {
                    MessageBox.Show("Primero agrega un producto al carrito.");
                    return;
                }

                string rut = txtRutCliente.Text.Trim();
                string nombre = txtNombreCliente.Text.Trim();
                string tipoDocumento = cmbTipoDocumento.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(rut) || rut == "RUT cliente")
                {
                    MessageBox.Show("Ingresa el RUT del cliente.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(nombre) || nombre == "Nombre completo del cliente")
                {
                    MessageBox.Show("Ingresa el nombre del cliente.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(tipoDocumento))
                {
                    MessageBox.Show("Selecciona el tipo de documento.");
                    return;
                }

                var detalles = new List<object>();

                foreach (DataGridViewRow row in tablaVenta.Rows)
                {
                    if (row.IsNewRow) continue;

                    detalles.Add(new
                    {
                        producto_id = Convert.ToInt32(row.Cells["ProductoId"].Value),
                        cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value)
                    });
                }

                if (detalles.Count == 0)
                {
                    MessageBox.Show("No hay productos válidos en el carrito.");
                    return;
                }

                var ventaData = new
                {
                    tipo_documento = tipoDocumento.ToUpper(),
                    cliente = new
                    {
                        numero_documento = rut,
                        nombre_completo = nombre
                    },
                    detalles = detalles
                };

                string respuesta = await ventaService.CrearVentaAsync(ventaData);

                MessageBox.Show("Venta registrada correctamente.");

                LimpiarVenta();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar venta: " + ex.Message);
            }
        }






        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarVenta();
        }

        private void LimpiarVenta()
        {
            tablaVenta.Rows.Clear();

            lblTotal.Text = "Total: $0";

            txtRutCliente.Text = "RUT cliente";
            txtRutCliente.ForeColor = TextoSuave;

            txtNombreCliente.Text = "Nombre completo del cliente";
            txtNombreCliente.ForeColor = TextoSuave;

            txtCodigo.Text = "Código o ID del producto";
            txtCodigo.ForeColor = TextoSuave;

            txtCantidad.Text = "Cantidad";
            txtCantidad.ForeColor = TextoSuave;

            lblProductoSeleccionado.Text =
                "Producto seleccionado:\n\nSin producto seleccionado.";

            productoActual = null;
        }
    }

}