using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuffmanDecoder
{
    public class Node
    {

    }
    public class NodeLeaf : Node
    {
        public ulong weight { get; set; }
        public int symbCode { get; set; }
        public NodeLeaf(int s, ulong w)
        {
            weight = w;
            symbCode = s;
        }
    }
    public class InnerNode : Node
    {
        public Node leftSon { get; set; }
        public Node rightSon { get; set; }
    }
}
