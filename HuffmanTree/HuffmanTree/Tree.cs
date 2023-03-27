using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree
{
    internal class Tree
    {
        private List<Node> nodes = new List<Node>();
        private Node root = null;
        private int miniTreesCount = 0;
        private int nodesCount = 0;
        public void append(Node node)
        {
            miniTreesCount++;
            nodes.Add(node);
        }
        int epochs = 0;
        public void TransformToHuffman()
        {
            nodesCount = miniTreesCount * 2 - 1;
            nodes.Sort();
            while (miniTreesCount > 1)
            {
                epochs++;
                Node newNode = new Node(nodes[0], nodes[1], nodes[0].getWeight() + nodes[1].getWeight(), 10000000, epochs);
                nodes.Add(newNode);
                nodes.RemoveAt(0);
                nodes.RemoveAt(0);
                nodes.Sort();
                miniTreesCount--;
            }
            root = nodes[0];
        }
        public Node GetRoot()
        {
            return root;
        }
        public void PrintTreeInPreorder()
        {
            int nodesCountForPrint = this.nodesCount;
            if (root != null)
            {
                PrintTreeInPreorder(root, nodesCountForPrint);
            }
        }
        public void PrintTreeInPreorder(Node curNode, int nodesCountForPrint)
        {
            nodesCountForPrint--;
            if (curNode.getEra() > 0)
            {
                Console.Write(curNode.getWeight());
                if (nodesCount != 0)
                {
                    Console.Write(' ');
                }
            }
            else
            {
                Console.Write('*');
                Console.Write(curNode.getCodeNumber());
                Console.Write(':');
                Console.Write(curNode.getWeight());
                if (nodesCount != 0)
                {
                    Console.Write(' ');
                }
            }
            if (curNode.getLeftSon() != null)
            {
                PrintTreeInPreorder(curNode.getLeftSon(), nodesCountForPrint);
            }
            if (curNode.getRightSon() != null)
            {
                PrintTreeInPreorder(curNode.getRightSon(), nodesCountForPrint);
            }
        }
    }
}
