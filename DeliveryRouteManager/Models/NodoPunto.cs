using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryRouteManager.Models
{
    public class NodoPunto(int id, string nombre, double x, double y)
    {
        public int Id { get; set; } = id;
        public string Nombre { get; set; } = nombre;
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public List<Arista> Conexiones { get; set; } = [];

        public override string ToString() => Nombre;
    }
}
