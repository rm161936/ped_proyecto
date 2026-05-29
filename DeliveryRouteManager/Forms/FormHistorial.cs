using DeliveryRouteManager.Database;
using DeliveryRouteManager.DataStructures;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.Forms
{
    public partial class FormHistorial : Form
    {
        private readonly HistorialEntregas _historial;
        private readonly DbManager _db;

        public FormHistorial(HistorialEntregas historial, DbManager db)
        {
            InitializeComponent();
            _historial = historial;
            _db = db;
            CargarHistorial();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            CargarHistorial();
        }

        // ── CARGA Y FILTRO ────────────────────────────────────────────────────────

        private void CargarHistorial(string filtroCliente = "", DateTime? filtroFecha = null)
        {
            dgvHistorial.Rows.Clear();

            var registros = _db.CargarHistorialCompleto();
            int mostrados = 0;

            foreach (var (idPedido, cliente, nombrePunto, prioridad,
                          rutaRecorrida, distanciaTotal, fechaEntrega) in registros)
            {
                bool coincideCliente = string.IsNullOrEmpty(filtroCliente)
                    || cliente.ToLower().Contains(filtroCliente.ToLower());

                bool coincideFecha = filtroFecha == null
                    || (DateTime.TryParse(fechaEntrega, out DateTime fe)
                        && fe.Date == filtroFecha.Value.Date);

                if (!coincideCliente || !coincideFecha) continue;

                string prioridadTexto = ((Prioridad)prioridad).ToString();
                string fechaMostrar = DateTime.TryParse(fechaEntrega, out DateTime fd)
                    ? fd.ToString("dd/MM/yyyy HH:mm")
                    : fechaEntrega;

                dgvHistorial.Rows.Add(
                    idPedido,
                    cliente,
                    nombrePunto,
                    prioridadTexto,
                    rutaRecorrida,
                    $"{distanciaTotal:F2} km",
                    fechaMostrar
                );
                mostrados++;
            }

            lblTotalEntregas.Text =
                $"Total entregas: {registros.Count}  |  Mostrando: {mostrados}";
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            string cliente = txtFiltroCliente.Text.Trim();
            DateTime? fecha = chkFiltroFecha.Checked ? dtpFiltroFecha.Value : null;
            CargarHistorial(cliente, fecha);
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFiltroCliente.Clear();
            chkFiltroFecha.Checked = false;
            dtpFiltroFecha.Enabled = false;
            CargarHistorial();
        }

        private void chkFiltroFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFiltroFecha.Enabled = chkFiltroFecha.Checked;
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            txtFiltroCliente.Clear();
            chkFiltroFecha.Checked = false;
            dtpFiltroFecha.Enabled = false;
            CargarHistorial();
        }

        // ── SELECCIÓN DE FILA → DETALLE ───────────────────────────────────────────

        private void dgvHistorial_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHistorial.SelectedRows.Count == 0)
            {
                lblDetalle.Text = "Selecciona una entrega para ver el detalle de la ruta.";
                return;
            }

            DataGridViewRow fila = dgvHistorial.SelectedRows[0];
            lblDetalle.Text =
                $"Pedido #{fila.Cells["colId"].Value}  |  " +
                $"Cliente: {fila.Cells["colCliente"].Value}  |  " +
                $"Destino: {fila.Cells["colPunto"].Value}  |  " +
                $"Prioridad: {fila.Cells["colPrioridad"].Value}\n" +
                $"Ruta recorrida: {fila.Cells["colRuta"].Value}\n" +
                $"Distancia total: {fila.Cells["colDistancia"].Value}  |  " +
                $"Fecha entrega: {fila.Cells["colFecha"].Value}";
        }
    }
}