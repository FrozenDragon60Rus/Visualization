using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Visualization.Graph
{
    internal class Node
    {
        public readonly Vector2 position;
        public float path;
        public Node parent;
        public List<Node> children;

        public Node(Vector2 position, float path)
        {
            this.position = position;
            this.path = path;
            children = new List<Node>();
        }
        private Node Root =>
            parent == null ? this
                           : parent.Root;
        public Node Last =>
            children.Count > 0 ? children[0]
                               : this;
        public float FullPath =>
            GetFullPath(path);
        public Node Nearest
        {
            /*get 
            {
                if (children.Count == 0) return this;
                List<Node> node = new List<Node>();
                foreach (Node child in children)
                    node.Add(child.Nearest);

                Node nearest = node[0];

                foreach (Node n in node)
                    if (nearest.FullPath > n.FullPath)
                        nearest = n;
                return nearest; 
            }*/
            get
            {
                if (children.Count == 0) return this;
                return children.Select(n => n.Nearest)
                               .Aggregate((n1, n2) =>
                                   n1.FullPath < n2.FullPath ? n1 : n2);
            }
        }
        
        public int StepCount
        {
            get
            {
                Node node = this;
                int step = -1;
                while (node != null)
                {
                    node = node.parent;
                    step++;
                }
                return step;
            }
        }
        public void AddChildren(Vector2 position, float path)
        {
            Node node = new Node(position, path);
            node.parent = this;
            children.Add(node);
        }
        public void AddChildren(Node node)
        {
            node.parent = this;
            children.Add(node);
        }
        
        public static float Distance(Vector2 pos1, Vector2 pos2) =>
            (float)Math.Sqrt(Math.Pow(pos2.x - pos1.x, 2) +
                             Math.Pow(pos2.y - pos1.y, 2));
        public float Distance(Vector2 pos) =>
            (float)Math.Sqrt(Math.Pow(pos.x - position.x, 2) +
                             Math.Pow(pos.y - position.y, 2));
        private float GetFullPath(float path) =>
            parent == null ? path
                           : parent.GetFullPath(path + this.path);
        public Vector2[] FromEndToStart()
        {
            Node currentNode = this;
            List<Vector2> nodes = new List<Vector2>();
            int i = 1;
            while (true)
            {
                if (currentNode.parent == null) break;
                Console.WriteLine(i++ + ") " + currentNode.parent.position);    
                nodes.Add(currentNode.position);
                currentNode = currentNode.parent;
            }   
            return nodes.ToArray();
        }
    }
}
