using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeliveryRouteManager.Algorithms;
using DeliveryRouteManager.Database;
using DeliveryRouteManager.DataStructures;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.Forms
{
    public partial class FormGrafo: Form
    {
        private Grafo _grafo;
        private DbManager _db;
        private NodoPunto _nodoSeleccionado;
        private List<NodoPunto> _rutaResaltada;

        private const int RADIO_NODO = 22;
        private const int ITERACIONES_LAYOUT = 500;
        private const double FUERZA_REPULSION = 22000.0;
        private const double FUERZA_ATRACCION = 0.02;
        private const double AMORTIGUACION = 0.88;

        public FormGrafo(Grafo grafo, DbManager db)
        {
            InitializeComponent();
            _grafo = grafo;
            _db = db;
            _nodoSeleccionado = null;
            _rutaResaltada = new List<NodoPunto>();
            this.Shown += FormGrafo_Shown;
            CargarDatos();
        }

        private void CargarDatos()
        {
            ActualizarCombos();
            canvasGrafo.Invalidate();
        }

        private void ActualizarCombos()
        {
            cmbOrigen.Items.Clear();
            cmbDestino.Items.Clear();
            cmbOrigenRuta.Items.Clear();
            cmbDestinoRuta.Items.Clear();

            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
            {
                cmbOrigen.Items.Add(nodo);
                cmbDestino.Items.Add(nodo);
                cmbOrigenRuta.Items.Add(nodo);
                cmbDestinoRuta.Items.Add(nodo);
            }
        }

        private void FormGrafo_Shown(object sender, EventArgs e)
        {
            EjecutarLayout();
            GuardarPosicionesEnDb();
            canvasGrafo.Refresh();
        }

        private void GuardarPosicionesEnDb()
        {
            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
                _db.ActualizarPosicionPunto(nodo.Id, nodo.X, nodo.Y);
        }

        // ─── FORCE-DIRECTED LAYOUT ────────────────────────────────────────────────

        private void EjecutarLayout()
        {
            List<NodoPunto> nodos = _grafo.ObtenerNodos();
            if (nodos.Count == 0) return;

            int ancho = canvasGrafo.Width > 0 ? canvasGrafo.Width : 800;
            int alto = canvasGrafo.Height > 0 ? canvasGrafo.Height : 600;

            if (nodos.Count == 1)
            {
                nodos[0].X = ancho / 2.0;
                nodos[0].Y = alto / 2.0;
                return;
            }

            Dictionary<int, (double vx, double vy)> velocidades = new();
            foreach (NodoPunto n in nodos)
                velocidades[n.Id] = (0, 0);

            for (int iter = 0; iter < ITERACIONES_LAYOUT; iter++)
            {
                Dictionary<int, (double fx, double fy)> fuerzas = new();
                foreach (NodoPunto n in nodos)
                    fuerzas[n.Id] = (0, 0);

                // Repulsión nodo-nodo
                for (int i = 0; i < nodos.Count; i++)
                {
                    for (int j = i + 1; j < nodos.Count; j++)
                    {
                        double dx = nodos[j].X - nodos[i].X;
                        double dy = nodos[j].Y - nodos[i].Y;
                        double dist = Math.Max(Math.Sqrt(dx * dx + dy * dy), 1.0);

                        double fuerza = FUERZA_REPULSION / (dist * dist);
                        double fx = (dx / dist) * fuerza;
                        double fy = (dy / dist) * fuerza;

                        fuerzas[nodos[i].Id] = (fuerzas[nodos[i].Id].fx - fx, fuerzas[nodos[i].Id].fy - fy);
                        fuerzas[nodos[j].Id] = (fuerzas[nodos[j].Id].fx + fx, fuerzas[nodos[j].Id].fy + fy);
                    }
                }

                // Atracción por aristas proporcional al peso
                foreach (NodoPunto nodo in nodos)
                {
                    foreach (Arista arista in nodo.Conexiones)
                    {
                        NodoPunto destino = arista.Destino;
                        double longitudIdeal = arista.Peso * 30.0;

                        double dx = destino.X - nodo.X;
                        double dy = destino.Y - nodo.Y;
                        double dist = Math.Max(Math.Sqrt(dx * dx + dy * dy), 1.0);

                        double fuerza = FUERZA_ATRACCION * (dist - longitudIdeal);
                        double fx = (dx / dist) * fuerza;
                        double fy = (dy / dist) * fuerza;

                        fuerzas[nodo.Id] = (fuerzas[nodo.Id].fx + fx, fuerzas[nodo.Id].fy + fy);
                        fuerzas[destino.Id] = (fuerzas[destino.Id].fx - fx, fuerzas[destino.Id].fy - fy);
                    }
                }

                // Repulsión nodo-arista: empuja nodos que quedan sobre una arista ajena
                foreach (NodoPunto nodo in nodos)
                {
                    foreach (NodoPunto a in nodos)
                    {
                        foreach (Arista arista in a.Conexiones)
                        {
                            NodoPunto b = arista.Destino;

                            if (nodo.Id == a.Id || nodo.Id == b.Id) continue;

                            double ax = a.X, ay = a.Y;
                            double bx = b.X, by = b.Y;
                            double px = nodo.X, py = nodo.Y;

                            double abx = bx - ax, aby = by - ay;
                            double abLen = Math.Max(Math.Sqrt(abx * abx + aby * aby), 1.0);

                            double t = ((px - ax) * abx + (py - ay) * aby) / (abLen * abLen);
                            t = Math.Clamp(t, 0.0, 1.0);

                            double closestX = ax + t * abx;
                            double closestY = ay + t * aby;

                            double dx = px - closestX;
                            double dy = py - closestY;
                            double dist = Math.Max(Math.Sqrt(dx * dx + dy * dy), 1.0);

                            double umbral = RADIO_NODO * 4.0;
                            if (dist < umbral)
                            {
                                double fuerza = (FUERZA_REPULSION * 0.4) / (dist * dist);
                                fuerzas[nodo.Id] = (
                                    fuerzas[nodo.Id].fx + (dx / dist) * fuerza,
                                    fuerzas[nodo.Id].fy + (dy / dist) * fuerza
                                );
                            }
                        }
                    }
                }

                // Aplicar velocidades
                foreach (NodoPunto nodo in nodos)
                {
                    var (vx, vy) = velocidades[nodo.Id];
                    vx = (vx + fuerzas[nodo.Id].fx) * AMORTIGUACION;
                    vy = (vy + fuerzas[nodo.Id].fy) * AMORTIGUACION;
                    velocidades[nodo.Id] = (vx, vy);

                    nodo.X = Math.Clamp(nodo.X + vx, RADIO_NODO + 60, ancho - RADIO_NODO - 90);
                    nodo.Y = Math.Clamp(nodo.Y + vy, RADIO_NODO + 50, alto - RADIO_NODO - 55);
                }
            }
        }

        // ─── EVENTOS BOTONES ──────────────────────────────────────────────────────

        private void btnAgregarPunto_Click(object sender, EventArgs e)
        {
            string nombre = txtNombrePunto.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Ingresa un nombre para el punto.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ancho = canvasGrafo.Width > 0 ? canvasGrafo.Width : 800;
            int alto = canvasGrafo.Height > 0 ? canvasGrafo.Height : 600;

            Random rnd = new Random();
            double x = rnd.NextDouble() * (ancho - RADIO_NODO * 6) + RADIO_NODO * 3;
            double y = rnd.NextDouble() * (alto - RADIO_NODO * 6) + RADIO_NODO * 3;

            int id = _db.InsertarPunto(nombre, x, y);
            NodoPunto nuevo = new NodoPunto(id, nombre, x, y);
            _grafo.AgregarNodo(nuevo);

            txtNombrePunto.Clear();
            ActualizarCombos();
            EjecutarLayout();
            GuardarPosicionesEnDb();
            canvasGrafo.Refresh();
        }

        private void btnAgregarRuta_Click(object sender, EventArgs e)
        {
            if (cmbOrigenRuta.SelectedItem == null || cmbDestinoRuta.SelectedItem == null)
            {
                MessageBox.Show("Selecciona origen y destino.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtPeso.Text, out double peso) || peso <= 0)
            {
                MessageBox.Show("Ingresa un peso válido mayor a 0.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NodoPunto origen = (NodoPunto)cmbOrigenRuta.SelectedItem;
            NodoPunto destino = (NodoPunto)cmbDestinoRuta.SelectedItem;

            if (origen.Id == destino.Id)
            {
                MessageBox.Show("El origen y destino no pueden ser el mismo.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_grafo.ExisteConexion(origen.Id, destino.Id))
            {
                MessageBox.Show("Ya existe una ruta entre esos puntos.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _grafo.AgregarArista(origen.Id, destino.Id, peso);
            _db.InsertarRuta(origen.Id, destino.Id, peso);

            txtPeso.Clear();
            cmbOrigenRuta.SelectedIndex = -1;
            cmbDestinoRuta.SelectedIndex = -1;
            EjecutarLayout();
            GuardarPosicionesEnDb();
            canvasGrafo.Refresh();
        }

        private void btnCalcularRuta_Click(object sender, EventArgs e)
        {
            if (cmbOrigen.SelectedItem == null || cmbDestino.SelectedItem == null)
            {
                MessageBox.Show("Selecciona origen y destino para calcular.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NodoPunto origen = (NodoPunto)cmbOrigen.SelectedItem;
            NodoPunto destino = (NodoPunto)cmbDestino.SelectedItem;

            if (origen.Id == destino.Id)
            {
                MessageBox.Show("Origen y destino son el mismo punto.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var (ruta, distancia) = Dijkstra.RutaMasCorta(_grafo.ObtenerNodos(), origen, destino);

            if (ruta.Count == 0)
            {
                MessageBox.Show("No existe ruta disponible entre esos puntos.", "Sin ruta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _rutaResaltada = ruta;
            canvasGrafo.Invalidate();

            lblResultadoRuta.Text = $"Ruta: {string.Join(" → ", ruta)}   |   Distancia total: {distancia:F2} km";
        }

        // ─── DIBUJADO ─────────────────────────────────────────────────────────────

        private void canvasGrafo_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            List<NodoPunto> nodos = _grafo.ObtenerNodos();
            Font fuentePeso = new Font("Segoe UI", 7.5F);
            Font fuenteNombre = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            foreach (NodoPunto nodo in nodos)
            {
                foreach (Arista arista in nodo.Conexiones)
                {
                    if (nodo.Id < arista.Destino.Id)
                    {
                        bool enRuta = EsAristaEnRuta(nodo, arista.Destino);

                        using Pen pen = enRuta
                            ? new Pen(Color.DodgerBlue, 3)
                            : new Pen(Color.Silver, 1.5f);

                        g.DrawLine(pen,
                            (float)nodo.X, (float)nodo.Y,
                            (float)arista.Destino.X, (float)arista.Destino.Y);

                        // Offset perpendicular al eje de la arista para no tapar el trazo
                        double edgeDx = arista.Destino.X - nodo.X;
                        double edgeDy = arista.Destino.Y - nodo.Y;
                        double edgeLen = Math.Max(Math.Sqrt(edgeDx * edgeDx + edgeDy * edgeDy), 1.0);
                        float midX = (float)((nodo.X + arista.Destino.X) / 2) + (float)(-edgeDy / edgeLen) * 12;
                        float midY = (float)((nodo.Y + arista.Destino.Y) / 2) + (float)(edgeDx / edgeLen) * 12;

                        string etiqueta = $"{arista.Peso:F1} km";
                        SizeF tamEtiqueta = g.MeasureString(etiqueta, fuentePeso);

                        RectangleF fondoEtiqueta = new RectangleF(
                            midX - tamEtiqueta.Width / 2 - 2,
                            midY - tamEtiqueta.Height / 2 - 1,
                            tamEtiqueta.Width + 4,
                            tamEtiqueta.Height + 2);

                        g.FillRectangle(Brushes.White, fondoEtiqueta);
                        g.DrawString(etiqueta, fuentePeso, Brushes.DimGray,
                            midX - tamEtiqueta.Width / 2,
                            midY - tamEtiqueta.Height / 2);
                    }
                }
            }

            foreach (NodoPunto nodo in nodos)
            {
                bool enRuta = _rutaResaltada.Contains(nodo);
                bool seleccionado = _nodoSeleccionado != null && _nodoSeleccionado.Id == nodo.Id;

                Color colorNodo = seleccionado ? Color.OrangeRed
                                : enRuta ? Color.DodgerBlue
                                : Color.FromArgb(60, 80, 100);

                using SolidBrush brush = new SolidBrush(colorNodo);

                g.FillEllipse(brush,
                    (float)nodo.X - RADIO_NODO,
                    (float)nodo.Y - RADIO_NODO,
                    RADIO_NODO * 2,
                    RADIO_NODO * 2);

                g.DrawEllipse(Pens.White,
                    (float)nodo.X - RADIO_NODO,
                    (float)nodo.Y - RADIO_NODO,
                    RADIO_NODO * 2,
                    RADIO_NODO * 2);

                SizeF tamNombre = g.MeasureString(nodo.Nombre, fuenteNombre);
                float nameX = (float)nodo.X - tamNombre.Width / 2;
                float nameY = (float)nodo.Y + RADIO_NODO + 4;

                using SolidBrush fondoNombre = new SolidBrush(Color.FromArgb(160, 25, 25, 25));
                g.FillRectangle(fondoNombre,
                    nameX - 3, nameY - 1,
                    tamNombre.Width + 6, tamNombre.Height + 2);

                g.DrawString(nodo.Nombre, fuenteNombre, Brushes.White, nameX, nameY);
            }

            fuentePeso.Dispose();
            fuenteNombre.Dispose();
        }

        // ─── HELPERS ──────────────────────────────────────────────────────────────

        private bool EsAristaEnRuta(NodoPunto a, NodoPunto b)
        {
            for (int i = 0; i < _rutaResaltada.Count - 1; i++)
            {
                if ((_rutaResaltada[i].Id == a.Id && _rutaResaltada[i + 1].Id == b.Id) ||
                    (_rutaResaltada[i].Id == b.Id && _rutaResaltada[i + 1].Id == a.Id))
                    return true;
            }
            return false;
        }

        private void canvasGrafo_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
            {
                double dist = Math.Sqrt(Math.Pow(e.X - nodo.X, 2) + Math.Pow(e.Y - nodo.Y, 2));
                if (dist <= RADIO_NODO)
                {
                    _nodoSeleccionado = nodo;
                    lblNodoInfo.Text = $"Nodo seleccionado: {nodo.Nombre} | Conexiones: {nodo.Conexiones.Count}";
                    canvasGrafo.Invalidate();
                    return;
                }
            }
            _nodoSeleccionado = null;
            lblNodoInfo.Text = "Haz clic sobre un nodo para ver su info";
            canvasGrafo.Invalidate();
        }
    }
}
