using DeliveryRouteManager.DataStructures;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.Forms
{
    public partial class FormHistorial : Form
    {
        private readonly HistorialEntregas _historial;

        public FormHistorial(HistorialEntregas historial)
        {
            InitializeComponent();
            _historial = historial;
            CargarHistorial();
        }

        // ── CARGA Y FILTRO ────────────────────────────────────────────────────────

        private void CargarHistorial(string filtroCliente = "", DateTime? filtroFecha = null)
        {
            dgvHistorial.Rows.Clear();

            List<NodoHistorial> lista = _historial.ObtenerHistorial();

            foreach (NodoHistorial nodo in lista)
            {
                bool coincideCliente = string.IsNullOrEmpty(filtroCliente)
                    || nodo.Pedido.Cliente.ToLower().Contains(filtroCliente.ToLower());

                bool coincideFecha = filtroFecha == null
                    || nodo.FechaEntrega.Date == filtroFecha.Value.Date;

                if (!coincideCliente || !coincideFecha) continue;

                string prioridadTexto = nodo.Pedido.Prioridad.ToString();

                dgvHistorial.Rows.Add(
                    nodo.Pedido.Id,
                    nodo.Pedido.Cliente,
                    nodo.Pedido.PuntoEntrega.Nombre,
                    prioridadTexto,
                    nodo.RutaRecorrida,
                    $"{nodo.DistanciaTotal:F2} km",
                    nodo.FechaEntrega.ToString("dd/MM/yyyy HH:mm")
                );
            }

            lblTotalEntregas.Text = $"Total entregas: {_historial.TotalEntregas()}  |  " +
                                    $"Mostrando: {dgvHistorial.Rows.Count}";
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