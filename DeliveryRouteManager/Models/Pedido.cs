using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryRouteManager.Models
{
    public class Pedido(int id, string cliente, NodoPunto puntoEntrega, Prioridad prioridad)
    {
        public int Id { get; set; } = id;
        public string Cliente { get; set; } = cliente;
        public NodoPunto PuntoEntrega { get; set; } = puntoEntrega;
        public Prioridad Prioridad { get; set; } = prioridad;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public override string ToString() =>
            $"[{Prioridad}] Pedido #{Id} - {Cliente} → {PuntoEntrega.Nombre}";
    }
}
