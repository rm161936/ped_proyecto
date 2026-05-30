using DeliveryRouteManager.Algorithms;
using DeliveryRouteManager.Database;
using DeliveryRouteManager.DataStructures;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.Forms
{
    public partial class FormPedidos : Form
    {
        private readonly Grafo _grafo;
        private readonly DbManager _db;
        private readonly ColaPedidos _cola;
        private readonly HistorialEntregas _historial;

        public FormPedidos(Grafo grafo, DbManager db, ColaPedidos cola, HistorialEntregas historial)
        {
            InitializeComponent();
            _grafo    = grafo;
            _db       = db;
            _cola     = cola;
            _historial = historial;

            CargarCombos();
            ActualizarListaCola();
        }

        // ─── INICIALIZACIÓN ───────────────────────────────────────────────────────

        private void CargarCombos()
        {
            cmbPunto.Items.Clear();
            cmbOrigen.Items.Clear();

            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
            {
                cmbPunto.Items.Add(nodo);
                cmbOrigen.Items.Add(nodo);
            }

            cmbPrioridad.Items.Clear();
            cmbPrioridad.Items.Add(Prioridad.Alta);
            cmbPrioridad.Items.Add(Prioridad.Media);
            cmbPrioridad.Items.Add(Prioridad.Baja);
            cmbPrioridad.SelectedIndex = 0;
        }

        // ─── EVENTOS ──────────────────────────────────────────────────────────────

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string cliente = txtCliente.Text.Trim();
            if (string.IsNullOrEmpty(cliente))
            {
                MessageBox.Show("Ingresa el nombre del cliente.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPunto.SelectedItem == null)
            {
                MessageBox.Show("Selecciona el punto de entrega.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPrioridad.SelectedItem == null)
            {
                MessageBox.Show("Selecciona la prioridad.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NodoPunto punto     = (NodoPunto)cmbPunto.SelectedItem;
            Prioridad prioridad = (Prioridad)cmbPrioridad.SelectedItem;
            string fecha        = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            int id = _db.InsertarPedido(cliente, punto.Id, (int)prioridad, fecha);

            Pedido pedido = new Pedido(id, cliente, punto, prioridad)
            {
                FechaRegistro = DateTime.Parse(fecha)
            };

            _cola.Encolar(pedido);
            ActualizarListaCola();

            txtCliente.Clear();
            cmbPunto.SelectedIndex = -1;
            cmbPrioridad.SelectedIndex = 0;
            txtCliente.Focus();

            lblResultadoDespacho.BackColor = System.Drawing.Color.FromArgb(230, 245, 255);
            lblResultadoDespacho.ForeColor = System.Drawing.Color.FromArgb(20, 80, 160);
            lblResultadoDespacho.Text = $"Pedido de \"{cliente}\" registrado con prioridad {prioridad}.";
        }

        private void btnDespachar_Click(object sender, EventArgs e)
        {
            if (_cola.EstaVacia())
            {
                MessageBox.Show("No hay pedidos en la cola.", "Cola vacía",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbOrigen.SelectedItem == null)
            {
                MessageBox.Show("Selecciona el punto de origen del repartidor.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NodoPunto origen = (NodoPunto)cmbOrigen.SelectedItem;
            Pedido pedido    = _cola.VerPrimero()!;

            if (origen.Id == pedido.PuntoEntrega.Id)
            {
                MessageBox.Show("El origen y el destino del pedido son el mismo punto.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var (ruta, distancia) = Dijkstra.RutaMasCorta(_grafo.ObtenerNodos(), origen, pedido.PuntoEntrega);

            if (ruta.Count == 0)
            {
                MessageBox.Show(
                    $"No existe ruta disponible desde \"{origen.Nombre}\" hasta \"{pedido.PuntoEntrega.Nombre}\".\n" +
                    "Verifica las conexiones en el módulo de Rutas y Puntos.",
                    "Sin ruta",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmado: desencolar y registrar entrega
            _cola.Desencolar();

            string rutaStr   = string.Join(" → ", ruta.Select(n => n.Nombre));
            string fechaStr  = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _historial.AgregarEntrega(pedido, rutaStr, distancia);
            _db.InsertarHistorial(pedido.Id, pedido.Cliente, rutaStr, distancia, fechaStr);
            _db.EliminarPedido(pedido.Id);

            ActualizarListaCola();

            lblResultadoDespacho.BackColor = System.Drawing.Color.FromArgb(230, 255, 235);
            lblResultadoDespacho.ForeColor = System.Drawing.Color.FromArgb(20, 110, 30);
            lblResultadoDespacho.Text =
                $"Entregado: [{pedido.Prioridad}] {pedido.Cliente} → {pedido.PuntoEntrega.Nombre} | " +
                $"Ruta: {rutaStr} | {distancia:F2} km";
        }

        // ─── HELPERS ──────────────────────────────────────────────────────────────

        private void ActualizarListaCola()
        {
            listCola.Items.Clear();
            int pos = 1;

            foreach (Pedido p in _cola.VerCola())
            {
                ListViewItem item = new ListViewItem(pos.ToString());
                item.SubItems.Add(p.Cliente);
                item.SubItems.Add(p.PuntoEntrega.Nombre);
                item.SubItems.Add(p.Prioridad.ToString());
                item.SubItems.Add(p.FechaRegistro.ToString("dd/MM/yyyy HH:mm"));

                // Color por prioridad
                item.BackColor = p.Prioridad switch
                {
                    Prioridad.Alta  => System.Drawing.Color.FromArgb(255, 235, 235),
                    Prioridad.Media => System.Drawing.Color.FromArgb(255, 250, 220),
                    _               => System.Drawing.Color.White
                };

                listCola.Items.Add(item);
                pos++;
            }

            lblTotalCola.Text = $"Pedidos en cola: {_cola.Total()}";
        }
    }
}
