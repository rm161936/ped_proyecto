using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.DataStructures
{
    public class Grafo
    {
        private readonly Dictionary<int, NodoPunto> _nodos;

        public Grafo()
        {
            _nodos = [];
        }

        public void AgregarNodo(NodoPunto nodo)
        {
            if (!_nodos.ContainsKey(nodo.Id))
                _nodos[nodo.Id] = nodo;
        }

        public void AgregarArista(int idOrigen, int idDestino, double peso)
        {
            if (!_nodos.TryGetValue(idOrigen, out NodoPunto? origen) || !_nodos.TryGetValue(idDestino, out NodoPunto? destino))
                return;
            origen.Conexiones.Add(new Arista(destino, peso));
            destino.Conexiones.Add(new Arista(origen, peso));
        }

        public NodoPunto? ObtenerNodo(int id) =>
            _nodos.TryGetValue(id, out NodoPunto? value) ? value : null;

        public List<NodoPunto> ObtenerNodos() =>
            [.. _nodos.Values];

        public bool ExisteConexion(int idOrigen, int idDestino)
        {
            if (!_nodos.TryGetValue(idOrigen, out NodoPunto? value)) return false;
            return value.Conexiones.Any(a => a.Destino.Id == idDestino);
        }

        public void EliminarNodo(int id)
        {
            if (!_nodos.ContainsKey(id)) return;

            foreach (NodoPunto nodo in _nodos.Values)
                nodo.Conexiones.RemoveAll(a => a.Destino.Id == id);

            _nodos.Remove(id);
        }

        public int TotalNodos() => _nodos.Count;

    }
}
