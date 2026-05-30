namespace DeliveryRouteManager.Forms
{
    partial class FormHistorial
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListView listHistorial;
        private System.Windows.Forms.ColumnHeader colH_Num;
        private System.Windows.Forms.ColumnHeader colH_Cliente;
        private System.Windows.Forms.ColumnHeader colH_Ruta;
        private System.Windows.Forms.ColumnHeader colH_Distancia;
        private System.Windows.Forms.ColumnHeader colH_Fecha;

        private System.Windows.Forms.Button btnActualizar;
        private Label lblTotalHistorial;

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
            this.Text = "Historial de Entregas";
            this.Size = new System.Drawing.Size(900, 520);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MinimumSize = new System.Drawing.Size(750, 420);

            // ── BARRA SUPERIOR ───────────────────────────────────
            Panel panelTop = new Panel();
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Height = 48;
            panelTop.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            panelTop.Padding = new System.Windows.Forms.Padding(8, 8, 8, 4);

            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.Size = new System.Drawing.Size(110, 30);
            this.btnActualizar.Location = new System.Drawing.Point(8, 8);
            this.btnActualizar.BackColor = System.Drawing.Color.FromArgb(50, 130, 200);
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnActualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);

            panelTop.Controls.Add(this.btnActualizar);

            // ── LISTVIEW ─────────────────────────────────────────
            this.listHistorial = new System.Windows.Forms.ListView();
            this.listHistorial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listHistorial.View = System.Windows.Forms.View.Details;
            this.listHistorial.FullRowSelect = true;
            this.listHistorial.GridLines = true;
            this.listHistorial.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.colH_Num       = new System.Windows.Forms.ColumnHeader { Text = "#",          Width = 40  };
            this.colH_Cliente   = new System.Windows.Forms.ColumnHeader { Text = "Cliente",    Width = 150 };
            this.colH_Ruta      = new System.Windows.Forms.ColumnHeader { Text = "Ruta Recorrida", Width = 340 };
            this.colH_Distancia = new System.Windows.Forms.ColumnHeader { Text = "Dist. (km)", Width = 85  };
            this.colH_Fecha     = new System.Windows.Forms.ColumnHeader { Text = "Fecha Entrega", Width = 140 };

            this.listHistorial.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.colH_Num, this.colH_Cliente, this.colH_Ruta,
                this.colH_Distancia, this.colH_Fecha
            });

            // ── BARRA INFERIOR ───────────────────────────────────
            this.lblTotalHistorial = new Label();
            this.lblTotalHistorial.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotalHistorial.Height = 26;
            this.lblTotalHistorial.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.lblTotalHistorial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTotalHistorial.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblTotalHistorial.Text = "Total de entregas registradas: 0";

            // ── ARMAR FORM ───────────────────────────────────────
            this.Controls.Add(this.listHistorial);
            this.Controls.Add(this.lblTotalHistorial);
            this.Controls.Add(panelTop);
        }
    }
}
