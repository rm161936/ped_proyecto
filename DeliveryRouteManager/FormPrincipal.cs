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

        public FormPrincipal()
        {
            InitializeComponent();
            _db       = new DbManager();
            _grafo    = new Grafo();
            _cola     = new ColaPedidos();
            _historial = new HistorialEntregas();

            CargarGrafoDesdeDb();
            CargarPedidosEnCola();
        }

        private void CargarGrafoDesdeDb()
        {
            foreach (var punto in _db.CargarPuntos())
                _grafo.AgregarNodo(punto);

            foreach (var (idOrigen, idDestino, peso) in _db.CargarRutas())
                _grafo.AgregarArista(idOrigen, idDestino, peso);
        }

        private void CargarPedidosEnCola()
        {
            Dictionary<int, NodoPunto> mapa = new();
            foreach (NodoPunto nodo in _grafo.ObtenerNodos())
                mapa[nodo.Id] = nodo;

            foreach (var (id, cliente, idPunto, prioridad, fechaStr) in _db.CargarPedidosRaw())
            {
                if (!mapa.ContainsKey(idPunto)) continue;

                Pedido pedido = new Pedido(id, cliente, mapa[idPunto], (Prioridad)prioridad)
                {
                    FechaRegistro = DateTime.TryParse(fechaStr, out DateTime dt) ? dt : DateTime.Now
                };

                _cola.Encolar(pedido);
            }
        }

        private void BtnGrafo_Click(object sender, EventArgs e)
        {
            FormGrafo formGrafo = new(_grafo, _db)
            {
                MdiParent = this
            };
            formGrafo.Show();
        }

        private void BtnPedidos_Click(object sender, EventArgs e)
        {
            FormPedidos formPedidos = new(_grafo, _db, _cola, _historial)
            {
                MdiParent = this
            };
            formPedidos.Show();
        }

        private void BtnHistorial_Click(object sender, EventArgs e)
        {
            FormHistorial formHistorial = new(_db, _historial)
            {
                MdiParent = this
            };
            formHistorial.Show();
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
