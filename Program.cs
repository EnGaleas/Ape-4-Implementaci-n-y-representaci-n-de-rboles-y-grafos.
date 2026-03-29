// ==========================================================
// Sistema de Vuelos Baratos con Grafos
// AUTORES:
// - Erminia Galeas
// - Lady Guerrero
// - Andrea Alejandro
// ==========================================================

using System;
using System.Collections.Generic;

class Program
{
    // ==========================================================
    // BASE DE DATOS FICTICIA
    // Se usa un grafo representado con listas de adyacencia
    // Cada ciudad tiene destinos con un costo asociado
    // ==========================================================
    static Dictionary<string, List<(string destino, int costo)>> grafo =
        new Dictionary<string, List<(string, int)>>()
    {
        { "Quito", new List<(string, int)> { ("Guayaquil", 50), ("Cuenca", 70) } },
        { "Guayaquil", new List<(string, int)> { ("Quito", 50), ("Cuenca", 40), ("Manta", 30) } },
        { "Cuenca", new List<(string, int)> { ("Quito", 70), ("Guayaquil", 40), ("Loja", 60) } },
        { "Manta", new List<(string, int)> { ("Guayaquil", 30) } },
        { "Loja", new List<(string, int)> { ("Cuenca", 60) } }
    };

    // ==========================================================
    // MÉTODO DE REPORTERÍA
    // Permite visualizar todos los vuelos disponibles
    // ==========================================================
    static void MostrarVuelos()
    {
        Console.WriteLine("\n--- LISTA DE VUELOS DISPONIBLES ---");

        // Recorre cada ciudad del grafo
        foreach (var ciudad in grafo)
        {
            Console.WriteLine("\nDesde " + ciudad.Key + ":");

            // Recorre cada conexión (vuelo)
            foreach (var vuelo in ciudad.Value)
            {
                Console.WriteLine(" -> " + vuelo.destino + " ($" + vuelo.costo + ")");
            }
        }
    }

    // ==========================================================
    // ALGORITMO DIJKSTRA
    // Encuentra el costo mínimo desde una ciudad origen
    // ==========================================================
    static Dictionary<string, int> Dijkstra(string inicio)
    {
        var distancias = new Dictionary<string, int>();
        var visitados = new HashSet<string>();

        // Inicializa todas las distancias en infinito
        foreach (var ciudad in grafo.Keys)
            distancias[ciudad] = int.MaxValue;

        // La ciudad inicial tiene costo 0
        distancias[inicio] = 0;

        // Mientras haya nodos sin visitar
        while (visitados.Count < grafo.Count)
        {
            string actual = null;
            int min = int.MaxValue;

            // Busca la ciudad no visitada con menor costo
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

            // Actualiza los costos de las ciudades vecinas
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

    // ==========================================================
    // MÉTODO PRINCIPAL (MENÚ)
    // Permite interactuar con el usuario
    // ==========================================================
    static void Main()
    {
        int opcion = 0;

        do
        {
            Console.Clear();

            // Menú principal
            Console.WriteLine("===== SISTEMA DE VUELOS =====");
            Console.WriteLine("1. Ver vuelos disponibles");
            Console.WriteLine("2. Buscar vuelo más barato");
            Console.WriteLine("3. Salir");
            Console.Write("Seleccione una opción: ");

            // Validación de entrada
            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Entrada inválida.");
                Console.ReadKey();
                continue;
            }

            switch (opcion)
            {
                case 1:
                    // Llamada a reportería
                    MostrarVuelos();
                    break;

                case 2:
                    Console.Write("\nIngrese ciudad de origen: ");
                    string origen = Console.ReadLine();

                    Console.Write("Ingrese ciudad destino: ");
                    string destino = Console.ReadLine();

                    // Validación de ciudades
                    if (!grafo.ContainsKey(origen) || !grafo.ContainsKey(destino))
                    {
                        Console.WriteLine("Ciudad no válida.");
                    }
                    else
                    {
                        // Ejecuta algoritmo de búsqueda
                        var distancias = Dijkstra(origen);

                        // Muestra resultado final
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