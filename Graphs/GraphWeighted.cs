using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Schema;
using lab4.Queues;

namespace lab4.Graphs
{
    public class GraphWeighted
    {
        public Dictionary<char, Dictionary<char, int>> AdjacencyList { get; set; }
        public int Count { get; private set; }
        public IGraphPrinter Printer { get; set; }

        public GraphWeighted(Dictionary<char, Dictionary<char, int>> graph)
        {
            AdjacencyList = graph ?? throw new ArgumentException();
            Count = AdjacencyList.Count;
        }

        public GraphWeighted()
        {
            Count = 0;
            AdjacencyList = new Dictionary<char, Dictionary<char, int>>();
        }

        public int Dijkstra(char from, char to)
        {
            var set = Dijkstra(from);

            if (set == null || set.Count == 0)
                return -1;

            set.TryGetValue(new Path(from, to, 0), out var res);

            return res.Value;
        }

        /**
         * Finds all paths from specified vertex
         */
        public HashSet<Path> Dijkstra(char from)
        {
            if (AdjacencyList.Count == 0)
                return null;

            var queue = new UpdatablePriorityQueue<Path>();
            var visited = new HashSet<char>();
            var paths = new HashSet<Path>();

            queue.Enqueue(new Path(from, from, 0));

            while (!queue.IsEmpty())
            {
                var path = queue.Dequeue();

                // get all adjacent vertecies for current vertex
                AdjacencyList.TryGetValue(path.To, out var edges);
                foreach (var edge in edges)
                {
                    if (visited.Contains(edge.Key))
                        continue;

                    var tPath = new Path(from, edge.Key, path.Value + edge.Value);

                    if (!paths.Add(tPath))
                    {
                        // get old value of the path
                        paths.TryGetValue(tPath, out var tPath2);

                        // update priority in queue and 
                        // replace paths if value of the current path is less
                        // than previously found path from vertex `from`
                        // to current vertex
                        if (tPath.Value < tPath2.Value)
                        {
                            queue.Update(tPath, tPath);
                            paths.Remove(tPath2);
                            paths.Add(tPath);
                        }
                    }

                    if (!queue.Contains(tPath))
                        queue.Enqueue(tPath);

                }

                visited.Add(path.To);
            }

            return paths;
        }

        public GraphWeighted Kruskal()
        {
            var edges = new List<Edge>();
            var resultGraph = new GraphWeighted();

            foreach (var v in AdjacencyList)
            {
                foreach (var e in v.Value)
                {
                    var edge = new Edge(v.Key, e.Key, e.Value);
                    if (!edges.Contains(edge))
                        edges.Add(edge);
                }
            }
            
            edges.Sort((left, right) => left.Weight - right.Weight);
            edges.ForEach(edge => resultGraph.AddNoCycle(edge));
            
            return resultGraph;
        }

        
        public Dictionary<char, Dictionary<char, int>> PrimSarAdj()
        {
            var result = new Dictionary<char, Dictionary<char, int>>();
            var list = new SortedList<Edge>(Comparer<Edge>.Create(
                                        (l, r) => l.Weight - r.Weight));
            var included = new HashSet<char>();
            
            if (AdjacencyList.Count == 0)
                return result;

            var k = AdjacencyList.Keys.ElementAt(0);

            if (!AdjacencyList.TryGetValue(k, out var values))
                return result;
            
            foreach (var e in values)
            {
                list.Add(new Edge(k, e.Key, e.Value));
            }
            included.Add(k);
            
            while (list.Count != 0)
            {
                var edge = list.RemoveAt(0);
                if (!included.Contains(edge.V2))
                {
                    if (!result.ContainsKey(edge.V1))
                        result.TryAdd(edge.V1, new Dictionary<char, int>());

                    result[edge.V1].TryAdd(edge.V2, edge.Weight);

                    if (!result.ContainsKey(edge.V2))
                        result.TryAdd(edge.V2, new Dictionary<char, int>());

                    result[edge.V2].TryAdd(edge.V1, edge.Weight);
                }
                
                values = AdjacencyList.GetValueOrDefault(edge.V2);
                foreach (var e in values)
                {
                    var tEdge = new Edge(edge.V2, e.Key, e.Value);
                    if (!included.Contains(edge.V2))
                        list.Add(tEdge);
                }
                included.Add(edge.V2);
            }

            return result;
        }

