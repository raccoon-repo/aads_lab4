using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using lab4.Graphs;

namespace lab4
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
                        
            var vertices = new Dictionary<char, Dictionary<char, int>>() {
                {'A', new Dictionary<char, int>() {
                    {'E', 5}, {'D', 10}, {'B', 7}
                }},
                
                {'B', new Dictionary<char, int>() {
                    {'A', 7}, {'D', 11}, {'C', 2}
                }},
                
                {'C', new Dictionary<char, int>() {
                    {'B', 2}, {'F', 7}
                }},
                
                {'D', new Dictionary<char, int>() {
                    {'B', 11}, {'A', 10}, {'E', 1}, {'F', 5}
                }},
                
                {'E', new Dictionary<char, int>() {
                    {'D', 1}, {'A', 5}
                }},
                
                {'F', new Dictionary<char, int>() {
                    {'D', 5}, {'C', 7}, {'G', 18}
                }},
                
                {'G', new Dictionary<char, int>() {
                    {'F', 18}, {'H', 10}
                }},
                
                {'H', new Dictionary<char, int>() {
                    {'G', 10}, {'K', 6}
                }},
                
                {'K', new Dictionary<char, int>() {
                    {'H', 6}
                }}
            };
            
            var graph = new GraphWeighted(vertices);
            graph.Printer = new ConsoleGraphPrinter();
            var kruskal = graph.Kruskal();
            kruskal.Printer = graph.Printer;
            kruskal.Print();
            var prim = graph.PrimSarAdj();
            Console.WriteLine();
            
            graph.Printer.Print(new GraphWeighted(prim));
        }
    }
}