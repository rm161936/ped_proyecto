namespace DeliveryRouteManager.Forms
{
    partial class FormPedidos
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controles ─────────────────────────────────────────────────────────────
        private Panel pnlControles;
        private Panel pnlMain;
        private Panel pnlStatus;

        // Panel de controles - Nuevo Pedido
        private Label lblTituloNuevoPedido;
        private Label lblCliente;
        private TextBox txtCliente;
        private Label lblPunto;
        private ComboBox cmbPuntoEntrega;
        private Label lblPrioridad;
        private ComboBox cmbPrioridad;
        private Button btnRegistrarPedido;

        // Panel de controles - Gestión
        private Label lblTituloGestion;
        private Button btnAtenderSiguiente;
        private Button btnConfirmarEntrega;
        private Button btnDeshacer;

        // Panel principal - Cola
        private Label lblTituloCola;
        private Label lblTotalCola;
        private DataGridView dgvCola;

        // Status
        private Label lblRutaAsignada;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Form ──────────────────────────────────────────────────────────────
            this.Text = "Gestión de Pedidos";
            this.Size = new Size(1000, 620);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            // ── pnlControles (derecha) ────────────────────────────────────────────
            pnlControles = new Panel
            {
                Dock = DockStyle.Right,
                Width = 260,
                BackColor = Color.FromArgb(24, 24, 24),
                Padding = new Padding(12)
            };

            // Título Nuevo Pedido
            lblTituloNuevoPedido = new Label
            {
                Text = "Nuevo Pedido",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 12),
                AutoSize = true
            };

            // Cliente
            lblCliente = new Label
            {
                Text = "Cliente:",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, 40),
                AutoSize = true
            };
            txtCliente = new TextBox
            {
                Location = new Point(12, 58),
                Width = 230,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9F)
            };

            // Punto de entrega
            lblPunto = new Label
            {
                Text = "Punto de entrega:",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, 88),
                AutoSize = true
            };
            cmbPuntoEntrega = new ComboBox
            {
                Location = new Point(12, 106),
                Width = 230,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F)
            };

            // Prioridad
            lblPrioridad = new Label
            {
                Text = "Prioridad:",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, 136),
                AutoSize = true
            };
            cmbPrioridad = new ComboBox
            {
                Location = new Point(12, 154),
                Width = 230,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F)
            };

            // Botón registrar
            btnRegistrarPedido = new Button
            {
                Text = "Registrar Pedido",
                Location = new Point(12, 186),
                Width = 230,
                Height = 32,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRegistrarPedido.FlatAppearance.BorderSize = 0;
            btnRegistrarPedido.Click += btnRegistrarPedido_Click;

            // Separador
            var sep = new Label
            {
                Location = new Point(12, 232),
                Width = 230,
                Height = 1,
                BackColor = Color.FromArgb(60, 60, 60)
            };

            // Título Gestión
            lblTituloGestion = new Label
            {
                Text = "Gestión de Pedidos",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 244),
                AutoSize = true
            };

            // Botón Atender Siguiente
            btnAtenderSiguiente = new Button
            {
                Text = "Atender Siguiente",
                Location = new Point(12, 270),
                Width = 230,
                Height = 32,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAtenderSiguiente.FlatAppearance.BorderSize = 0;
            btnAtenderSiguiente.Click += btnAtenderSiguiente_Click;

            // Botón Confirmar Entrega
            btnConfirmarEntrega = new Button
            {
                Text = "Confirmar Entrega",
                Location = new Point(12, 312),
                Width = 230,
                Height = 32,
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnConfirmarEntrega.FlatAppearance.BorderSize = 0;
            btnConfirmarEntrega.Click += btnConfirmarEntrega_Click;

            // Botón Deshacer
            btnDeshacer = new Button
            {
                Text = "Deshacer",
                Location = new Point(12, 354),
                Width = 230,
                Height = 32,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnDeshacer.FlatAppearance.BorderSize = 0;
            btnDeshacer.Click += btnDeshacer_Click;

            pnlControles.Controls.AddRange(new Control[]
            {
                lblTituloNuevoPedido, lblCliente, txtCliente,
                lblPunto, cmbPuntoEntrega, lblPrioridad, cmbPrioridad,
                btnRegistrarPedido, sep,
                lblTituloGestion, btnAtenderSiguiente,
                btnConfirmarEntrega, btnDeshacer
            });

            // ── pnlStatus (abajo) ─────────────────────────────────────────────────
            pnlStatus = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(10, 6, 10, 6)
            };

            lblRutaAsignada = new Label
            {
                Text = "Sin pedido en proceso actualmente.",
                Dock = DockStyle.Fill,
                ForeColor = Color.LightCyan,
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlStatus.Controls.Add(lblRutaAsignada);

            // ── pnlMain (centro) ──────────────────────────────────────────────────
            pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(12)
            };

            lblTituloCola = new Label
            {
                Text = "Cola de Pedidos Pendientes",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 10),
                AutoSize = true
            };

            lblTotalCola = new Label
            {
                Text = "Pedidos en cola: 0",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Silver,
                Location = new Point(12, 34),
                AutoSize = true
            };

            // DataGridView
            dgvCola = new DataGridView
            {
                Location = new Point(12, 58),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(pnlMain.Width - 24, pnlMain.Height - 70),
                BackgroundColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9F),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 30
            };
            dgvCola.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dgvCola.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCola.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvCola.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvCola.DefaultCellStyle.ForeColor = Color.White;
            dgvCola.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dgvCola.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCola.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(48, 48, 48);

            dgvCola.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", FillWeight = 40 });
            dgvCola.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCliente", HeaderText = "Cliente", FillWeight = 160 });
            dgvCola.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPunto", HeaderText = "Punto Entrega", FillWeight = 160 });
            dgvCola.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrioridad", HeaderText = "Prioridad", FillWeight = 80 });
            dgvCola.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha Registro", FillWeight = 120 });

            pnlMain.Controls.AddRange(new Control[] { lblTituloCola, lblTotalCola, dgvCola });

            // ── Agregar al Form ───────────────────────────────────────────────────
            this.Controls.Add(pnlMain);
            this.Controls.Add(pnlStatus);
            this.Controls.Add(pnlControles);
        }
    }
}