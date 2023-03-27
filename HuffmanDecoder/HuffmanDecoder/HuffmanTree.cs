using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace HuffmanDecoder
{
    internal class HuffmanTree
    {
        public Node root;
        private string path;
        public ulong count { get; set; }
        public HuffmanTree(FileStream fs, string path)
        {
            this.path = path;
            count = 0;
            root = BuildHuffmanTree(fs);
        }
        private bool IsList(byte b)
        {
            int firstBit = b % 2;
            if (firstBit == 0) return false;

            return true;
        }
        private void ReadChar(ref int buffer, Node node, FileStream fs, ref int readedBitsInByte, FileStream fw)
        {
            if (node.GetType() == typeof(InnerNode))
            {
                if (readedBitsInByte == 8)
                {
                    readedBitsInByte = 0;
                    buffer = fs.ReadByte();
                }
                InnerNode n1 = (InnerNode)node;
                if (buffer % 2 == 0)
                {
                    readedBitsInByte++;
                    buffer = buffer >> 1;
                    ReadChar(ref buffer, n1.leftSon, fs, ref readedBitsInByte, fw);
                }
                else
                {
                    readedBitsInByte++;
                    buffer = buffer >> 1;
                    ReadChar(ref buffer, n1.rightSon, fs, ref readedBitsInByte, fw);
                }
            }
            else
            {
                count--;
                NodeLeaf n1 = (NodeLeaf)node;
                fw.WriteByte((byte)n1.symbCode);
                //Console.Write((char)n1.symbCode);
            }
        }
        public void ReadCodedText(FileStream fs)
        {
            int readedBitsInByte = 0;
            int buffer = 0;
            ulong charCount = count;
            int curByte;
            buffer = fs.ReadByte();
            InnerNode start = (InnerNode)root;
            using (FileStream fw = File.Create(path.Substring(0, path.Length - 5)))
            {
                while (count > 0)
                {
                    ReadChar(ref buffer, start, fs, ref readedBitsInByte, fw);
                }
            }
        }
        public Node BuildHuffmanTree(FileStream fs)
        {
            byte[] buffer = new byte[8];
            fs.Read(buffer, 0, 7);
            int lastByte = fs.ReadByte();
            if (IsList(buffer[0]))
            {
                count += (BitConverter.ToUInt64(buffer)) >> 1;
                return new NodeLeaf(lastByte, BitConverter.ToUInt64(buffer));
            }
            else
            {
                InnerNode node = new InnerNode();
                node.leftSon = BuildHuffmanTree(fs);
                node.rightSon = BuildHuffmanTree(fs);
                return node;
            }
        }
    }
}
