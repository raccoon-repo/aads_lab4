using System;

namespace lab4.Graphs
{
    public class ConsoleGraphPrinter : IGraphPrinter
    {
        public void Print(GraphWeighted graph)
        {
            var adjList = graph.AdjacencyList;

            foreach (var v1 in adjList)
            {
                var counter = 0;
                Console.Write(v1.Key + ": ");
                foreach (var v2 in v1.Value)
                {
                    var pair = "[" + v2.Key + ";" + v2.Value + "]";
                    
                    if (counter < v1.Value.Count - 1)
                        Console.Write(pair + "->");
                    else
                        Console.Write(pair);
                    counter++;
                }
                Console.WriteLine();
            }
        }
    }
}