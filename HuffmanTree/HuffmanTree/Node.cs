using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree
{
    internal class Node : IComparable<Node>
    {
        private int weight;
        private Node leftSon;
        private Node rightSon;
        private int codeNumber;
        private int era;
        public Node(Node leftSon, Node rightSon, int weight, int codeNumber, int era)
        {
            this.leftSon = leftSon;
            this.rightSon = rightSon;
            this.weight = weight;
            this.codeNumber = codeNumber;
            this.era = era;
        }
        public int getWeight()
        {
            return weight;
        }
        public Node getLeftSon()
        {
            return leftSon;
        }
        public Node getRightSon()
        {
            return rightSon;
        }
        public int getEra()
        {
            return era;
        }
        public int getCodeNumber()
        {
            return codeNumber;
        }
        public int CompareTo(Node node)
        {
            if (this.weight > node.weight)
            {
                return 1;
            }
            else if (this.weight < node.weight)
            {
                return -1;
            }
            else
            {
                if (codeNumber < 256)
                {
                    if (this.codeNumber > node.codeNumber)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (this.era > node.era)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

    }
}
