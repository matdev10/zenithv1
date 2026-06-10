using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace zenith_v1.Forms
{
    public class MainLayout : Form
    {
        private Panel sidebar;
        private Panel header;
        private Panel contentPanel;

        private readonly Color Fondo = Color.FromArgb(5, 7, 13);
        private readonly Color Panel = Color.FromArgb(11, 18, 32);
        private readonly Color Card = Color.FromArgb(17, 24, 39);
        private readonly Color Borde = Color.FromArgb(31, 41, 55);
        private readonly Color Texto = Color.FromArgb(248, 250, 252);
        private readonly Color TextoSuave = Color.FromArgb(148, 163, 184);
        private readonly Color Celeste = Color.FromArgb(15, 207, 217);
        private readonly Color Morado = Color.FromArgb(124, 58, 237);
        private readonly Color Verde = Color.FromArgb(34, 197, 94);
        private readonly Color Rojo = Color.FromArgb(239, 68, 68);

        public MainLayout()
        {
            ConfigurarVentana();
            CrearSidebar();
            CrearHeader();
            CrearContent();
            MostrarDashboard();
        }

        private void ConfigurarVentana()
        {
            Text = "ZenithPOS v1";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1280, 760);
            MinimumSize = new Size(1100, 650);
            BackColor = Fondo;
            Font = new Font("Segoe UI", 10F);
        }

        private void CrearSidebar()
        {
            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Panel,
                Padding = new Padding(22, 24, 22, 24)
            };

            Controls.Add(sidebar);

            Label logo = new Label
            {
                Text = "ZenithPOS",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 62,
                TextAlign = ContentAlignment.MiddleLeft
            };

            sidebar.Controls.Add(logo);

            FlowLayoutPanel menu = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 470,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 18, 0, 0)
            };



            sidebar.Controls.Add(menu);

            menu.Controls.Add(CrearMenuButton("🏠  Inicio", (s, e) => MostrarDashboard(), true));
            menu.Controls.Add(CrearMenuButton("🛒  Ventas", (s, e) => MostrarControl(new UserVentas())));
            menu.Controls.Add(CrearMenuButton("📦  Inventario", (s, e) => MostrarControl(new UserInventario())));
            menu.Controls.Add(CrearMenuButton("👥  Clientes", (s, e) => MostrarControl(new UserClientes())));
            menu.Controls.Add(CrearMenuButton("📄  Detalle ventas", (s, e) => MostrarControl(new UserDetalleVentas())));
            menu.Controls.Add(CrearMenuButton("📊  Informes", (s, e) => MostrarModuloPendiente("Informes")));
            menu.Controls.Add(CrearMenuButton("⚙️  Configuración", (s, e) => MostrarModuloPendiente("Configuración")));

            Button salir = CrearMenuButton("↩  Cerrar sesión", (s, e) => Close());
            salir.Dock = DockStyle.Bottom;
            salir.ForeColor = TextoSuave;
            sidebar.Controls.Add(salir);
        }

        private Button CrearMenuButton(string texto, EventHandler click, bool activo = false)
        {
            Button btn = new Button
            {
                Text = texto,
                Width = 205,
                Height = 48,
                Margin = new Padding(0, 0, 0, 12),
                FlatStyle = FlatStyle.Flat,
                BackColor = activo ? Color.FromArgb(36, 34, 74) : Panel,
                ForeColor = activo ? Texto : TextoSuave,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0),
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F)
            };

            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = activo ? Morado : Borde;
            btn.Click += click;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(31, 41, 65);
                btn.ForeColor = Texto;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = activo ? Color.FromArgb(36, 34, 74) : Panel;
                btn.ForeColor = activo ? Texto : TextoSuave;
            };

            return btn;
        }

        private void CrearHeader()
        {
            header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 86,
                BackColor = Fondo,
                Padding = new Padding(34, 16, 34, 0)
            };

            Controls.Add(header);

            Label titulo = new Label
            {
                Text = "Panel principal",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(34, 14)
            };

            Label subtitulo = new Label
            {
                Text = "Control de ventas, inventario, clientes y reportes.",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10.5F),
                AutoSize = true,
                Location = new Point(38, 56)
            };

            Label usuario = new Label
            {
                Text = "Usuario Administrador",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(760, 32)
            };

            header.Controls.Add(titulo);
            header.Controls.Add(subtitulo);
            header.Controls.Add(usuario);
        }

        private void CrearContent()
        {
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Fondo,
                Padding = new Padding(34, 20, 34, 34)
            };

            Controls.Add(contentPanel);
            contentPanel.BringToFront();
        }

        private void MostrarDashboard()
        {
            contentPanel.Controls.Clear();

            FlowLayoutPanel cards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 145,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            contentPanel.Controls.Add(cards);

            cards.Controls.Add(CrearCard("Ventas del día", "$0", "Resumen diario", Verde));
            cards.Controls.Add(CrearCard("Productos", "0", "Stock disponible", Celeste));
            cards.Controls.Add(CrearCard("Clientes", "0", "Registrados", Morado));
            cards.Controls.Add(CrearCard("Stock bajo", "0", "Requiere atención", Rojo));

            Panel acciones = CrearPanelRedondeado();
            acciones.Dock = DockStyle.Top;
            acciones.Height = 210;
            acciones.Margin = new Padding(0, 18, 0, 0);
            acciones.Padding = new Padding(22);
            contentPanel.Controls.Add(acciones);
            acciones.BringToFront();

            Label tituloAcciones = new Label
            {
                Text = "Accesos rápidos",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 42
            };

            acciones.Controls.Add(tituloAcciones);

            FlowLayoutPanel botones = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            acciones.Controls.Add(botones);

            botones.Controls.Add(CrearAccesoButton("Nueva venta", Morado));
            botones.Controls.Add(CrearAccesoButton("Buscar producto", Celeste));
            botones.Controls.Add(CrearAccesoButton("Clientes", Verde));
            botones.Controls.Add(CrearAccesoButton("Historial ventas", Rojo));

            Panel resumen = CrearPanelRedondeado();
            resumen.Dock = DockStyle.Fill;
            resumen.Margin = new Padding(0, 18, 0, 0);
            resumen.Padding = new Padding(24);
            contentPanel.Controls.Add(resumen);

            Label tituloResumen = new Label
            {
                Text = "Resumen del sistema",
                ForeColor = Texto,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 42
            };

            Label textoResumen = new Label
            {
                Text = "ZenithPOS v1 será una aplicación de escritorio moderna conectada a tu API.\n\n" +
                       "Próximo paso: crear módulos internos como UserVentas, UserInventario y UserClientes.",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 11F),
                Dock = DockStyle.Top,
                Height = 120
            };

            resumen.Controls.Add(textoResumen);
            resumen.Controls.Add(tituloResumen);
        }

        private Panel CrearCard(string titulo, string valor, string detalle, Color color)
        {
            Panel card = CrearPanelRedondeado();
            card.Width = 205;
            card.Height = 120;
            card.Margin = new Padding(0, 0, 18, 0);
            card.Padding = new Padding(18);

            Label lblTitulo = new Label
            {
                Text = titulo,
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 10F),
                Dock = DockStyle.Top,
                Height = 26
            };

            Label lblValor = new Label
            {
                Text = valor,
                ForeColor = Texto,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 46
            };

            Label lblDetalle = new Label
            {
                Text = detalle,
                ForeColor = color,
                Font = new Font("Segoe UI", 9.5F),
                Dock = DockStyle.Top,
                Height = 26
            };

            card.Controls.Add(lblDetalle);
            card.Controls.Add(lblValor);
            card.Controls.Add(lblTitulo);

            return card;
        }

        private Button CrearAccesoButton(string texto, Color color)
        {
            Button btn = new Button
            {
                Text = texto,
                Width = 170,
                Height = 58,
                Margin = new Padding(0, 12, 16, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(20, 28, 43),
                ForeColor = Texto,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Borde;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = color;
                btn.ForeColor = Color.White;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(20, 28, 43);
                btn.ForeColor = Texto;
            };

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

        private void MostrarControl(UserControl control)
        {
            contentPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(control);
        }

        
        private void MostrarModuloPendiente(string modulo)
        {
            contentPanel.Controls.Clear();

            Panel panel = CrearPanelRedondeado();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(30);
            contentPanel.Controls.Add(panel);

            Label titulo = new Label
            {
                Text = modulo,
                ForeColor = Texto,
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 60
            };

            Label descripcion = new Label
            {
                Text = "Este módulo será construido en la siguiente etapa de ZenithPOS v1.",
                ForeColor = TextoSuave,
                Font = new Font("Segoe UI", 12F),
                Dock = DockStyle.Top,
                Height = 40
            };

            panel.Controls.Add(descripcion);
            panel.Controls.Add(titulo);
        }
    }
}