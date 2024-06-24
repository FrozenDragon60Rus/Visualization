using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Visualization.Graph
{
    internal class Route
    {
        Node startNode, endNode, currentNode;
        private List<Vector2> nodePosition;
        ExcelReader excelReader;

        private Vector2 GetDirrection()
        {
            Vector2 dir = endNode.position - startNode.position;
            int x = dir.x < 0 ? -1 : 1,
                y = dir.y < 0 ? -1 : 1;
            return new Vector2(x, y);
        }
        public void BuildRoute()
        {
            //Console.WriteLine("startPosition " + nodePosition.IndexOf(new Vector2(7, 3)));
            nodePosition.Remove(startNode.position);
            currentNode = startNode;
            while (true)
            {
                GetAllChildren();
                currentNode = startNode.Nearest;
                Console.WriteLine("position (" + currentNode.position.x + "," + currentNode.position.y + ") " + currentNode.FullPath);
                if (currentNode.position == endNode.position) break;
                //Console.ReadKey();
            }
        }
        public Route()
        {
            excelReader = new ExcelReader();
            Console.WriteLine("Start");
            Vector2 startPoint, endPoint;
            excelReader.FindKeyPoint(out startPoint, out endPoint);
            Console.WriteLine("Find point");
            
            startNode = new Node(startPoint, 0);
            endNode = new Node(endPoint, 0);
            FillNodePositionList();
            Console.WriteLine("Fill");
            BuildRoute();
            Console.WriteLine("Build");
            excelReader.Clear();
            excelReader.WriteRoute(currentNode.FromEndToStart(), startPoint, endPoint);
            WritePath(startNode);
            Console.WriteLine("Draw");
            excelReader.Close();
            Console.WriteLine("End");
        }
        private void FillNodePositionList()
        {
            nodePosition = new List<Vector2>();
            for (int x = 1; x <= excelReader.size.x; x++)
                for (int y = 1; y <= excelReader.size.y; y++)
                    if (Convert.ToSingle(excelReader.cells[x, y].Value2) > 0)
                        nodePosition.Add(new Vector2(x, y));
            Console.WriteLine("nodes: " + nodePosition.Count);
            Console.WriteLine("Columns: " + excelReader.size.x + " Rows:" + excelReader.size.y);
        }
        private void GetAllChildren()
        {
            float path;
            foreach (Vector2 pos in nodePosition)
            {
                path = currentNode.Distance(pos);
                if (path < 1.5f)
                    currentNode.AddChildren(pos,
                                            Convert.ToSingle(excelReader.cells[pos.x, pos.y].Value2) * path);
                //Console.WriteLine("(" + pos.x + " " + pos.y + ") | (" + currentNode.position.x + " " + currentNode.position.y + ") - " + currentNode.Distance(pos));
            }
                
            if (currentNode.children.Count > 0)
                foreach (Node node in currentNode.children)
                    nodePosition.Remove(node.position);
            else
                currentNode.parent.children.Remove(currentNode);
            //Console.WriteLine(nodePosition.Count + " " + currentNode.children.Count);
        }
        private void WritePath(Node node)
        {
            foreach (Node child in node.children)
            {
                excelReader.Write(child.position, child.FullPath);
                WritePath(child);
            }
        }
    }
}
