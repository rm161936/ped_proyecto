using DeliveryRouteManager.Database;
using DeliveryRouteManager.DataStructures;
using DeliveryRouteManager.Forms;

namespace DeliveryRouteManager
{
    public partial class FormPrincipal : Form
    {
        private Grafo _grafo;
        private DbManager _db;

        public FormPrincipal()
        {
            InitializeComponent();
            _db = new DbManager();
            _grafo = new Grafo();
            CargarGrafoDesdeDb();
        }

        private void CargarGrafoDesdeDb()
        {
            foreach (var punto in _db.CargarPuntos())
                _grafo.AgregarNodo(punto);

            foreach (var (idOrigen, idDestino, peso) in _db.CargarRutas())
                _grafo.AgregarArista(idOrigen, idDestino, peso);
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
            FormPedidos formPedidos = new()
            {
                MdiParent = this
            };
            formPedidos.Show();
        }

        private void BtnHistorial_Click(object sender, EventArgs e)
        {
            FormHistorial formHistorial = new()
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
