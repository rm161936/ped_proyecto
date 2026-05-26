namespace DeliveryRouteManager.Forms
{
    partial class FormHistorial
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controles ─────────────────────────────────────────────────────────────
        private Panel pnlFiltros;
        private Panel pnlMain;
        private Panel pnlDetalle;

        // Filtros
        private Label lblTituloFiltros;
        private Label lblFiltroCliente;
        private TextBox txtFiltroCliente;
        private CheckBox chkFiltroFecha;
        private DateTimePicker dtpFiltroFecha;
        private Button btnFiltrar;
        private Button btnLimpiarFiltros;
        private Button btnRefrescar;
        private Label lblTotalEntregas;

        // Tabla
        private DataGridView dgvHistorial;

        // Detalle
        private Label lblDetalle;

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
            this.Text = "Historial de Entregas";
            this.Size = new Size(1050, 620);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            // ── pnlFiltros (derecha) ──────────────────────────────────────────────
            pnlFiltros = new Panel
            {
                Dock = DockStyle.Right,
                Width = 240,
                BackColor = Color.FromArgb(24, 24, 24),
                Padding = new Padding(12)
            };

            lblTituloFiltros = new Label
            {
                Text = "Filtros",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 12),
                AutoSize = true
            };

            lblFiltroCliente = new Label
            {
                Text = "Cliente:",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, 40),
                AutoSize = true
            };

            txtFiltroCliente = new TextBox
            {
                Location = new Point(12, 58),
                Width = 210,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9F)
            };

            chkFiltroFecha = new CheckBox
            {
                Text = "Filtrar por fecha",
                Location = new Point(12, 92),
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = true
            };
            chkFiltroFecha.CheckedChanged += chkFiltroFecha_CheckedChanged;

            dtpFiltroFecha = new DateTimePicker
            {
                Location = new Point(12, 114),
                Width = 210,
                Format = DateTimePickerFormat.Short,
                Enabled = false,
                CalendarForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(45, 45, 45),
                Font = new Font("Segoe UI", 9F)
            };

            btnFiltrar = new Button
            {
                Text = "Filtrar",
                Location = new Point(12, 150),
                Width = 210,
                Height = 32,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.Click += btnFiltrar_Click;

            btnLimpiarFiltros = new Button
            {
                Text = "Limpiar filtros",
                Location = new Point(12, 192),
                Width = 210,
                Height = 32,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLimpiarFiltros.FlatAppearance.BorderSize = 0;
            btnLimpiarFiltros.Click += btnLimpiarFiltros_Click;

            var sep = new Label
            {
                Location = new Point(12, 238),
                Width = 210,
                Height = 1,
                BackColor = Color.FromArgb(60, 60, 60)
            };

            btnRefrescar = new Button
            {
                Text = "Refrescar",
                Location = new Point(12, 250),
                Width = 210,
                Height = 32,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.Click += btnRefrescar_Click;

            lblTotalEntregas = new Label
            {
                Text = "Total entregas: 0  |  Mostrando: 0",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8F),
                Location = new Point(12, 298),
                AutoSize = false,
                Width = 210,
                Height = 32
            };

            pnlFiltros.Controls.AddRange(new Control[]
            {
                lblTituloFiltros, lblFiltroCliente, txtFiltroCliente,
                chkFiltroFecha, dtpFiltroFecha,
                btnFiltrar, btnLimpiarFiltros, sep,
                btnRefrescar, lblTotalEntregas
            });

            // ── pnlDetalle (abajo) ────────────────────────────────────────────────
            pnlDetalle = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(10, 6, 10, 6)
            };

            lblDetalle = new Label
            {
                Text = "Selecciona una entrega para ver el detalle de la ruta.",
                Dock = DockStyle.Fill,
                ForeColor = Color.LightCyan,
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlDetalle.Controls.Add(lblDetalle);

            // ── pnlMain (centro) ──────────────────────────────────────────────────
            pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(12)
            };

            var lblTitulo = new Label
            {
                Text = "Historial de Entregas Completadas",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 10),
                AutoSize = true
            };

            var lblSubtitulo = new Label
            {
                Text = "Ordenado cronológicamente (más reciente primero)",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Silver,
                Location = new Point(12, 32),
                AutoSize = true
            };

            dgvHistorial = new DataGridView
            {
                Location = new Point(12, 56),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                       | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(pnlMain.Width - 24, pnlMain.Height - 68),
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
                ColumnHeadersHeightSizeMode =
                    DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 30
            };

            dgvHistorial.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dgvHistorial.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistorial.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvHistorial.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvHistorial.DefaultCellStyle.ForeColor = Color.White;
            dgvHistorial.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dgvHistorial.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvHistorial.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(48, 48, 48);

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colId", HeaderText = "ID", FillWeight = 40 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colCliente", HeaderText = "Cliente", FillWeight = 140 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colPunto", HeaderText = "Destino", FillWeight = 120 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colPrioridad", HeaderText = "Prioridad", FillWeight = 80 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colRuta", HeaderText = "Ruta Recorrida", FillWeight = 240 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colDistancia", HeaderText = "Distancia", FillWeight = 80 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colFecha", HeaderText = "Fecha Entrega", FillWeight = 120 });

            dgvHistorial.SelectionChanged += dgvHistorial_SelectionChanged;

            pnlMain.Controls.AddRange(new Control[]
                { lblTitulo, lblSubtitulo, dgvHistorial });

            // ── Agregar al Form ───────────────────────────────────────────────────
            this.Controls.Add(pnlMain);
            this.Controls.Add(pnlDetalle);
            this.Controls.Add(pnlFiltros);
        }
    }
}