        public bool AddNoCycle(Edge e)
        {
            return !CheckCycle(e.V1, e.V2, new HashSet<char>()) && Add(e);
        }

        public bool Add(Edge e)
        {
            if (!AdjacencyList.ContainsKey(e.V1))
                AdjacencyList.TryAdd(e.V1, new Dictionary<char, int>());
            if (!AdjacencyList.ContainsKey(e.V2))
                AdjacencyList.TryAdd(e.V2, new Dictionary<char, int>());

            AdjacencyList.TryGetValue(e.V1, out var e1Edges);
            AdjacencyList.TryGetValue(e.V2, out var e2Edges);

            return e1Edges.TryAdd(e.V2, e.Weight) && e2Edges.TryAdd(e.V1, e.Weight);
        }
        
        
        private bool CheckCycle(Edge e)
        {
            return CheckCycle(e.V1, e.V2, new HashSet<char>());
        }
        
        /**
         * Returns true when adding new edge into the graph
         * causes a cycle. Returns false otherwise.
         *
         * The idea is to walk graph using DFS.
         */
        private bool CheckCycle(char source, char dest, ISet<char> visited)
        {
            if (!AdjacencyList.ContainsKey(dest))
                return false;
            
            if (source == dest)
                return true;

            var res = false;

            if (!AdjacencyList.TryGetValue(source, out var adjacentVertices))
                return false;

            if (adjacentVertices.Count == 0)
                return false;

            visited.Add(source);

            foreach (var vertex in adjacentVertices.Keys)
            {
                if (visited.Contains(vertex))
                    continue;

                if (res) return true;

                res = res || CheckCycle(vertex, dest, visited);
            }

            return res;
        }
        

        public void Print()
        {
            Printer.Print(this);
        }
        
        public bool Add(char v1, char v2, int weight)
        {
            return Add(new Edge(v1, v2, weight));
        }

        public bool AddVertex(char v)
        {
            return AdjacencyList.TryAdd(v, new Dictionary<char, int>());
        }

        public bool AddNoCycle(char v1, char v2, int weight)
        {
            return AddNoCycle(new Edge(v1, v2, weight));
        }
        
        public struct Path : IComparable<Path>
        {
            public char From { get; }
            public char To { get; }
            public int Value { get; set; }

            /**
             * Compares to objects by Value
             *
             * To use it in proirity queue consider
             * that object with greater Value
             * has less priority
             */

            public int CompareTo(Path other)
            {
                return other.Value - Value;
            }

            public Path(char from, char to, int value)
            {
                From = from;
                To = to;
                Value = value;
            }

            /**
             * Compares two Path objects by From ant To
             *
             * From - point where to start from
             * To - destination
             *
             * It doesn't take account of Value
             */

            public override bool Equals(object obj)
            {
                if (!(obj is Path path))
                    return false;

                return From == path.From && To == path.To;
            }

            public override int GetHashCode()
            {
                return (From + To).GetHashCode();
            }

            public override string ToString()
            {
                return $"({From} - {To}; {Value})";
            }
        }

        public class Edge : IComparable<Edge>
        {
            public char V1 { get; set; }
            public char V2 { get; set; }
            public int Weight { get; set; }

            public Edge(char v1, char v2, int weight)
            {
                V1 = v1;
                V2 = v2;
                Weight = weight;
            }

            public int CompareTo(Edge other)
            {
                return other.Weight - Weight;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Edge e))
                    return false;

                return ((V1 == e.V1 && V2 == e.V2) || (V1 == e.V2 && V2 == e.V1))
                       && Weight == e.Weight;
            }

            public override string ToString()
            {
                return $"({V1}{V2}; {Weight})";
            }

            public override int GetHashCode()
            {
                return V1 + V2 + Weight;
            }
        }
    }
}