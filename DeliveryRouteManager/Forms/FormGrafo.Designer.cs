namespace DeliveryRouteManager.Forms
{
    partial class FormGrafo
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelCanvas;
        private Panel panelControles;
        private System.Windows.Forms.PictureBox canvasGrafo;

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

            // ── PANEL CONTROLES ─────────────────────────────────
            this.panelControles = new Panel();
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControles.Width = 280;
            this.panelControles.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.panelControles.Padding = new System.Windows.Forms.Padding(10);

            // ── AGREGAR PUNTO ───────────────────────────────────
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

            // ── AGREGAR RUTA ────────────────────────────────────
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

            // ── CALCULAR RUTA ───────────────────────────────────
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

            // ── CANVAS ──────────────────────────────────────────
            this.canvasGrafo = new System.Windows.Forms.PictureBox();
            this.canvasGrafo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasGrafo.BackColor = System.Drawing.Color.White;
            this.canvasGrafo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvasGrafo.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasGrafo_Paint);
            this.canvasGrafo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.canvasGrafo_MouseClick);

            // ── LABELS DE ESTADO ────────────────────────────────
            this.lblResultadoRuta = new Label();
            this.lblResultadoRuta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblResultadoRuta.Height = 24;
            this.lblResultadoRuta.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.lblResultadoRuta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblResultadoRuta.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblResultadoRuta.Text = "Selecciona origen y destino para calcular la ruta más corta";

            this.lblNodoInfo = new Label();
            this.lblNodoInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNodoInfo.Height = 22;
            this.lblNodoInfo.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.lblNodoInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblNodoInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblNodoInfo.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblNodoInfo.Text = "Haz clic sobre un nodo para ver su info";

            this.Controls.Add(this.canvasGrafo);
            this.Controls.Add(this.lblResultadoRuta);
            this.Controls.Add(this.lblNodoInfo);
            this.Controls.Add(this.panelControles);
        }
    }
}