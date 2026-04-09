using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.DataStructures
{
    public class NodoHistorial(Pedido pedido, string rutaRecorrida, double distanciaTotal)
    {
        public Pedido Pedido { get; set; } = pedido;
        public string RutaRecorrida { get; set; } = rutaRecorrida;
        public double DistanciaTotal { get; set; } = distanciaTotal;
        public DateTime FechaEntrega { get; set; } = DateTime.Now;
        public NodoHistorial? Siguiente { get; set; } = null;
    }
}
