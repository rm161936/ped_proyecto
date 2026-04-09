using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.DataStructures
{
    public class ColaPedidos
    {
        private readonly SortedList<(int Prioridad, DateTime Fecha), Pedido> _cola;

        public ColaPedidos()
        {
            _cola = new SortedList<(int Prioridad, DateTime Fecha), Pedido>(
                Comparer<(int Prioridad, DateTime Fecha)>.Create((a, b) =>
                {
                    int cmp = a.Prioridad.CompareTo(b.Prioridad);
                    return cmp != 0 ? cmp : a.Fecha.CompareTo(b.Fecha);
                })
            );
        }

        public void Encolar(Pedido pedido)
        {
            _cola.Add(((int)pedido.Prioridad, pedido.FechaRegistro), pedido);
        }

        public Pedido? Desencolar()
        {
            if (EstaVacia()) return null;
            Pedido primero = _cola.Values[0];
            _cola.RemoveAt(0);
            return primero;
        }

        public Pedido? VerPrimero() =>
            EstaVacia() ? null : _cola.Values[0];

        public List<Pedido> VerCola() =>
            [.. _cola.Values];

        public bool EstaVacia() => _cola.Count == 0;

        public int Total() => _cola.Count;
    }

}
