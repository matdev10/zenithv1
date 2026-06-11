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
        private Cliente clienteActual;

        private List<DetalleVentaItem> carrito;

        private ProductoService productoService;
        private VentaService ventaService;
        private ClienteService clienteService;

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
            clienteService = new ClienteService();

            carrito = new List<DetalleVentaItem>();

            ConstruirUI();
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
                Text = "Nueva venta",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 58
            };

            Label subtitulo = new Label
            {
                Text = "Busca productos, agrega cantidades y registra ventas.",
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
            contenedor.Controls.Add(subtitulo);
            contenedor.Controls.Add(titulo);

            Panel izquierda = CrearPanelRedondeado();
            izquierda.Dock = DockStyle.Left;
            izquierda.Width = 430;
            izquierda.Padding = new Padding(26);
            izquierda.AutoScroll = true;

            Panel separador = new Panel
            {
                Dock = DockStyle.Left,
                Width = 24,
                BackColor = Fondo
            };

            Panel derecha = CrearPanelRedondeado();
            derecha.Dock = DockStyle.Fill;
            derecha.Padding = new Padding(28);

            layout.Controls.Add(derecha);
            layout.Controls.Add(separador);
            layout.Controls.Add(izquierda);

            ConstruirPanelBusqueda(izquierda);
            ConstruirPanelDetalleVenta(derecha);
        }






        private void ConstruirPanelBusqueda(Panel panel)
        {
            panel.Controls.Clear();

            Label titulo = CrearTituloSeccion("Buscar producto");
            Label ayuda = CrearTextoAyuda("Selecciona un producto desde el buscador avanzado.");

            Button btnBuscar = CrearBoton("Abrir buscador de productos", Celeste);
            btnBuscar.Dock = DockStyle.Top;
            btnBuscar.Height = 54;
            btnBuscar.Click += BtnBuscar_Click;

            lblProductoSeleccionado = new Label
            {
                Text = "Sin producto seleccionado.",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10.5F),
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(0, 14, 0, 0)
            };

            Panel separador1 = CrearSeparador(18);

            Label tituloCliente = CrearTituloSeccion("Datos del cliente");
            Label ayudaCliente = CrearTextoAyuda("Completa los datos para emitir boleta o factura.");

            txtRutCliente = CrearTextBox("RUT cliente");
            Button btnBuscarCliente = CrearBoton("Buscar cliente registrado", Celeste);
            btnBuscarCliente.Dock = DockStyle.Top;
            btnBuscarCliente.Height = 52;
            btnBuscarCliente.Click += BtnBuscarCliente_Click;
            txtRutCliente.Dock = DockStyle.Top;

            txtNombreCliente = CrearTextBox("Nombre completo del cliente");
            txtNombreCliente.Dock = DockStyle.Top;

            cmbTipoDocumento = new ComboBox
            {
                Dock = DockStyle.Top,
                Height = 44,
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(11, 18, 32),
                ForeColor = Texto,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbTipoDocumento.Items.Add("Boleta");
            cmbTipoDocumento.Items.Add("Factura");
            cmbTipoDocumento.SelectedIndex = 0;

            txtCantidad = CrearTextBox("Cantidad");
            txtCantidad.Dock = DockStyle.Top;

            Panel separador2 = CrearSeparador(16);

            Button btnAgregar = CrearBoton("Agregar al carrito", Morado);
            btnAgregar.Dock = DockStyle.Top;
            btnAgregar.Height = 56;
            btnAgregar.Click += BtnAgregar_Click;

            panel.Controls.Add(btnAgregar);
            panel.Controls.Add(separador2);
            panel.Controls.Add(txtCantidad);
            panel.Controls.Add(cmbTipoDocumento);
            panel.Controls.Add(txtNombreCliente);
            panel.Controls.Add(txtRutCliente);
            panel.Controls.Add(btnBuscarCliente);
            panel.Controls.Add(ayudaCliente);
            panel.Controls.Add(tituloCliente);
            panel.Controls.Add(separador1);
            panel.Controls.Add(lblProductoSeleccionado);
            panel.Controls.Add(btnBuscar);
            panel.Controls.Add(ayuda);
            panel.Controls.Add(titulo);
        }







        private void ConstruirPanelDetalleVenta(Panel panel)
        {
            panel.Controls.Clear();

            Label titulo = new Label
            {
                Text = "Carrito de venta",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50
            };

            FlowLayoutPanel acciones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 82,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 14, 0, 0)
            };

            Button btnRegistrar = CrearBotonAncho("Registrar venta", Verde);
            btnRegistrar.Width = 190;
            btnRegistrar.Click += BtnRegistrar_Click;

            Button btnLimpiar = CrearBotonAncho("Limpiar", Rojo);
            btnLimpiar.Width = 150;
            btnLimpiar.Click += (s, e) => LimpiarVenta();

            acciones.Controls.Add(btnRegistrar);
            acciones.Controls.Add(btnLimpiar);

            lblTotal = new Label
            {
                Text = "Total: $0",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                Dock = DockStyle.Bottom,
                Height = 78,
                TextAlign = ContentAlignment.MiddleRight
            };

            tablaVenta = new DataGridView
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

            tablaVenta.ColumnHeadersHeight = 42;
            tablaVenta.RowTemplate.Height = 38;

            tablaVenta.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 28, 43);
            tablaVenta.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            tablaVenta.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            tablaVenta.DefaultCellStyle.BackColor = Color.FromArgb(11, 18, 32);
            tablaVenta.DefaultCellStyle.ForeColor = Texto;
            tablaVenta.DefaultCellStyle.SelectionBackColor = Morado;
            tablaVenta.DefaultCellStyle.SelectionForeColor = Color.White;
            tablaVenta.DefaultCellStyle.Font = new Font("Segoe UI", 10F);

            tablaVenta.GridColor = Borde;

            tablaVenta.Columns.Clear();

            tablaVenta.Columns.Add("ProductoId", "ProductoId");
            tablaVenta.Columns["ProductoId"].Visible = false;

            tablaVenta.Columns.Add("Producto", "Producto");
            tablaVenta.Columns.Add("Precio", "Precio");
            tablaVenta.Columns.Add("Cantidad", "Cantidad");
            tablaVenta.Columns.Add("Subtotal", "Subtotal");

            tablaVenta.Columns["Producto"].FillWeight = 50;
            tablaVenta.Columns["Precio"].FillWeight = 18;
            tablaVenta.Columns["Cantidad"].FillWeight = 14;
            tablaVenta.Columns["Subtotal"].FillWeight = 18;

            panel.Controls.Add(tablaVenta);
            panel.Controls.Add(lblTotal);
            panel.Controls.Add(acciones);
            panel.Controls.Add(titulo);
        }








        private async void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var productos = await productoService.ObtenerProductosAsync();
                BuscarProductoForm modal = new BuscarProductoForm(productos);

                if (modal.ShowDialog() == DialogResult.OK)
                {
                    productoActual = modal.ProductoSeleccionado;

                    if (productoActual != null)
                    {
                        lblProductoSeleccionado.Text =
                            $"Producto seleccionado:\n\n" +
                            $"{productoActual.nombre}\n" +
                            $"Precio: ${productoActual.precio:N0}\n" +
                            $"Stock: {productoActual.stock}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
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




        private async void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                var clientes = await clienteService.ObtenerClientesAsync();

                BuscarClienteForm modal = new BuscarClienteForm(clientes);

                if (modal.ShowDialog() == DialogResult.OK)
                {
                    clienteActual = modal.ClienteSeleccionado;

                    if (clienteActual != null)
                    {
                        txtRutCliente.Text = clienteActual.rut;
                        txtRutCliente.ForeColor = Texto;

                        txtNombreCliente.Text =
                            $"{clienteActual.nombre} {clienteActual.apellido}".Trim();

                        txtNombreCliente.ForeColor = Texto;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
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

                var detallesApi = new List<object>();
                var detallesBoleta = new List<BoletaDetalle>();

                int total = 0;

                foreach (DataGridViewRow row in tablaVenta.Rows)
                {
                    if (row.IsNewRow) continue;

                    int productoId = Convert.ToInt32(row.Cells["ProductoId"].Value);
                    string producto = row.Cells["Producto"].Value.ToString();

                    int precio = Convert.ToInt32(
                        row.Cells["Precio"].Value.ToString().Replace(".", "").Replace(",", "")
                    );

                    int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);

                    int subtotal = Convert.ToInt32(
                        row.Cells["Subtotal"].Value.ToString().Replace(".", "").Replace(",", "")
                    );

                    total += subtotal;

                    detallesApi.Add(new
                    {
                        producto_id = productoId,
                        cantidad = cantidad
                    });

                    detallesBoleta.Add(new BoletaDetalle
                    {
                        Producto = producto,
                        Cantidad = cantidad,
                        Precio = precio,
                        Subtotal = subtotal
                    });
                }

                var ventaData = new
                {
                    tipo_documento = tipoDocumento.ToUpper(),
                    cliente = new
                    {
                        numero_documento = rut,
                        nombre_completo = nombre
                    },
                    detalles = detallesApi
                };

                await ventaService.CrearVentaAsync(ventaData);

                Boleta boleta = new Boleta
                {
                    Numero = DateTime.Now.Millisecond,
                    Cliente = nombre,
                    Rut = rut,
                    Fecha = DateTime.Now,
                    Total = total,
                    Detalles = detallesBoleta
                };

                BoletaForm boletaForm = new BoletaForm(boleta);
                boletaForm.ShowDialog();

                LimpiarVenta();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar venta: " + ex.Message);
            }
        }




        private void LimpiarVenta()
        {
            tablaVenta.Rows.Clear();

            lblTotal.Text = "Total: $0";

            txtRutCliente.Text = "RUT cliente";
            txtRutCliente.ForeColor = TextoSuave;

            txtNombreCliente.Text = "Nombre completo del cliente";
            txtNombreCliente.ForeColor = TextoSuave;

            txtCantidad.Text = "Cantidad";
            txtCantidad.ForeColor = TextoSuave;

            lblProductoSeleccionado.Text =
                "Sin producto seleccionado.";

            productoActual = null;
            clienteActual = null;
        }
    }
}