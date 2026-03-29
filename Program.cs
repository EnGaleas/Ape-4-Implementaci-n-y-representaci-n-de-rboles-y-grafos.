// ==========================================================
// PROYECTO: Sistema de Vuelos Baratos con Grafos
// AUTORES:
// - Erminia Galeas
// - Lady Guerrero
// - Andrea Alejandro
// ==========================================================

using System;
using System.Collections.Generic;

class Program
{
    // ===================== GRAFO =====================
    static Dictionary<string, List<(string destino, int costo)>> grafo =
        new Dictionary<string, List<(string, int)>>()
    {
        { "Quito", new List<(string, int)> { ("Guayaquil", 50), ("Cuenca", 70) } },
        { "Guayaquil", new List<(string, int)> { ("Quito", 50), ("Cuenca", 40), ("Manta", 30) } },
        { "Cuenca", new List<(string, int)> { ("Quito", 70), ("Guayaquil", 40), ("Loja", 60) } },
        { "Manta", new List<(string, int)> { ("Guayaquil", 30) } },
        { "Loja", new List<(string, int)> { ("Cuenca", 60) } }
    };

    // ===================== REPORTERÍA =====================
    static void MostrarVuelos()
    {
        Console.WriteLine("\n--- LISTA DE VUELOS DISPONIBLES ---");

        foreach (var ciudad in grafo)
        {
            Console.WriteLine("\nDesde " + ciudad.Key + ":");

            foreach (var vuelo in ciudad.Value)
            {
                Console.WriteLine(" -> " + vuelo.destino + " ($" + vuelo.costo + ")");
            }
        }
    }

    // ===================== DIJKSTRA =====================
    static Dictionary<string, int> Dijkstra(string inicio)
    {
        var distancias = new Dictionary<string, int>();
        var visitados = new HashSet<string>();

        foreach (var ciudad in grafo.Keys)
            distancias[ciudad] = int.MaxValue;

        distancias[inicio] = 0;

        while (visitados.Count < grafo.Count)
        {
            // Buscar nodo no visitado con menor distancia
            string actual = null;
            int min = int.MaxValue;

            foreach (var ciudad in distancias)
            {
                if (!visitados.Contains(ciudad.Key) && ciudad.Value < min)
                {
                    min = ciudad.Value;
                    actual = ciudad.Key;
                }
            }

            if (actual == null) break;

            visitados.Add(actual);

            foreach (var vecino in grafo[actual])
            {
                int nueva = distancias[actual] + vecino.costo;

                if (nueva < distancias[vecino.destino])
                {
                    distancias[vecino.destino] = nueva;
                }
            }
        }

        return distancias;
    }

    // ===================== MAIN =====================
    static void Main()
    {
        int opcion = 0;

        do
        {
            Console.Clear();

            Console.WriteLine("===== SISTEMA DE VUELOS =====");
            Console.WriteLine("1. Ver vuelos disponibles");
            Console.WriteLine("2. Buscar vuelo más barato");
            Console.WriteLine("3. Salir");
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Entrada inválida.");
                Console.ReadKey();
                continue;
            }

            switch (opcion)
            {
                case 1:
                    MostrarVuelos();
                    break;

                case 2:
                    Console.Write("\nIngrese ciudad de origen: ");
                    string origen = Console.ReadLine();

                    Console.Write("Ingrese ciudad destino: ");
                    string destino = Console.ReadLine();

                    if (!grafo.ContainsKey(origen) || !grafo.ContainsKey(destino))
                    {
                        Console.WriteLine("Ciudad no válida.");
                    }
                    else
                    {
                        var distancias = Dijkstra(origen);
                        Console.WriteLine("\nCosto mínimo: $" + distancias[destino]);
                    }
                    break;

                case 3:
                    Console.WriteLine("Saliendo...");
                    break;

                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();

        } while (opcion != 3);
    }
}