using DeliveryRouteManager.Algorithms;
using DeliveryRouteManager.Database;
using DeliveryRouteManager.DataStructures;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.Forms
{
    public partial class FormGrafo : Form
    {
        private Grafo    _grafo;
        private DbManager _db;
        private NodoPunto?       _nodoSeleccionado;
        private List<NodoPunto>  _rutaResaltada;

        // Dimensiones del canvas en el último frame — para escalar en resize
        private int _ultimoAncho;
        private int _ultimoAlto;

        // ── Geometría ────────────────────────────────────────────────────────────
        private const int    RADIO_NODO      = 22;
        private const double DIST_MIN_NODOS  = RADIO_NODO * 2 + 30;  // 74 px centro-a-centro

        // ── Parámetros layout force-directed ─────────────────────────────────────
        private const int    ITERACIONES_LAYOUT = 450;
        private const double FUERZA_REPULSION   = 18000.0;
        private const double FUERZA_ATRACCION   =  0.022;
        private const double AMORTIGUACION      =  0.78;

        // ── Colores del log de trazabilidad ──────────────────────────────────────
        private static readonly System.Drawing.Color ColorPunto    = System.Drawing.Color.FromArgb( 20, 130,  60);
        private static readonly System.Drawing.Color ColorRuta     = System.Drawing.Color.FromArgb( 25, 100, 200);
        private static readonly System.Drawing.Color ColorDijkstra = System.Drawing.Color.FromArgb(180,  90,   0);
        private static readonly System.Drawing.Color ColorNodo     = System.Drawing.Color.FromArgb( 90,  90,  90);
        private static readonly System.Drawing.Color ColorError    = System.Drawing.Color.FromArgb(200,  30,  30);
        private static readonly System.Drawing.Color ColorSistema  = System.Drawing.Color.FromArgb(120, 120, 140);

        // ═════════════════════════════════════════════════════════════════════════
        // CONSTRUCTOR
        // ═════════════════════════════════════════════════════════════════════════

        public FormGrafo(Grafo grafo, DbManager db)
        {
            InitializeComponent();
            _grafo            = grafo;
            _db               = db;
            _nodoSeleccionado = null;
            _rutaResaltada    = new List<NodoPunto>();

            // Escalar posiciones cuando el usuario redimensiona el canvas
            canvasGrafo.Resize += canvasGrafo_Resize;

            CargarDatos();
        }

        // ═════════════════════════════════════════════════════════════════════════
        // CARGA INICIAL
        // ═════════════════════════════════════════════════════════════════════════

        private void CargarDatos()
        {
            ActualizarCombos();
            EjecutarLayout();
            canvasGrafo.Invalidate();

            int nodos   = _grafo.ObtenerNodos().Count;
            int aristas = _grafo.ObtenerNodos().Sum(n => n.Conexiones.Count) / 2;
            AgregarEntradaLog(
                $"Módulo iniciado — {nodos} punto(s) y {aristas} ruta(s) cargados desde la base de datos.",
                ColorSistema);
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

        // ═════════════════════════════════════════════════════════════════════════
        // LAYOUT FORCE-DIRECTED + POST-PROCESO
        // ═════════════════════════════════════════════════════════════════════════

        private void EjecutarLayout()
        {
            List<NodoPunto> nodos = _grafo.ObtenerNodos();
            if (nodos.Count == 0) return;

            int ancho = canvasGrafo.Width  > 0 ? canvasGrafo.Width  : 800;
            int alto  = canvasGrafo.Height > 0 ? canvasGrafo.Height : 600;

            // Nodo único: centrar y salir
            if (nodos.Count == 1)
            {
                nodos[0].X = ancho / 2.0;
                nodos[0].Y = alto  / 2.0;
                _ultimoAncho = ancho;
                _ultimoAlto  = alto;
                return;
            }

            Dictionary<int, (double vx, double vy)> vel = new();
            foreach (NodoPunto n in nodos)
                vel[n.Id] = (0, 0);

            for (int iter = 0; iter < ITERACIONES_LAYOUT; iter++)
            {
                Dictionary<int, (double fx, double fy)> fuerza = new();
                foreach (NodoPunto n in nodos)
                    fuerza[n.Id] = (0, 0);

                // ── Repulsión nodo-nodo ──────────────────────────────────────
                for (int i = 0; i < nodos.Count; i++)
                {
                    for (int j = i + 1; j < nodos.Count; j++)
                    {
                        double dx   = nodos[j].X - nodos[i].X;
                        double dy   = nodos[j].Y - nodos[i].Y;
                        double dist = Math.Max(Math.Sqrt(dx * dx + dy * dy), 1.0);

                        // Fuerza extra cuando los nodos se superponen
                        double magnitud = FUERZA_REPULSION / (dist * dist);
                        if (dist < DIST_MIN_NODOS)
                            magnitud += FUERZA_REPULSION * 3.0 / (dist * dist);

                        double fx = (dx / dist) * magnitud;
                        double fy = (dy / dist) * magnitud;

                        fuerza[nodos[i].Id] = (fuerza[nodos[i].Id].fx - fx, fuerza[nodos[i].Id].fy - fy);
                        fuerza[nodos[j].Id] = (fuerza[nodos[j].Id].fx + fx, fuerza[nodos[j].Id].fy + fy);
                    }
                }

                // ── Atracción proporcional al peso de la arista ──────────────
                foreach (NodoPunto nodo in nodos)
                {
                    foreach (Arista arista in nodo.Conexiones)
                    {
                        NodoPunto dest        = arista.Destino;
                        double    longIdeal   = arista.Peso * 30.0;
                        double    dx          = dest.X - nodo.X;
                        double    dy          = dest.Y - nodo.Y;
                        double    dist        = Math.Max(Math.Sqrt(dx * dx + dy * dy), 1.0);
                        double    f           = FUERZA_ATRACCION * (dist - longIdeal);
                        double    fx          = (dx / dist) * f;
                        double    fy          = (dy / dist) * f;

                        fuerza[nodo.Id] = (fuerza[nodo.Id].fx + fx, fuerza[nodo.Id].fy + fy);
                        fuerza[dest.Id] = (fuerza[dest.Id].fx - fx, fuerza[dest.Id].fy - fy);
                    }
                }

                // ── Repulsión nodo-arista (evita que nodos queden sobre aristas) ──
                foreach (NodoPunto nodo in nodos)
                {
                    foreach (NodoPunto a in nodos)
                    {
                        foreach (Arista arista in a.Conexiones)
                        {
                            NodoPunto b = arista.Destino;
                            if (nodo.Id == a.Id || nodo.Id == b.Id) continue;

                            double abx   = b.X - a.X, aby = b.Y - a.Y;
                            double abLen = Math.Max(Math.Sqrt(abx * abx + aby * aby), 1.0);
                            double t     = Math.Clamp(((nodo.X - a.X) * abx + (nodo.Y - a.Y) * aby) / (abLen * abLen), 0.0, 1.0);

                            double cx   = a.X + t * abx, cy = a.Y + t * aby;
                            double dx   = nodo.X - cx,   dy  = nodo.Y - cy;
                            double dist = Math.Max(Math.Sqrt(dx * dx + dy * dy), 1.0);

                            if (dist < RADIO_NODO * 4.5)
                            {
                                double f = (FUERZA_REPULSION * 0.5) / (dist * dist);
                                fuerza[nodo.Id] = (
                                    fuerza[nodo.Id].fx + (dx / dist) * f,
                                    fuerza[nodo.Id].fy + (dy / dist) * f);
                            }
                        }
                    }
                }

                // ── Aplicar velocidades ──────────────────────────────────────
                foreach (NodoPunto nodo in nodos)
                {
                    var (vx, vy) = vel[nodo.Id];
                    vx = (vx + fuerza[nodo.Id].fx) * AMORTIGUACION;
                    vy = (vy + fuerza[nodo.Id].fy) * AMORTIGUACION;
                    vel[nodo.Id] = (vx, vy);

                    nodo.X = Math.Clamp(nodo.X + vx, RADIO_NODO + 20, ancho - RADIO_NODO - 20);
                    nodo.Y = Math.Clamp(nodo.Y + vy, RADIO_NODO + 30, alto  - RADIO_NODO - 30);
                }
            }

            // ── Post-proceso: eliminar solapamientos residuales ──────────────
            SepararNodosSuperpuestos(nodos, ancho, alto);

            _ultimoAncho = ancho;
            _ultimoAlto  = alto;
        }

        // ─── Restricción dura: separa nodos que aún se superponen ─────────────────

        private void SepararNodosSuperpuestos(List<NodoPunto> nodos, int ancho, int alto)
        {
            bool haySolapamiento = true;
            int  iteraciones     = 0;

            while (haySolapamiento && iteraciones < 200)
            {
                haySolapamiento = false;
                iteraciones++;

                for (int i = 0; i < nodos.Count; i++)
                {
                    for (int j = i + 1; j < nodos.Count; j++)
                    {
                        double dx   = nodos[j].X - nodos[i].X;
                        double dy   = nodos[j].Y - nodos[i].Y;
                        double dist = Math.Sqrt(dx * dx + dy * dy);

                        if (dist < DIST_MIN_NODOS)
                        {
                            haySolapamiento = true;

                            if (dist < 0.5)
                            {
                                // Posiciones idénticas: empujar en direcciones opuestas
                                dx = (i % 2 == 0) ?  1.0 : -1.0;
                                dy = (j % 2 == 0) ?  1.0 : -1.0;
                                dist = Math.Sqrt(2.0);
                            }

                            double empuje = (DIST_MIN_NODOS - dist) / 2.0 + 1.0;
                            double ex     = (dx / dist) * empuje;
                            double ey     = (dy / dist) * empuje;

                            nodos[i].X = Math.Clamp(nodos[i].X - ex, RADIO_NODO + 20, ancho - RADIO_NODO - 20);
                            nodos[i].Y = Math.Clamp(nodos[i].Y - ey, RADIO_NODO + 30, alto  - RADIO_NODO - 30);
                            nodos[j].X = Math.Clamp(nodos[j].X + ex, RADIO_NODO + 20, ancho - RADIO_NODO - 20);
                            nodos[j].Y = Math.Clamp(nodos[j].Y + ey, RADIO_NODO + 30, alto  - RADIO_NODO - 30);
                        }
                    }
                }
            }
        }

        // ─── Posición libre en el canvas para un nodo nuevo ───────────────────────
        //  Divide el canvas en una grilla y elige la celda más alejada de todos
        //  los nodos existentes.

        private (double x, double y) EncontrarPosicionLibre(int ancho, int alto)
        {
            List<NodoPunto> nodos = _grafo.ObtenerNodos();
            if (nodos.Count == 0)
                return (ancho / 2.0, alto / 2.0);

            int    margen      = RADIO_NODO + 35;
            int    cols        = 10;
            int    filas       = 7;
            double mejorX      = ancho / 2.0;
            double mejorY      = alto  / 2.0;
            double mejorDistMin = -1;

            for (int r = 0; r < filas; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    double x = margen + (ancho - 2 * margen) * c / (double)(cols - 1);
                    double y = margen + (alto  - 2 * margen) * r / (double)(filas - 1);

                    double distMin = nodos.Min(n =>
                        Math.Sqrt(Math.Pow(n.X - x, 2) + Math.Pow(n.Y - y, 2)));

                    if (distMin > mejorDistMin)
                    {
                        mejorDistMin = distMin;
                        mejorX = x;
                        mejorY = y;
                    }
                }
            }

            return (mejorX, mejorY);
        }

        // ═════════════════════════════════════════════════════════════════════════
        // EVENTOS — BOTONES DE CONTROL
        // ═════════════════════════════════════════════════════════════════════════

        private void btnAgregarPunto_Click(object sender, EventArgs e)
        {
            string nombre = txtNombrePunto.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Ingresa un nombre para el punto.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AgregarEntradaLog("✗  Intento de agregar punto sin nombre.", ColorError);
                return;
            }

            int ancho = canvasGrafo.Width  > 0 ? canvasGrafo.Width  : 800;
            int alto  = canvasGrafo.Height > 0 ? canvasGrafo.Height : 600;

            // Colocar el nuevo nodo en el área más libre del canvas
            var (x, y) = EncontrarPosicionLibre(ancho, alto);

            int       id    = _db.InsertarPunto(nombre, x, y);
            NodoPunto nuevo = new NodoPunto(id, nombre, x, y);
            _grafo.AgregarNodo(nuevo);

            txtNombrePunto.Clear();
            ActualizarCombos();
            EjecutarLayout();
            canvasGrafo.Invalidate();

            int totalNodos = _grafo.ObtenerNodos().Count;
            AgregarEntradaLog(
                $"✓  Punto agregado → \"{nombre}\"  (ID: {id})  |  Total en grafo: {totalNodos} nodo(s).",
                ColorPunto);
        }

        private void btnAgregarRuta_Click(object sender, EventArgs e)
        {
            if (cmbOrigenRuta.SelectedItem == null || cmbDestinoRuta.SelectedItem == null)
            {
                MessageBox.Show("Selecciona origen y destino.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AgregarEntradaLog("✗  Ruta no creada: falta seleccionar origen o destino.", ColorError);
                return;
            }

            if (!double.TryParse(txtPeso.Text, out double peso) || peso <= 0)
            {
                MessageBox.Show("Ingresa un peso válido mayor a 0.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AgregarEntradaLog("✗  Ruta no creada: distancia inválida.", ColorError);
                return;
            }

            NodoPunto origen  = (NodoPunto)cmbOrigenRuta.SelectedItem;
            NodoPunto destino = (NodoPunto)cmbDestinoRuta.SelectedItem;

            if (origen.Id == destino.Id)
            {
                MessageBox.Show("El origen y destino no pueden ser el mismo.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AgregarEntradaLog("✗  Ruta no creada: origen y destino son el mismo punto.", ColorError);
                return;
            }

            if (_grafo.ExisteConexion(origen.Id, destino.Id))
            {
                MessageBox.Show("Ya existe una ruta entre esos puntos.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AgregarEntradaLog(
                    $"✗  Ruta no creada: ya existe conexión entre \"{origen.Nombre}\" y \"{destino.Nombre}\".",
                    ColorError);
                return;
            }

            _grafo.AgregarArista(origen.Id, destino.Id, peso);
            _db.InsertarRuta(origen.Id, destino.Id, peso);

            txtPeso.Clear();
            cmbOrigenRuta.SelectedIndex  = -1;
            cmbDestinoRuta.SelectedIndex = -1;
            EjecutarLayout();
            canvasGrafo.Invalidate();

            int totalAristas = _grafo.ObtenerNodos().Sum(n => n.Conexiones.Count) / 2;
            AgregarEntradaLog(
                $"✓  Ruta agregada → \"{origen.Nombre}\" ↔ \"{destino.Nombre}\"  ({peso:F1} km)" +
                $"  |  Total rutas: {totalAristas}.",
                ColorRuta);
        }

        private void btnCalcularRuta_Click(object sender, EventArgs e)
        {
            if (cmbOrigen.SelectedItem == null || cmbDestino.SelectedItem == null)
            {
                MessageBox.Show("Selecciona origen y destino para calcular.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AgregarEntradaLog("✗  Cálculo cancelado: falta origen o destino.", ColorError);
                return;
            }

            NodoPunto origen  = (NodoPunto)cmbOrigen.SelectedItem;
            NodoPunto destino = (NodoPunto)cmbDestino.SelectedItem;

            if (origen.Id == destino.Id)
            {
                MessageBox.Show("Origen y destino son el mismo punto.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AgregarEntradaLog("✗  Cálculo cancelado: origen y destino son el mismo nodo.", ColorError);
                return;
            }

            var (ruta, distancia) = Dijkstra.RutaMasCorta(_grafo.ObtenerNodos(), origen, destino);

            if (ruta.Count == 0)
            {
                MessageBox.Show("No existe ruta disponible entre esos puntos.", "Sin ruta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                AgregarEntradaLog(
                    $"✗  Sin ruta: no hay camino entre \"{origen.Nombre}\" y \"{destino.Nombre}\".",
                    ColorError);
                return;
            }

            _rutaResaltada = ruta;
            canvasGrafo.Invalidate();

            string rutaStr = string.Join(" → ", ruta.Select(n => n.Nombre));
            lblResultadoRuta.Text = $"Ruta: {rutaStr}   |   Distancia total: {distancia:F2} km";

            AgregarEntradaLog(
                $"→  Dijkstra: \"{origen.Nombre}\" → \"{destino.Nombre}\"" +
                $"  |  {ruta.Count} salto(s)  |  {distancia:F2} km",
                ColorDijkstra);
            AgregarEntradaLog(
                $"   Camino: {rutaStr}",
                System.Drawing.Color.FromArgb(160, 100, 0));
        }

        private void btnLimpiarLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
            AgregarEntradaLog("Log limpiado.", ColorSistema);
        }

        // ═════════════════════════════════════════════════════════════════════════
        // EVENTO — RESIZE DEL CANVAS
        //  Escala proporcionalmente las coordenadas de todos los nodos al nuevo
        //  tamaño sin reorganizar el layout desde cero.
        // ═════════════════════════════════════════════════════════════════════════

        private void canvasGrafo_Resize(object sender, EventArgs e)
        {
            int nuevoAncho = canvasGrafo.Width;
            int nuevoAlto  = canvasGrafo.Height;

            // Ignorar tamaños degenerados o primer frame
            if (nuevoAncho < 100 || nuevoAlto < 100 || _ultimoAncho == 0 || _ultimoAlto == 0)
            {
                _ultimoAncho = nuevoAncho;
                _ultimoAlto  = nuevoAlto;
                return;
            }

            double scaleX = (double)nuevoAncho / _ultimoAncho;
            double scaleY = (double)nuevoAlto  / _ultimoAlto;

            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
            {
                nodo.X = Math.Clamp(nodo.X * scaleX, RADIO_NODO + 20, nuevoAncho - RADIO_NODO - 20);
                nodo.Y = Math.Clamp(nodo.Y * scaleY, RADIO_NODO + 30, nuevoAlto  - RADIO_NODO - 30);
            }

            // Corrección de solapamientos que puedan generarse al encoger el canvas
            SepararNodosSuperpuestos(_grafo.ObtenerNodos(), nuevoAncho, nuevoAlto);

            _ultimoAncho = nuevoAncho;
            _ultimoAlto  = nuevoAlto;

            canvasGrafo.Invalidate();
        }

        // ═════════════════════════════════════════════════════════════════════════
        // PINTADO DEL GRAFO
        // ═════════════════════════════════════════════════════════════════════════

        private void canvasGrafo_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode     = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            List<NodoPunto> nodos      = _grafo.ObtenerNodos();
            Font            fPeso      = new Font("Segoe UI", 7.5F);
            Font            fNombre    = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            // ── Aristas ───────────────────────────────────────────────────────
            foreach (NodoPunto nodo in nodos)
            {
                foreach (Arista arista in nodo.Conexiones)
                {
                    // Dibujar cada arista una sola vez
                    if (nodo.Id >= arista.Destino.Id) continue;

                    bool enRuta = EsAristaEnRuta(nodo, arista.Destino);

                    using Pen pen = enRuta
                        ? new Pen(System.Drawing.Color.DodgerBlue, 3)
                        : new Pen(System.Drawing.Color.Silver, 1.5f);

                    g.DrawLine(pen,
                        (float)nodo.X, (float)nodo.Y,
                        (float)arista.Destino.X, (float)arista.Destino.Y);

                    // ── Etiqueta de distancia con posición inteligente ───────
                    string etiqueta    = $"{arista.Peso:F1} km";
                    SizeF  tamEtiqueta = g.MeasureString(etiqueta, fPeso);

                    System.Drawing.PointF centro = CalcularCentroEtiqueta(nodo, arista.Destino, nodos);
                    float labelX = centro.X - tamEtiqueta.Width  / 2;
                    float labelY = centro.Y - tamEtiqueta.Height / 2;

                    RectangleF fondo = new RectangleF(
                        labelX - 3, labelY - 1,
                        tamEtiqueta.Width + 6, tamEtiqueta.Height + 2);

                    // Fondo con borde sutil para destacar sobre la arista
                    using (var brushFondo = new SolidBrush(
                        System.Drawing.Color.FromArgb(230, 255, 255, 255)))
                        g.FillRectangle(brushFondo, fondo);

                    using (var penFondo = new Pen(
                        System.Drawing.Color.FromArgb(80, 180, 180, 180), 0.8f))
                        g.DrawRectangle(penFondo, fondo.X, fondo.Y, fondo.Width, fondo.Height);

                    System.Drawing.Color colorEtiqueta = enRuta
                        ? System.Drawing.Color.DodgerBlue
                        : System.Drawing.Color.FromArgb(80, 80, 80);

                    using var brushEtiqueta = new SolidBrush(colorEtiqueta);
                    g.DrawString(etiqueta, fPeso, brushEtiqueta, labelX, labelY);
                }
            }

            // ── Nodos ─────────────────────────────────────────────────────────
            foreach (NodoPunto nodo in nodos)
            {
                bool seleccionado = _nodoSeleccionado != null && _nodoSeleccionado.Id == nodo.Id;
                bool enRuta       = _rutaResaltada.Contains(nodo);

                System.Drawing.Color colorNodo =
                    seleccionado ? System.Drawing.Color.OrangeRed
                  : enRuta       ? System.Drawing.Color.DodgerBlue
                  : System.Drawing.Color.FromArgb(60, 80, 100);

                // Sombra suave
                using (var brushSombra = new SolidBrush(
                    System.Drawing.Color.FromArgb(40, 0, 0, 0)))
                    g.FillEllipse(brushSombra,
                        (float)nodo.X - RADIO_NODO + 2,
                        (float)nodo.Y - RADIO_NODO + 2,
                        RADIO_NODO * 2, RADIO_NODO * 2);

                // Círculo relleno
                using (var brushNodo = new SolidBrush(colorNodo))
                    g.FillEllipse(brushNodo,
                        (float)nodo.X - RADIO_NODO,
                        (float)nodo.Y - RADIO_NODO,
                        RADIO_NODO * 2, RADIO_NODO * 2);

                // Borde blanco
                g.DrawEllipse(Pens.White,
                    (float)nodo.X - RADIO_NODO,
                    (float)nodo.Y - RADIO_NODO,
                    RADIO_NODO * 2, RADIO_NODO * 2);

                // Nombre debajo del nodo con fondo para legibilidad
                SizeF tamNombre = g.MeasureString(nodo.Nombre, fNombre);
                float textX     = (float)nodo.X - tamNombre.Width  / 2;
                float textY     = (float)nodo.Y + RADIO_NODO + 3;

                using (var brushNombreFondo = new SolidBrush(
                    System.Drawing.Color.FromArgb(200, 255, 255, 255)))
                    g.FillRectangle(brushNombreFondo,
                        textX - 2, textY - 1,
                        tamNombre.Width + 4, tamNombre.Height + 2);

                g.DrawString(nodo.Nombre, fNombre,
                    seleccionado ? Brushes.OrangeRed : Brushes.Black,
                    textX, textY);
            }

            fPeso.Dispose();
            fNombre.Dispose();
        }

        // ═════════════════════════════════════════════════════════════════════════
        // CLICK EN CANVAS — selección de nodo
        // ═════════════════════════════════════════════════════════════════════════

        private void canvasGrafo_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
            {
                double dist = Math.Sqrt(Math.Pow(e.X - nodo.X, 2) + Math.Pow(e.Y - nodo.Y, 2));
                if (dist <= RADIO_NODO)
                {
                    _nodoSeleccionado = nodo;

                    string infoConexiones = nodo.Conexiones.Count == 0
                        ? "sin conexiones"
                        : string.Join(", ", nodo.Conexiones.Select(a => $"{a.Destino.Nombre} ({a.Peso:F1} km)"));

                    lblNodoInfo.Text =
                        $"● Seleccionado: {nodo.Nombre}  |  " +
                        $"{nodo.Conexiones.Count} conexión(es): {infoConexiones}";

                    AgregarEntradaLog(
                        $"●  Seleccionado: \"{nodo.Nombre}\"  |  " +
                        $"Conexiones ({nodo.Conexiones.Count}): {infoConexiones}",
                        ColorNodo);

                    canvasGrafo.Invalidate();
                    return;
                }
            }

            _nodoSeleccionado = null;
            lblNodoInfo.Text  = "Haz clic sobre un nodo del grafo para ver su información";
            canvasGrafo.Invalidate();
        }

        // ═════════════════════════════════════════════════════════════════════════
        // HELPERS
        // ═════════════════════════════════════════════════════════════════════════

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

        /// <summary>
        /// Calcula el centro ideal para la etiqueta de una arista.
        /// Si el punto medio cae demasiado cerca de algún nodo intermediario,
        /// aplica un offset perpendicular hacia el lado con más espacio.
        /// </summary>
        private System.Drawing.PointF CalcularCentroEtiqueta(
            NodoPunto a, NodoPunto b, List<NodoPunto> todosLosNodos)
        {
            float midX = (float)((a.X + b.X) / 2.0);
            float midY = (float)((a.Y + b.Y) / 2.0);

            // Vector perpendicular a la arista (normalizado)
            double edgeDx = b.X - a.X;
            double edgeDy = b.Y - a.Y;
            double len    = Math.Max(Math.Sqrt(edgeDx * edgeDx + edgeDy * edgeDy), 1.0);
            float  perpX  = (float)(-edgeDy / len);
            float  perpY  = (float)( edgeDx / len);

            // ¿El punto medio queda dentro del radio de algún nodo ajeno?
            bool necesitaOffset = todosLosNodos.Any(n =>
            {
                if (n.Id == a.Id || n.Id == b.Id) return false;
                double d = Math.Sqrt(Math.Pow(n.X - midX, 2) + Math.Pow(n.Y - midY, 2));
                return d < RADIO_NODO + 12;
            });

            // También verificar: aristas muy cortas hacen que el midpoint esté
            // encima de los propios nodos extremos
            if (!necesitaOffset)
            {
                double dA = Math.Sqrt(Math.Pow(a.X - midX, 2) + Math.Pow(a.Y - midY, 2));
                double dB = Math.Sqrt(Math.Pow(b.X - midX, 2) + Math.Pow(b.Y - midY, 2));
                if (dA < RADIO_NODO + 6 || dB < RADIO_NODO + 6)
                    necesitaOffset = true;
            }

            if (!necesitaOffset)
                return new System.Drawing.PointF(midX, midY);

            // Evaluar ambos lados del perpendicular y elegir el más despejado
            float offset = RADIO_NODO + 10f;
            float px1 = midX + perpX * offset, py1 = midY + perpY * offset;
            float px2 = midX - perpX * offset, py2 = midY - perpY * offset;

            double dMin1 = todosLosNodos.Min(n =>
                Math.Sqrt(Math.Pow(n.X - px1, 2) + Math.Pow(n.Y - py1, 2)));
            double dMin2 = todosLosNodos.Min(n =>
                Math.Sqrt(Math.Pow(n.X - px2, 2) + Math.Pow(n.Y - py2, 2)));

            return dMin1 >= dMin2
                ? new System.Drawing.PointF(px1, py1)
                : new System.Drawing.PointF(px2, py2);
        }

        // ─── Trazabilidad ─────────────────────────────────────────────────────────

        private void AgregarEntradaLog(string mensaje, System.Drawing.Color color)
        {
            string linea = $"{DateTime.Now:HH:mm:ss}  {mensaje}{Environment.NewLine}";

            rtbLog.SelectionStart  = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor  = color;
            rtbLog.AppendText(linea);
            rtbLog.SelectionColor  = rtbLog.ForeColor;
            rtbLog.ScrollToCaret();
        }
    }
}
