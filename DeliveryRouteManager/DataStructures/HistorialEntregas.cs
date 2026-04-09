using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.DataStructures
{
    public class HistorialEntregas
    {
        private NodoHistorial? _cabeza;
        private int _total;

        public HistorialEntregas()
        {
            _cabeza = null;
            _total = 0;
        }

        public void AgregarEntrega(Pedido pedido, string rutaRecorrida, double distanciaTotal)
        {
            NodoHistorial nuevo = new(pedido, rutaRecorrida, distanciaTotal)
            {
                Siguiente = _cabeza
            };
            _cabeza = nuevo;
            _total++;
        }

        public List<NodoHistorial> ObtenerHistorial()
        {
            List<NodoHistorial> lista = [];
            NodoHistorial actual = _cabeza;
            while (actual != null)
            {
                lista.Add(actual);
                actual = actual.Siguiente;
            }
            return lista;
        }

        public int TotalEntregas() => _total;

    }
}
