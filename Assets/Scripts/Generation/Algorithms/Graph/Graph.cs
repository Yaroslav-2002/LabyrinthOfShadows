using System.Collections.Generic;

namespace Graph
{
    public class Graph<T>
    {
        private Dictionary<T, HashSet<T>> _adjacencyList = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex)
        {
            _adjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(T vertex1, T vertex2)
        {
            if (!_adjacencyList.ContainsKey(vertex1))
                AddVertex(vertex1);

            if (!_adjacencyList.ContainsKey(vertex2))
                AddVertex(vertex2);

            _adjacencyList[vertex1].Add(vertex2);
            _adjacencyList[vertex2].Add(vertex1);
        }

        public bool HasEdge(T vertex1, T vertex2)
        {
            return _adjacencyList.ContainsKey(vertex1) && _adjacencyList[vertex1].Contains(vertex2);
        }
    }
}