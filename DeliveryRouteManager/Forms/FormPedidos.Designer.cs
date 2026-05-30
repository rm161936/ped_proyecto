namespace DeliveryRouteManager.Forms
{
    partial class FormPedidos
    {
        private System.ComponentModel.IContainer components = null;

        // ── Registrar Pedido ────────────────────────────────────────────────────
        private GroupBox grpRegistrar;
        private Label lblCliente;
        private System.Windows.Forms.TextBox txtCliente;
        private Label lblPunto;
        private System.Windows.Forms.ComboBox cmbPunto;
        private Label lblPrioridad;
        private System.Windows.Forms.ComboBox cmbPrioridad;
        private System.Windows.Forms.Button btnRegistrar;

        // ── Cola de Pedidos ─────────────────────────────────────────────────────
        private GroupBox grpCola;
        private System.Windows.Forms.ListView listCola;
        private System.Windows.Forms.ColumnHeader colCola_Num;
        private System.Windows.Forms.ColumnHeader colCola_Cliente;
        private System.Windows.Forms.ColumnHeader colCola_Destino;
        private System.Windows.Forms.ColumnHeader colCola_Prioridad;
        private System.Windows.Forms.ColumnHeader colCola_Fecha;
        private Label lblTotalCola;

        // ── Despachar ───────────────────────────────────────────────────────────
        private GroupBox grpDespachar;
        private Label lblOrigen;
        private System.Windows.Forms.ComboBox cmbOrigen;
        private System.Windows.Forms.Button btnDespachar;
        private Label lblResultadoDespacho;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── FORM ─────────────────────────────────────────────
            this.Text = "Gestión de Pedidos";
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MinimumSize = new System.Drawing.Size(800, 520);

            // ── PANEL LATERAL DERECHO ────────────────────────────
            Panel panelLateral = new Panel();
            panelLateral.Dock = System.Windows.Forms.DockStyle.Right;
            panelLateral.Width = 280;
            panelLateral.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            panelLateral.Padding = new System.Windows.Forms.Padding(10);

            // ── GRP: REGISTRAR PEDIDO ────────────────────────────
            this.grpRegistrar = new GroupBox();
            this.grpRegistrar.Text = "Nuevo Pedido";
            this.grpRegistrar.Location = new System.Drawing.Point(10, 10);
            this.grpRegistrar.Size = new System.Drawing.Size(255, 175);

            this.lblCliente = new Label();
            this.lblCliente.Text = "Cliente:";
            this.lblCliente.Location = new System.Drawing.Point(10, 28);
            this.lblCliente.Size = new System.Drawing.Size(60, 20);

            this.txtCliente = new System.Windows.Forms.TextBox();
            this.txtCliente.Location = new System.Drawing.Point(75, 25);
            this.txtCliente.Size = new System.Drawing.Size(168, 22);

            this.lblPunto = new Label();
            this.lblPunto.Text = "Destino:";
            this.lblPunto.Location = new System.Drawing.Point(10, 58);
            this.lblPunto.Size = new System.Drawing.Size(60, 20);

            this.cmbPunto = new System.Windows.Forms.ComboBox();
            this.cmbPunto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPunto.Location = new System.Drawing.Point(75, 55);
            this.cmbPunto.Size = new System.Drawing.Size(168, 22);

            this.lblPrioridad = new Label();
            this.lblPrioridad.Text = "Prioridad:";
            this.lblPrioridad.Location = new System.Drawing.Point(10, 90);
            this.lblPrioridad.Size = new System.Drawing.Size(65, 20);

            this.cmbPrioridad = new System.Windows.Forms.ComboBox();
            this.cmbPrioridad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrioridad.Location = new System.Drawing.Point(80, 87);
            this.cmbPrioridad.Size = new System.Drawing.Size(163, 22);

            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnRegistrar.Text = "Registrar Pedido";
            this.btnRegistrar.Location = new System.Drawing.Point(10, 120);
            this.btnRegistrar.Size = new System.Drawing.Size(233, 32);
            this.btnRegistrar.BackColor = System.Drawing.Color.FromArgb(50, 130, 200);
            this.btnRegistrar.ForeColor = System.Drawing.Color.White;
            this.btnRegistrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrar.FlatAppearance.BorderSize = 0;
            this.btnRegistrar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRegistrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);

            this.grpRegistrar.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblCliente, this.txtCliente,
                this.lblPunto,   this.cmbPunto,
                this.lblPrioridad, this.cmbPrioridad,
                this.btnRegistrar
            });

            // ── GRP: DESPACHAR ───────────────────────────────────
            this.grpDespachar = new GroupBox();
            this.grpDespachar.Text = "Despachar Siguiente Pedido";
            this.grpDespachar.Location = new System.Drawing.Point(10, 200);
            this.grpDespachar.Size = new System.Drawing.Size(255, 115);

            this.lblOrigen = new Label();
            this.lblOrigen.Text = "Origen:";
            this.lblOrigen.Location = new System.Drawing.Point(10, 28);
            this.lblOrigen.Size = new System.Drawing.Size(55, 20);

            this.cmbOrigen = new System.Windows.Forms.ComboBox();
            this.cmbOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrigen.Location = new System.Drawing.Point(70, 25);
            this.cmbOrigen.Size = new System.Drawing.Size(173, 22);

            this.btnDespachar = new System.Windows.Forms.Button();
            this.btnDespachar.Text = "Despachar Pedido";
            this.btnDespachar.Location = new System.Drawing.Point(10, 60);
            this.btnDespachar.Size = new System.Drawing.Size(233, 32);
            this.btnDespachar.BackColor = System.Drawing.Color.FromArgb(50, 170, 90);
            this.btnDespachar.ForeColor = System.Drawing.Color.White;
            this.btnDespachar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDespachar.FlatAppearance.BorderSize = 0;
            this.btnDespachar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDespachar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDespachar.Click += new System.EventHandler(this.btnDespachar_Click);

            this.grpDespachar.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblOrigen, this.cmbOrigen, this.btnDespachar
            });

            panelLateral.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.grpRegistrar,
                this.grpDespachar
            });

            // ── GRP: COLA DE PEDIDOS (panel izquierdo) ───────────
            this.grpCola = new GroupBox();
            this.grpCola.Text = "Cola de Pedidos (por prioridad)";
            this.grpCola.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCola.Padding = new System.Windows.Forms.Padding(10);

            this.listCola = new System.Windows.Forms.ListView();
            this.listCola.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listCola.View = System.Windows.Forms.View.Details;
            this.listCola.FullRowSelect = true;
            this.listCola.GridLines = true;
            this.listCola.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.colCola_Num       = new System.Windows.Forms.ColumnHeader { Text = "#",        Width = 35  };
            this.colCola_Cliente   = new System.Windows.Forms.ColumnHeader { Text = "Cliente",  Width = 140 };
            this.colCola_Destino   = new System.Windows.Forms.ColumnHeader { Text = "Destino",  Width = 120 };
            this.colCola_Prioridad = new System.Windows.Forms.ColumnHeader { Text = "Prioridad",Width = 80  };
            this.colCola_Fecha     = new System.Windows.Forms.ColumnHeader { Text = "Registrado",Width = 130};

            this.listCola.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.colCola_Num, this.colCola_Cliente, this.colCola_Destino,
                this.colCola_Prioridad, this.colCola_Fecha
            });

            this.lblTotalCola = new Label();
            this.lblTotalCola.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotalCola.Height = 24;
            this.lblTotalCola.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.lblTotalCola.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTotalCola.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblTotalCola.Text = "Pedidos en cola: 0";

            this.grpCola.Controls.Add(this.listCola);
            this.grpCola.Controls.Add(this.lblTotalCola);

            // ── BARRA DE RESULTADO ───────────────────────────────
            this.lblResultadoDespacho = new Label();
            this.lblResultadoDespacho.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblResultadoDespacho.Height = 28;
            this.lblResultadoDespacho.BackColor = System.Drawing.Color.FromArgb(240, 255, 240);
            this.lblResultadoDespacho.ForeColor = System.Drawing.Color.FromArgb(30, 110, 30);
            this.lblResultadoDespacho.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblResultadoDespacho.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblResultadoDespacho.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblResultadoDespacho.Text = "Listo para registrar y despachar pedidos.";

            // ── ARMAR FORM ───────────────────────────────────────
            this.Controls.Add(this.grpCola);
            this.Controls.Add(this.lblResultadoDespacho);
            this.Controls.Add(panelLateral);
        }
    }
}
