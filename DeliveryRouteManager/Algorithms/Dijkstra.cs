using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryRouteManager.Models;

namespace DeliveryRouteManager.Algorithms
{
    public static class Dijkstra
    {
        public static (List<NodoPunto> Ruta, double DistanciaTotal) RutaMasCorta(
            List<NodoPunto> nodos,
            NodoPunto origen,
            NodoPunto destino)
        {
            Dictionary<int, double> distancias = new Dictionary<int, double>();
            Dictionary<int, NodoPunto> anteriores = new Dictionary<int, NodoPunto>();
            List<NodoPunto> noVisitados = new List<NodoPunto>(nodos);

            foreach (NodoPunto nodo in nodos)
            {
                distancias[nodo.Id] = double.MaxValue;
                anteriores[nodo.Id] = null;
            }

            distancias[origen.Id] = 0;

            while (noVisitados.Count > 0)
            {
                NodoPunto actual = ObtenerMinimo(noVisitados, distancias);
                if (actual == null || distancias[actual.Id] == double.MaxValue) break;
                if (actual.Id == destino.Id) break;

                noVisitados.Remove(actual);

                foreach (Arista arista in actual.Conexiones)
                {
                    if (!noVisitados.Contains(arista.Destino)) continue;

                    double nuevaDistancia = distancias[actual.Id] + arista.Peso;
                    if (nuevaDistancia < distancias[arista.Destino.Id])
                    {
                        distancias[arista.Destino.Id] = nuevaDistancia;
                        anteriores[arista.Destino.Id] = actual;
                    }
                }
            }

            return ReconstruirRuta(anteriores, distancias, destino);
        }

        private static NodoPunto ObtenerMinimo(
            List<NodoPunto> noVisitados,
            Dictionary<int, double> distancias)
        {
            NodoPunto minimo = null;
            double menorDistancia = double.MaxValue;

            foreach (NodoPunto nodo in noVisitados)
            {
                if (distancias[nodo.Id] < menorDistancia)
                {
                    menorDistancia = distancias[nodo.Id];
                    minimo = nodo;
                }
            }

            return minimo;
        }

        private static (List<NodoPunto> Ruta, double DistanciaTotal) ReconstruirRuta(
            Dictionary<int, NodoPunto> anteriores,
            Dictionary<int, double> distancias,
            NodoPunto destino)
        {
            List<NodoPunto> ruta = new List<NodoPunto>();

            if (distancias[destino.Id] == double.MaxValue)
                return (ruta, 0);

            NodoPunto actual = destino;
            while (actual != null)
            {
                ruta.Insert(0, actual);
                actual = anteriores[actual.Id];
            }

            return (ruta, distancias[destino.Id]);
        }
    }

}
