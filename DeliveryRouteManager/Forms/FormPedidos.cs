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
        private readonly Stack<(Pedido Pedido, List<NodoPunto> Ruta)> _pilaDeshacer;

        private Pedido? _pedidoEnProceso;
        private List<NodoPunto> _rutaEnProceso;

        public FormPedidos(
            Grafo grafo,
            DbManager db,
            ColaPedidos cola,
            HistorialEntregas historial,
            Stack<(Pedido, List<NodoPunto>)> pilaDeshacer)
        {
            InitializeComponent();
            _grafo = grafo;
            _db = db;
            _cola = cola;
            _historial = historial;
            _pilaDeshacer = pilaDeshacer;
            _pedidoEnProceso = null;
            _rutaEnProceso = [];
            CargarDatos();
        }

        private void CargarDatos()
        {
            cmbPuntoEntrega.Items.Clear();
            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
                cmbPuntoEntrega.Items.Add(nodo);

            cmbPrioridad.Items.Clear();
            cmbPrioridad.Items.Add(Prioridad.Alta);
            cmbPrioridad.Items.Add(Prioridad.Media);
            cmbPrioridad.Items.Add(Prioridad.Baja);
            cmbPrioridad.SelectedIndex = 0;

            ActualizarVistaCola();
            ActualizarBotones();
        }

        private void ActualizarVistaCola()
        {
            dgvCola.Rows.Clear();
            foreach (Pedido p in _cola.VerCola())
            {
                dgvCola.Rows.Add(
                    p.Id,
                    p.Cliente,
                    p.PuntoEntrega.Nombre,
                    p.Prioridad.ToString(),
                    p.FechaRegistro.ToString("dd/MM/yyyy HH:mm")
                );
            }
            lblTotalCola.Text = $"Pedidos en cola: {_cola.Total()}";
        }

        private void ActualizarBotones()
        {
            btnAtenderSiguiente.Enabled = !_cola.EstaVacia() && _pedidoEnProceso == null;
            btnConfirmarEntrega.Enabled = _pedidoEnProceso != null;
            btnDeshacer.Enabled = _pilaDeshacer.Count > 0;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            NodoPunto? seleccionado = cmbPuntoEntrega.SelectedItem as NodoPunto;

            cmbPuntoEntrega.Items.Clear();
            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
                cmbPuntoEntrega.Items.Add(nodo);

            // Restaura la selección previa si todavía existe
            if (seleccionado != null)
            {
                NodoPunto? restaurado = cmbPuntoEntrega.Items
                    .OfType<NodoPunto>()
                    .FirstOrDefault(n => n.Id == seleccionado.Id);
                cmbPuntoEntrega.SelectedItem = restaurado;
            }
        }

        // ── REGISTRAR PEDIDO ──────────────────────────────────────────────────────

        private void btnRegistrarPedido_Click(object sender, EventArgs e)
        {
            string cliente = txtCliente.Text.Trim();
            if (string.IsNullOrEmpty(cliente))
            {
                MessageBox.Show("Ingresa el nombre del cliente.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPuntoEntrega.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un punto de entrega.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NodoPunto punto = (NodoPunto)cmbPuntoEntrega.SelectedItem;
            Prioridad prioridad = (Prioridad)cmbPrioridad.SelectedItem!;

            int id = _db.InsertarPedido(
                cliente,
                punto.Id,
                (int)prioridad,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            );

            Pedido nuevo = new(id, cliente, punto, prioridad);
            _cola.Encolar(nuevo);

            txtCliente.Clear();
            cmbPuntoEntrega.SelectedIndex = -1;
            cmbPrioridad.SelectedIndex = 0;

            ActualizarVistaCola();
            ActualizarBotones();

            MessageBox.Show($"Pedido #{id} registrado correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── ATENDER SIGUIENTE ─────────────────────────────────────────────────────

        private void btnAtenderSiguiente_Click(object sender, EventArgs e)
        {
            if (_cola.EstaVacia())
            {
                MessageBox.Show("No hay pedidos pendientes.", "Cola vacía",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            NodoPunto? almacen = _grafo.ObtenerNodos()
                .FirstOrDefault(n => n.Nombre.ToLower().Contains("almac"))
                ?? _grafo.ObtenerNodos().FirstOrDefault();

            if (almacen == null)
            {
                MessageBox.Show("No hay puntos registrados en el grafo.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Pedido pedido = _cola.Desencolar()!;
            var (ruta, distancia) = Dijkstra.RutaMasCorta(
                _grafo.ObtenerNodos(), almacen, pedido.PuntoEntrega);

            if (ruta.Count == 0)
            {
                MessageBox.Show(
                    $"No existe ruta hacia '{pedido.PuntoEntrega.Nombre}'.\nEl pedido fue removido de la cola.",
                    "Sin ruta disponible",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ActualizarVistaCola();
                ActualizarBotones();
                return;
            }

            _pedidoEnProceso = pedido;
            _rutaEnProceso = ruta;
            _pilaDeshacer.Push((pedido, ruta));

            string rutaTexto = string.Join(" → ", ruta);
            lblRutaAsignada.Text =
                $"Pedido #{pedido.Id}  |  Cliente: {pedido.Cliente}  |  Prioridad: {pedido.Prioridad}\n" +
                $"Ruta: {rutaTexto}\n" +
                $"Distancia total: {distancia:F2} km";

            ActualizarVistaCola();
            ActualizarBotones();
        }

        // ── CONFIRMAR ENTREGA ─────────────────────────────────────────────────────

        private void btnConfirmarEntrega_Click(object sender, EventArgs e)
        {
            if (_pedidoEnProceso == null) return;

            string rutaTexto = string.Join(" → ", _rutaEnProceso);
            double distancia = CalcularDistanciaRuta(_rutaEnProceso);

            // Guarda en lista enlazada en memoria
            _historial.AgregarEntrega(_pedidoEnProceso, rutaTexto, distancia, DateTime.Now);

            // Guarda en BD con todos los campos del pedido
            _db.InsertarHistorial(
                _pedidoEnProceso.Id,
                _pedidoEnProceso.Cliente,
                _pedidoEnProceso.PuntoEntrega.Id,
                (int)_pedidoEnProceso.Prioridad,
                _pedidoEnProceso.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss"),
                rutaTexto,
                distancia,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            );

            // Elimina el pedido de la tabla Pedidos
            _db.EliminarPedido(_pedidoEnProceso.Id);

            MessageBox.Show(
                $"Entrega #{_pedidoEnProceso.Id} confirmada.\nCliente: {_pedidoEnProceso.Cliente}",
                "Entrega confirmada",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            _pedidoEnProceso = null;
            _rutaEnProceso = [];
            lblRutaAsignada.Text = "Sin pedido en proceso actualmente.";
            ActualizarBotones();
        }

        // ── DESHACER ──────────────────────────────────────────────────────────────

        private void btnDeshacer_Click(object sender, EventArgs e)
        {
            if (_pilaDeshacer.Count == 0) return;

            var (pedido, _) = _pilaDeshacer.Pop();

            if (_pedidoEnProceso != null && _pedidoEnProceso.Id == pedido.Id)
            {
                _cola.Encolar(_pedidoEnProceso);
                _pedidoEnProceso = null;
                _rutaEnProceso = [];
                lblRutaAsignada.Text = "Sin pedido en proceso actualmente.";
            }

            ActualizarVistaCola();
            ActualizarBotones();

            MessageBox.Show(
                $"Asignación del pedido #{pedido.Id} deshecha.\nRegresado a la cola.",
                "Deshacer",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── HELPERS ───────────────────────────────────────────────────────────────

        private double CalcularDistanciaRuta(List<NodoPunto> ruta)
        {
            double total = 0;
            for (int i = 0; i < ruta.Count - 1; i++)
            {
                Arista? arista = ruta[i].Conexiones
                    .FirstOrDefault(a => a.Destino.Id == ruta[i + 1].Id);
                if (arista != null)
                    total += arista.Peso;
            }
            return total;
        }
    }
}