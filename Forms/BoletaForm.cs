using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using zenith_v1.Models;

namespace zenith_v1.Forms
{
    public class BoletaForm : Form
    {
        private readonly Boleta boleta;

        public BoletaForm(Boleta boleta)
        {
            this.boleta = boleta;

            Text = $"Boleta #{boleta.Numero}";
            Width = 760;
            Height = 780;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 245, 245);

            ConstruirUI();
        }

        private void ConstruirUI()
        {
            Panel ticket = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(35),
                BackColor = Color.White
            };

            Controls.Add(ticket);

            Label titulo = new Label
            {
                Text = "ZEEZTON STORE",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 55,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label subtitulo = new Label
            {
                Text = "Comprobante de venta",
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label info = new Label
            {
                Text =
                    $"Boleta N° {boleta.Numero}\n" +
                    $"Fecha: {boleta.Fecha:dd/MM/yyyy HH:mm}\n\n" +
                    $"Cliente: {boleta.Cliente}\n" +
                    $"RUT: {boleta.Rut}",
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Top,
                Height = 120
            };

            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            dgv.Columns.Add("Producto", "Producto");
            dgv.Columns.Add("Cantidad", "Cant.");
            dgv.Columns.Add("Precio", "Precio");
            dgv.Columns.Add("Subtotal", "Subtotal");

            dgv.Columns["Producto"].FillWeight = 50;
            dgv.Columns["Cantidad"].FillWeight = 12;
            dgv.Columns["Precio"].FillWeight = 18;
            dgv.Columns["Subtotal"].FillWeight = 20;

            foreach (var item in boleta.Detalles)
            {
                dgv.Rows.Add(
                    item.Producto,
                    item.Cantidad,
                    "$" + item.Precio.ToString("N0"),
                    "$" + item.Subtotal.ToString("N0")
                );
            }

            Label total = new Label
            {
                Text = $"TOTAL: ${boleta.Total:N0}",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                Dock = DockStyle.Bottom,
                Height = 70,
                TextAlign = ContentAlignment.MiddleRight
            };

            FlowLayoutPanel acciones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 65,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };

            Button btnWhatsapp = CrearBoton("WhatsApp");
            btnWhatsapp.Click += BtnWhatsapp_Click;

            Button btnCorreo = CrearBoton("Correo");
            btnCorreo.Click += BtnCorreo_Click;

            Button btnGuardar = CrearBoton("Guardar");
            btnGuardar.Click += BtnGuardar_Click;

            acciones.Controls.Add(btnWhatsapp);
            acciones.Controls.Add(btnCorreo);
            acciones.Controls.Add(btnGuardar);

            ticket.Controls.Add(dgv);
            ticket.Controls.Add(total);
            ticket.Controls.Add(acciones);
            ticket.Controls.Add(info);
            ticket.Controls.Add(subtitulo);
            ticket.Controls.Add(titulo);
        }

        private Button CrearBoton(string texto)
        {
            return new Button
            {
                Text = texto,
                Width = 110,
                Height = 38,
                Margin = new Padding(8, 12, 0, 0),
                Cursor = Cursors.Hand
            };
        }

        private string GenerarTextoBoleta()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("ZEEZTON STORE");
            sb.AppendLine("Comprobante de venta");
            sb.AppendLine("------------------------------");
            sb.AppendLine($"Boleta N° {boleta.Numero}");
            sb.AppendLine($"Fecha: {boleta.Fecha:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Cliente: {boleta.Cliente}");
            sb.AppendLine($"RUT: {boleta.Rut}");
            sb.AppendLine("------------------------------");

            foreach (var item in boleta.Detalles)
            {
                sb.AppendLine($"{item.Producto}");
                sb.AppendLine($"Cant: {item.Cantidad} | Precio: ${item.Precio:N0} | Subtotal: ${item.Subtotal:N0}");
                sb.AppendLine();
            }

            sb.AppendLine("------------------------------");
            sb.AppendLine($"TOTAL: ${boleta.Total:N0}");
            sb.AppendLine("------------------------------");
            sb.AppendLine("Gracias por su compra.");

            return sb.ToString();
        }

        private void BtnWhatsapp_Click(object sender, EventArgs e)
        {
            string texto = Uri.EscapeDataString(GenerarTextoBoleta());
            Process.Start($"https://wa.me/?text={texto}");
        }

        private void BtnCorreo_Click(object sender, EventArgs e)
        {
            string asunto = Uri.EscapeDataString($"Boleta #{boleta.Numero} - Zeezton Store");
            string cuerpo = Uri.EscapeDataString(GenerarTextoBoleta());

            Process.Start($"mailto:?subject={asunto}&body={cuerpo}");
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Archivo de texto|*.txt",
                FileName = $"boleta_{boleta.Numero}.txt"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(dialog.FileName, GenerarTextoBoleta());
                MessageBox.Show("Boleta guardada correctamente.");
            }
        }
    }
}