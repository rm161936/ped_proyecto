namespace DeliveryRouteManager.Forms
{
    partial class FormGrafo
    {
        private System.ComponentModel.IContainer components = null;

        // ── Área principal ───────────────────────────────────────────────────────
        private Panel panelLeft;
        private System.Windows.Forms.PictureBox canvasGrafo;

        // ── Panel controles (derecha) ────────────────────────────────────────────
        private Panel panelControles;

        private GroupBox grpAgregarPunto;
        private Label lblNombrePunto;
        private System.Windows.Forms.TextBox txtNombrePunto;
        private System.Windows.Forms.Button btnAgregarPunto;

        private GroupBox grpAgregarRuta;
        private Label lblOrigenRuta;
        private Label lblDestinoRuta;
        private Label lblPeso;
        private System.Windows.Forms.ComboBox cmbOrigenRuta;
        private System.Windows.Forms.ComboBox cmbDestinoRuta;
        private System.Windows.Forms.TextBox txtPeso;
        private System.Windows.Forms.Button btnAgregarRuta;

        private GroupBox grpCalcular;
        private Label lblOrigen;
        private Label lblDestino;
        private System.Windows.Forms.ComboBox cmbOrigen;
        private System.Windows.Forms.ComboBox cmbDestino;
        private System.Windows.Forms.Button btnCalcularRuta;

        // ── Barra resultado Dijkstra ─────────────────────────────────────────────
        private Label lblResultadoRuta;

        // ── Panel trazabilidad (inferior izquierdo) ──────────────────────────────
        private Panel panelTrazabilidad;
        private GroupBox grpLog;
        private Label lblNodoInfo;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button btnLimpiarLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Rutas y Puntos";
            this.Size = new System.Drawing.Size(1100, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MinimumSize = new System.Drawing.Size(900, 560);

            // ════════════════════════════════════════════════════════════════════
            // PANEL CONTROLES — derecha
            // ════════════════════════════════════════════════════════════════════
            this.panelControles = new Panel();
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControles.Width = 280;
            this.panelControles.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.panelControles.Padding = new System.Windows.Forms.Padding(10);

            // ── Agregar Punto ────────────────────────────────────────────────────
            this.grpAgregarPunto = new GroupBox();
            this.grpAgregarPunto.Text = "Nuevo Punto de Entrega";
            this.grpAgregarPunto.Location = new System.Drawing.Point(10, 10);
            this.grpAgregarPunto.Size = new System.Drawing.Size(255, 90);

            this.lblNombrePunto = new Label();
            this.lblNombrePunto.Text = "Nombre:";
            this.lblNombrePunto.Location = new System.Drawing.Point(10, 25);
            this.lblNombrePunto.Size = new System.Drawing.Size(60, 20);

            this.txtNombrePunto = new System.Windows.Forms.TextBox();
            this.txtNombrePunto.Location = new System.Drawing.Point(75, 22);
            this.txtNombrePunto.Size = new System.Drawing.Size(170, 22);

            this.btnAgregarPunto = new System.Windows.Forms.Button();
            this.btnAgregarPunto.Text = "Agregar Punto";
            this.btnAgregarPunto.Location = new System.Drawing.Point(10, 55);
            this.btnAgregarPunto.Size = new System.Drawing.Size(235, 28);
            this.btnAgregarPunto.Click += new System.EventHandler(this.btnAgregarPunto_Click);

            this.grpAgregarPunto.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblNombrePunto, this.txtNombrePunto, this.btnAgregarPunto
            });

            // ── Agregar Ruta ─────────────────────────────────────────────────────
            this.grpAgregarRuta = new GroupBox();
            this.grpAgregarRuta.Text = "Nueva Ruta entre Puntos";
            this.grpAgregarRuta.Location = new System.Drawing.Point(10, 115);
            this.grpAgregarRuta.Size = new System.Drawing.Size(255, 145);

            this.lblOrigenRuta = new Label();
            this.lblOrigenRuta.Text = "Origen:";
            this.lblOrigenRuta.Location = new System.Drawing.Point(10, 25);
            this.lblOrigenRuta.Size = new System.Drawing.Size(55, 20);

            this.cmbOrigenRuta = new System.Windows.Forms.ComboBox();
            this.cmbOrigenRuta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrigenRuta.Location = new System.Drawing.Point(70, 22);
            this.cmbOrigenRuta.Size = new System.Drawing.Size(175, 22);

            this.lblDestinoRuta = new Label();
            this.lblDestinoRuta.Text = "Destino:";
            this.lblDestinoRuta.Location = new System.Drawing.Point(10, 55);
            this.lblDestinoRuta.Size = new System.Drawing.Size(55, 20);

            this.cmbDestinoRuta = new System.Windows.Forms.ComboBox();
            this.cmbDestinoRuta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestinoRuta.Location = new System.Drawing.Point(70, 52);
            this.cmbDestinoRuta.Size = new System.Drawing.Size(175, 22);

            this.lblPeso = new Label();
            this.lblPeso.Text = "Distancia(km):";
            this.lblPeso.Location = new System.Drawing.Point(10, 85);
            this.lblPeso.Size = new System.Drawing.Size(65, 20);

            this.txtPeso = new System.Windows.Forms.TextBox();
            this.txtPeso.Location = new System.Drawing.Point(80, 82);
            this.txtPeso.Size = new System.Drawing.Size(165, 22);

            this.btnAgregarRuta = new System.Windows.Forms.Button();
            this.btnAgregarRuta.Text = "Agregar Ruta";
            this.btnAgregarRuta.Location = new System.Drawing.Point(10, 112);
            this.btnAgregarRuta.Size = new System.Drawing.Size(235, 28);
            this.btnAgregarRuta.Click += new System.EventHandler(this.btnAgregarRuta_Click);

            this.grpAgregarRuta.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblOrigenRuta, this.cmbOrigenRuta,
                this.lblDestinoRuta, this.cmbDestinoRuta,
                this.lblPeso, this.txtPeso, this.btnAgregarRuta
            });

            // ── Calcular Ruta ────────────────────────────────────────────────────
            this.grpCalcular = new GroupBox();
            this.grpCalcular.Text = "Calcular Ruta más Corta (Dijkstra)";
            this.grpCalcular.Location = new System.Drawing.Point(10, 275);
            this.grpCalcular.Size = new System.Drawing.Size(255, 120);

            this.lblOrigen = new Label();
            this.lblOrigen.Text = "Origen:";
            this.lblOrigen.Location = new System.Drawing.Point(10, 25);
            this.lblOrigen.Size = new System.Drawing.Size(55, 20);

            this.cmbOrigen = new System.Windows.Forms.ComboBox();
            this.cmbOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrigen.Location = new System.Drawing.Point(70, 22);
            this.cmbOrigen.Size = new System.Drawing.Size(175, 22);

            this.lblDestino = new Label();
            this.lblDestino.Text = "Destino:";
            this.lblDestino.Location = new System.Drawing.Point(10, 55);
            this.lblDestino.Size = new System.Drawing.Size(55, 20);

            this.cmbDestino = new System.Windows.Forms.ComboBox();
            this.cmbDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestino.Location = new System.Drawing.Point(70, 52);
            this.cmbDestino.Size = new System.Drawing.Size(175, 22);

            this.btnCalcularRuta = new System.Windows.Forms.Button();
            this.btnCalcularRuta.Text = "Calcular Ruta";
            this.btnCalcularRuta.Location = new System.Drawing.Point(10, 85);
            this.btnCalcularRuta.Size = new System.Drawing.Size(235, 28);
            this.btnCalcularRuta.Click += new System.EventHandler(this.btnCalcularRuta_Click);

            this.grpCalcular.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblOrigen, this.cmbOrigen,
                this.lblDestino, this.cmbDestino,
                this.btnCalcularRuta
            });

            this.panelControles.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.grpAgregarPunto, this.grpAgregarRuta, this.grpCalcular
            });

            // ════════════════════════════════════════════════════════════════════
            // PANEL TRAZABILIDAD — inferior izquierdo (dentro de panelLeft)
            // ════════════════════════════════════════════════════════════════════
            this.panelTrazabilidad = new Panel();
            this.panelTrazabilidad.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTrazabilidad.Height = 170;
            this.panelTrazabilidad.BackColor = System.Drawing.Color.FromArgb(248, 248, 250);
            this.panelTrazabilidad.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);

            this.grpLog = new GroupBox();
            this.grpLog.Text = "Registro de actividad";
            this.grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLog.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);

            // ── Cabecera del log: info de nodo + botón limpiar
            Panel panelLogHeader = new Panel();
            panelLogHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelLogHeader.Height = 26;
            panelLogHeader.Padding = new System.Windows.Forms.Padding(2, 2, 2, 0);

            this.lblNodoInfo = new Label();
            this.lblNodoInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNodoInfo.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblNodoInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular);
            this.lblNodoInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblNodoInfo.Text = "Haz clic sobre un nodo del grafo para ver su información";

            this.btnLimpiarLog = new System.Windows.Forms.Button();
            this.btnLimpiarLog.Text = "Limpiar";
            this.btnLimpiarLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLimpiarLog.Width = 68;
            this.btnLimpiarLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiarLog.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.btnLimpiarLog.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.btnLimpiarLog.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular);
            this.btnLimpiarLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiarLog.Click += new System.EventHandler(this.btnLimpiarLog_Click);

            // Orden: limpiar (Right) primero, luego lblNodoInfo (Fill) toma el resto
            panelLogHeader.Controls.Add(this.lblNodoInfo);
            panelLogHeader.Controls.Add(this.btnLimpiarLog);

            // ── RichTextBox: cuerpo del log
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.ReadOnly = true;
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(252, 252, 254);
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.WordWrap = false;

            // grpLog: panelLogHeader (Top) + rtbLog (Fill)
            // Docking: último agregado = z-order 0 = se ancla primero
            // → panelLogHeader (index 0 en Controls = z alto) ancla Top
            // → rtbLog (index 1 = z bajo = ancla después) Fill
            this.grpLog.Controls.Add(this.rtbLog);         // Fill  (index 0, z alto)
            this.grpLog.Controls.Add(panelLogHeader);       // Top   (index 1, z bajo → Top primero)

            this.panelTrazabilidad.Controls.Add(this.grpLog);

            // ════════════════════════════════════════════════════════════════════
            // BARRA RESULTADO DIJKSTRA
            // ════════════════════════════════════════════════════════════════════
            this.lblResultadoRuta = new Label();
            this.lblResultadoRuta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblResultadoRuta.Height = 24;
            this.lblResultadoRuta.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.lblResultadoRuta.ForeColor = System.Drawing.Color.FromArgb(30, 80, 160);
            this.lblResultadoRuta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblResultadoRuta.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblResultadoRuta.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblResultadoRuta.Text = "Selecciona origen y destino para calcular la ruta más corta";

            // ════════════════════════════════════════════════════════════════════
            // CANVAS GRAFO
            // ════════════════════════════════════════════════════════════════════
            this.canvasGrafo = new System.Windows.Forms.PictureBox();
            this.canvasGrafo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasGrafo.BackColor = System.Drawing.Color.White;
            this.canvasGrafo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvasGrafo.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasGrafo_Paint);
            this.canvasGrafo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.canvasGrafo_MouseClick);

            // ════════════════════════════════════════════════════════════════════
            // PANEL LEFT — envuelve canvas + resultado + trazabilidad
            // Orden de Controls.Add determina z-order:
            //   último agregado = z 0 = se ancla primero
            //   panelTrazabilidad (último) → Bottom (muy abajo)
            //   lblResultadoRuta  (penúlt) → Bottom (sobre trazabilidad)
            //   canvasGrafo       (primero)→ Fill   (espacio restante)
            // ════════════════════════════════════════════════════════════════════
            this.panelLeft = new Panel();
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;

            this.panelLeft.Controls.Add(this.canvasGrafo);         // z=2  Fill
            this.panelLeft.Controls.Add(this.lblResultadoRuta);    // z=1  Bottom
            this.panelLeft.Controls.Add(this.panelTrazabilidad);   // z=0  Bottom (primero en anclar)

            // ════════════════════════════════════════════════════════════════════
            // FORM — panelLeft (Fill) + panelControles (Right)
            //   panelControles último = z 0 = se ancla primero (reclama franja derecha)
            //   panelLeft (Fill) ocupa el resto
            // ════════════════════════════════════════════════════════════════════
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelControles);
        }
    }
}
