using DeliveryRouteManager.Database;
using DeliveryRouteManager.DataStructures;

namespace DeliveryRouteManager.Forms
{
    public partial class FormHistorial : Form
    {
        private readonly DbManager _db;
        private readonly HistorialEntregas _historial;

        public FormHistorial(DbManager db, HistorialEntregas historial)
        {
            InitializeComponent();
            _db       = db;
            _historial = historial;

            CargarHistorial();
        }

        // ─── EVENTOS ──────────────────────────────────────────────────────────────

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarHistorial();
        }

        // ─── LÓGICA ───────────────────────────────────────────────────────────────

        private void CargarHistorial()
        {
            listHistorial.Items.Clear();

            var registros = _db.CargarHistorialCompleto();
            int pos = 1;

            foreach (var (_, _, cliente, ruta, distancia, fecha) in registros)
            {
                ListViewItem item = new ListViewItem(pos.ToString());
                item.SubItems.Add(cliente);
                item.SubItems.Add(ruta);
                item.SubItems.Add(distancia.ToString("F2"));
                item.SubItems.Add(fecha);

                // Alternar color de filas
                item.BackColor = pos % 2 == 0
                    ? System.Drawing.Color.FromArgb(245, 248, 255)
                    : System.Drawing.Color.White;

                listHistorial.Items.Add(item);
                pos++;
            }

            lblTotalHistorial.Text = $"Total de entregas registradas: {registros.Count}  " +
                                     $"(en memoria: {_historial.TotalEntregas()})";
        }
    }
}
