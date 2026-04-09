namespace DeliveryRouteManager
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuArchivo;
        private ToolStripMenuItem menuItemSalir;
        private ToolStripMenuItem menuModulos;
        private ToolStripMenuItem menuItemGrafo;
        private ToolStripMenuItem menuItemPedidos;
        private ToolStripMenuItem menuItemHistorial;

        private Panel panelLateral;
        private Panel panelContenido;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnGrafo;
        private Button btnPedidos;
        private Button btnHistorial;
        private Button btnSalir;
        private Label lblVersion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── FORM ────────────────────────────────────────────
            this.Text = "Delivery Route Manager";
            this.Size = new System.Drawing.Size(1000, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.IsMdiContainer = true;
            this.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            // ── MENU STRIP ──────────────────────────────────────
            this.menuStrip = new MenuStrip();
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.menuStrip.ForeColor = System.Drawing.Color.White;

            this.menuArchivo = new ToolStripMenuItem("Archivo");
            this.menuArchivo.ForeColor = System.Drawing.Color.White;
            this.menuItemSalir = new ToolStripMenuItem("Salir");
            this.menuItemSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            this.menuArchivo.DropDownItems.Add(this.menuItemSalir);

            this.menuModulos = new ToolStripMenuItem("Módulos");
            this.menuModulos.ForeColor = System.Drawing.Color.White;
            this.menuItemGrafo = new ToolStripMenuItem("Gestión de Rutas y Puntos");
            this.menuItemGrafo.Click += new System.EventHandler(this.BtnGrafo_Click);
            this.menuItemPedidos = new ToolStripMenuItem("Gestión de Pedidos");
            this.menuItemPedidos.Click += new System.EventHandler(this.BtnPedidos_Click);
            this.menuItemHistorial = new ToolStripMenuItem("Historial de Entregas");
            this.menuItemHistorial.Click += new System.EventHandler(this.BtnHistorial_Click);
            this.menuModulos.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.menuItemGrafo,
                this.menuItemPedidos,
                this.menuItemHistorial
            });

            this.menuStrip.Items.AddRange(new ToolStripItem[]
            {
                this.menuArchivo,
                this.menuModulos
            });

            // ── PANEL LATERAL ───────────────────────────────────
            this.panelLateral = new Panel();
            this.panelLateral.Dock = DockStyle.Left;
            this.panelLateral.Width = 220;
            this.panelLateral.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);

            // ── TÍTULO ──────────────────────────────────────────
            this.lblTitulo = new Label();
            this.lblTitulo.Text = "DELIVERY";
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitulo.Size = new System.Drawing.Size(220, 40);
            this.lblTitulo.Location = new System.Drawing.Point(0, 30);

            this.lblSubtitulo = new Label();
            this.lblSubtitulo.Text = "Route Manager";
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSubtitulo.Size = new System.Drawing.Size(220, 24);
            this.lblSubtitulo.Location = new System.Drawing.Point(0, 68);

            // ── BOTONES LATERALES ───────────────────────────────
            this.btnGrafo = CrearBotonLateral("Rutas y Puntos", 140);
            this.btnGrafo.Click += new System.EventHandler(this.BtnGrafo_Click);

            this.btnPedidos = CrearBotonLateral("Pedidos", 200);
            this.btnPedidos.Click += new System.EventHandler(this.BtnPedidos_Click);

            this.btnHistorial = CrearBotonLateral("Historial", 260);
            this.btnHistorial.Click += new System.EventHandler(this.BtnHistorial_Click);

            this.btnSalir = CrearBotonLateral("Salir", 460);
            this.btnSalir.ForeColor = System.Drawing.Color.FromArgb(220, 80, 80);
            this.btnSalir.Click += new System.EventHandler(this.BtnSalir_Click);

            // ── VERSIÓN ─────────────────────────────────────────
            this.lblVersion = new Label();
            this.lblVersion.Text = "v1.0.0";
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblVersion.Size = new System.Drawing.Size(220, 20);
            this.lblVersion.Location = new System.Drawing.Point(0, 540);

            this.panelLateral.Controls.AddRange(new Control[]
            {
                this.lblTitulo,
                this.lblSubtitulo,
                this.btnGrafo,
                this.btnPedidos,
                this.btnHistorial,
                this.btnSalir,
                this.lblVersion
            });

            // ── PANEL CONTENIDO (área MDI) ──────────────────────
            this.panelContenido = new Panel();
            this.panelContenido.Dock = DockStyle.Fill;
            this.panelContenido.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            // ── AGREGAR AL FORM ─────────────────────────────────
            this.MainMenuStrip = this.menuStrip;
            this.Controls.Add(this.panelLateral);
            this.Controls.Add(this.menuStrip);
        }

        private Button CrearBotonLateral(string texto, int posicionY)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.ForeColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btn.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(55, 55, 55);
            btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(70, 70, 70);
            btn.Font = new System.Drawing.Font("Segoe UI", 10F);
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0);
            btn.Size = new System.Drawing.Size(220, 44);
            btn.Location = new System.Drawing.Point(0, posicionY);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

    }
}
