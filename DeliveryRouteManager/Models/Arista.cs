using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryRouteManager.Models
{
    public class Arista(NodoPunto destino, double peso)
    {
        public NodoPunto Destino { get; set; } = destino;
        public double Peso { get; set; } = peso;
    }
}
