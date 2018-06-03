using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Schema;
using lab4.Queues;

namespace lab4.Graphs
{
    public class GraphWeighted
    {
        private Dictionary<char, Dictionary<char, int>> _graph;

        public int Count { get; private set; }

        public GraphWeighted(Dictionary<char, Dictionary<char, int>> graph)
        {
            _graph = graph ?? throw new ArgumentException();
            Count = _graph.Count;
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
            if (_graph.Count == 0)
                return null;
            
            var queue = new UpdatablePriorityQueue<Path>();
            var visited = new HashSet<char>();
            var paths = new HashSet<Path>();

            queue.Enqueue(new Path(from, from, 0));
            
            while (!queue.IsEmpty())
            {
                var path = queue.Dequeue();
                
                // get all adjacent vertecies for current vertex
                _graph.TryGetValue(path.To, out var edges);
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
    }
}