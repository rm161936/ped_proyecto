namespace DeliveryRouteManager.Forms
{
    partial class FormGrafo
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelCanvas;
        private Panel panelControles;
        private System.Windows.Forms.PictureBox canvasGrafo;

        // Agregar Punto
        private Label lblTituloAgregarPunto;
        private Label lblNombrePunto;
        private System.Windows.Forms.TextBox txtNombrePunto;
        private System.Windows.Forms.Button btnAgregarPunto;

        // Agregar Ruta
        private Label lblTituloAgregarRuta;
        private Label lblOrigenRuta;
        private Label lblDestinoRuta;
        private Label lblPeso;
        private System.Windows.Forms.ComboBox cmbOrigenRuta;
        private System.Windows.Forms.ComboBox cmbDestinoRuta;
        private System.Windows.Forms.TextBox txtPeso;
        private System.Windows.Forms.Button btnAgregarRuta;

        // Calcular Ruta
        private Label lblTituloCalcular;
        private Label lblOrigen;
        private Label lblDestino;
        private System.Windows.Forms.ComboBox cmbOrigen;
        private System.Windows.Forms.ComboBox cmbDestino;
        private System.Windows.Forms.Button btnCalcularRuta;
        private Label lblResultadoRuta;
        private Label lblNodoInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Rutas y Puntos";
            this.Size = new System.Drawing.Size(1100, 640);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ForeColor = System.Drawing.Color.White;

            // ── PANEL CONTROLES (derecha) ─────────────────────────────────────────
            this.panelControles = new Panel();
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControles.Width = 280;
            this.panelControles.BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            this.panelControles.Padding = new System.Windows.Forms.Padding(12);

            // ── SECCIÓN: NUEVO PUNTO ──────────────────────────────────────────────
            lblTituloAgregarPunto = new Label
            {
                Text = "Nuevo Punto de Entrega",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(12, 12),
                AutoSize = true
            };

            var sep1 = new Label
            {
                Location = new System.Drawing.Point(12, 32),
                Width = 250,
                Height = 1,
                BackColor = System.Drawing.Color.FromArgb(60, 60, 60)
            };

            this.lblNombrePunto = new Label
            {
                Text = "Nombre:",
                ForeColor = System.Drawing.Color.Silver,
                Font = new System.Drawing.Font("Segoe UI", 8.5F),
                Location = new System.Drawing.Point(12, 42),
                AutoSize = true
            };

            this.txtNombrePunto = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(12, 60),
                Size = new System.Drawing.Size(250, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 45),
                ForeColor = System.Drawing.Color.White,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };

            this.btnAgregarPunto = new System.Windows.Forms.Button
            {
                Text = "Agregar Punto",
                Location = new System.Drawing.Point(12, 92),
                Size = new System.Drawing.Size(250, 32),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            this.btnAgregarPunto.FlatAppearance.BorderSize = 0;
            this.btnAgregarPunto.Click += new System.EventHandler(this.btnAgregarPunto_Click);

            // ── SECCIÓN: NUEVA RUTA ───────────────────────────────────────────────
            var sep2 = new Label
            {
                Location = new System.Drawing.Point(12, 140),
                Width = 250,
                Height = 1,
                BackColor = System.Drawing.Color.FromArgb(60, 60, 60)
            };

            lblTituloAgregarRuta = new Label
            {
                Text = "Nueva Ruta entre Puntos",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(12, 150),
                AutoSize = true
            };

            this.lblOrigenRuta = new Label
            {
                Text = "Origen:",
                ForeColor = System.Drawing.Color.Silver,
                Font = new System.Drawing.Font("Segoe UI", 8.5F),
                Location = new System.Drawing.Point(12, 175),
                AutoSize = true
            };

            this.cmbOrigenRuta = new System.Windows.Forms.ComboBox
            {
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,
                Location = new System.Drawing.Point(12, 193),
                Size = new System.Drawing.Size(250, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 45),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };

            this.lblDestinoRuta = new Label
            {
                Text = "Destino:",
                ForeColor = System.Drawing.Color.Silver,
                Font = new System.Drawing.Font("Segoe UI", 8.5F),
                Location = new System.Drawing.Point(12, 222),
                AutoSize = true
            };

            this.cmbDestinoRuta = new System.Windows.Forms.ComboBox
            {
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,
                Location = new System.Drawing.Point(12, 240),
                Size = new System.Drawing.Size(250, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 45),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };

            this.lblPeso = new Label
            {
                Text = "Distancia (km):",
                ForeColor = System.Drawing.Color.Silver,
                Font = new System.Drawing.Font("Segoe UI", 8.5F),
                Location = new System.Drawing.Point(12, 270),
                AutoSize = true
            };

            this.txtPeso = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(12, 288),
                Size = new System.Drawing.Size(250, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 45),
                ForeColor = System.Drawing.Color.White,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };

            this.btnAgregarRuta = new System.Windows.Forms.Button
            {
                Text = "Agregar Ruta",
                Location = new System.Drawing.Point(12, 320),
                Size = new System.Drawing.Size(250, 32),
                BackColor = System.Drawing.Color.FromArgb(40, 167, 69),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            this.btnAgregarRuta.FlatAppearance.BorderSize = 0;
            this.btnAgregarRuta.Click += new System.EventHandler(this.btnAgregarRuta_Click);

            // ── SECCIÓN: CALCULAR RUTA ────────────────────────────────────────────
            var sep3 = new Label
            {
                Location = new System.Drawing.Point(12, 368),
                Width = 250,
                Height = 1,
                BackColor = System.Drawing.Color.FromArgb(60, 60, 60)
            };

            lblTituloCalcular = new Label
            {
                Text = "Calcular Ruta más Corta (Dijkstra)",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(12, 378),
                AutoSize = true
            };

            this.lblOrigen = new Label
            {
                Text = "Origen:",
                ForeColor = System.Drawing.Color.Silver,
                Font = new System.Drawing.Font("Segoe UI", 8.5F),
                Location = new System.Drawing.Point(12, 403),
                AutoSize = true
            };

            this.cmbOrigen = new System.Windows.Forms.ComboBox
            {
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,
                Location = new System.Drawing.Point(12, 421),
                Size = new System.Drawing.Size(250, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 45),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };

            this.lblDestino = new Label
            {
                Text = "Destino:",
                ForeColor = System.Drawing.Color.Silver,
                Font = new System.Drawing.Font("Segoe UI", 8.5F),
                Location = new System.Drawing.Point(12, 450),
                AutoSize = true
            };

            this.cmbDestino = new System.Windows.Forms.ComboBox
            {
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,
                Location = new System.Drawing.Point(12, 468),
                Size = new System.Drawing.Size(250, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 45),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };

            this.btnCalcularRuta = new System.Windows.Forms.Button
            {
                Text = "Calcular Ruta",
                Location = new System.Drawing.Point(12, 500),
                Size = new System.Drawing.Size(250, 32),
                BackColor = System.Drawing.Color.FromArgb(23, 162, 184),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            this.btnCalcularRuta.FlatAppearance.BorderSize = 0;
            this.btnCalcularRuta.Click += new System.EventHandler(this.btnCalcularRuta_Click);

            // Agregar controles al panel
            this.panelControles.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTituloAgregarPunto, sep1,
                this.lblNombrePunto, this.txtNombrePunto, this.btnAgregarPunto,
                sep2, lblTituloAgregarRuta,
                this.lblOrigenRuta, this.cmbOrigenRuta,
                this.lblDestinoRuta, this.cmbDestinoRuta,
                this.lblPeso, this.txtPeso, this.btnAgregarRuta,
                sep3, lblTituloCalcular,
                this.lblOrigen, this.cmbOrigen,
                this.lblDestino, this.cmbDestino,
                this.btnCalcularRuta
            });

            // ── CANVAS ────────────────────────────────────────────────────────────
            this.canvasGrafo = new System.Windows.Forms.PictureBox
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.FromArgb(38, 38, 38),
                BorderStyle = System.Windows.Forms.BorderStyle.None
            };
            this.canvasGrafo.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasGrafo_Paint);
            this.canvasGrafo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.canvasGrafo_MouseClick);

            // ── LABELS DE ESTADO (abajo) ──────────────────────────────────────────
            this.lblResultadoRuta = new Label
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 28,
                BackColor = System.Drawing.Color.FromArgb(20, 20, 20),
                ForeColor = System.Drawing.Color.LightCyan,
                Font = new System.Drawing.Font("Segoe UI", 8.5F),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new System.Windows.Forms.Padding(10, 0, 0, 0),
                Text = "Selecciona origen y destino para calcular la ruta más corta"
            };

            this.lblNodoInfo = new Label
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 24,
                BackColor = System.Drawing.Color.FromArgb(24, 24, 24),
                ForeColor = System.Drawing.Color.Silver,
                Font = new System.Drawing.Font("Segoe UI", 8.5F),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new System.Windows.Forms.Padding(10, 0, 0, 0),
                Text = "Haz clic sobre un nodo para ver su info"
            };

            // ── Agregar al Form ───────────────────────────────────────────────────
            this.Controls.Add(this.canvasGrafo);
            this.Controls.Add(this.lblResultadoRuta);
            this.Controls.Add(this.lblNodoInfo);
            this.Controls.Add(this.panelControles);
        }
    }
}