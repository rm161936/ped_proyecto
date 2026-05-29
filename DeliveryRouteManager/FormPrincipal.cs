using DeliveryRouteManager.Database;
using DeliveryRouteManager.DataStructures;
using DeliveryRouteManager.Forms;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager
{
    public partial class FormPrincipal : Form
    {
        private Grafo _grafo;
        private DbManager _db;
        private ColaPedidos _cola;
        private HistorialEntregas _historial;
        private Stack<(Pedido Pedido, List<NodoPunto> Ruta)> _pilaDeshacer;

        public FormPrincipal()
        {
            InitializeComponent();
            _db = new DbManager();
            _grafo = new Grafo();
            _cola = new ColaPedidos();
            _historial = new HistorialEntregas();
            _pilaDeshacer = new Stack<(Pedido, List<NodoPunto>)>();
            CargarGrafoDesdeDb();
        }

        private void CargarGrafoDesdeDb()
        {
            foreach (var punto in _db.CargarPuntos())
                _grafo.AgregarNodo(punto);

            foreach (var (idOrigen, idDestino, peso) in _db.CargarRutas())
                _grafo.AgregarArista(idOrigen, idDestino, peso);

            CargarPedidosDesdeDb();
            CargarHistorialDesdeDb();
        }

        private void CargarPedidosDesdeDb()
        {
            foreach (var (id, cliente, idPunto, prioridad, fechaRegistro) in _db.CargarPedidos())
            {
                NodoPunto? punto = _grafo.ObtenerNodo(idPunto);
                if (punto == null) continue;

                Pedido pedido = new(id, cliente, punto, (Prioridad)prioridad);
                if (DateTime.TryParse(fechaRegistro, out DateTime fr))
                    pedido.FechaRegistro = fr;

                _cola.Encolar(pedido);
            }
        }

        private void CargarHistorialDesdeDb()
        {
            foreach (var (idPedido, cliente, idPunto, prioridad,
                           fechaRegistroPedido, rutaRecorrida,
                           distanciaTotal, fechaEntrega) in _db.CargarHistorial())
            {
                NodoPunto? punto = _grafo.ObtenerNodo(idPunto);
                // Placeholder si el nodo fue eliminado, para no perder el registro
                NodoPunto puntoFinal = punto ?? new NodoPunto(idPunto, $"Punto #{idPunto}", 0, 0);

                Pedido pedido = new(idPedido, cliente, puntoFinal, (Prioridad)prioridad);
                if (DateTime.TryParse(fechaRegistroPedido, out DateTime fr))
                    pedido.FechaRegistro = fr;

                DateTime fechaEnt = DateTime.TryParse(fechaEntrega, out DateTime fe)
                    ? fe : DateTime.Now;

                _historial.AgregarEntrega(pedido, rutaRecorrida, distanciaTotal, fechaEnt);
            }
        }

        // ── HELPER: evita instancias duplicadas ───────────────────────────────────
        private void AbrirFormulario<T>(Func<Form> crearForm) where T : Form
        {
            // Busca si ya hay una instancia abierta de ese tipo
            Form? existente = MdiChildren.OfType<T>().FirstOrDefault();

            if (existente != null)
            {
                // Si está minimizado, lo restaura
                if (existente.WindowState == FormWindowState.Minimized)
                    existente.WindowState = FormWindowState.Normal;

                existente.Activate();
                return;
            }

            // Si no existe, crea uno nuevo
            Form nuevo = crearForm();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        // ── BOTONES ───────────────────────────────────────────────────────────────

        private void BtnGrafo_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormGrafo>(() => new FormGrafo(_grafo, _db));
        }

        private void BtnPedidos_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormPedidos>(
                () => new FormPedidos(_grafo, _db, _cola, _historial, _pilaDeshacer));
        }

        private void BtnHistorial_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormHistorial>(() => new FormHistorial(_historial, _db));
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "¿Deseas salir del sistema?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
                Application.Exit();
        }
    }
}