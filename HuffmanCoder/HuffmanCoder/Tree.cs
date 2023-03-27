using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace HuffmanCoder
{
    internal class Tree
    {
        private List<Node> nodes = new List<Node>();
        private Node root = null;
        private int miniTreesCount = 0;
        private int nodesCount = 0;
        private List<bool>[] codingTable = new List<bool>[256];
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
        private void WriteByteTree(FileStream fs)
        {
            int nodesCountForPrint = this.nodesCount;
            if (root != null)
            {
                int code = 0;
                List<bool> codes = new List<bool>();
                bool bit = false;
                WriteByteTree(root, nodesCountForPrint, fs, code, 0, codes, bit);
            }
        }
        private void WriteByteTree(Node curNode, int nodesCountForPrint, FileStream fs, int code, int bitCount, List<bool> codes, bool bit)
        {
            if (curNode != root)
            {
                codes.Add(bit);
            }
            nodesCountForPrint--;
            if (curNode.getEra() > 0)
            {
                long weight = curNode.getWeight();
                weight = weight << 1;
                byte[] nodeBinary = BitConverter.GetBytes(weight);
                fs.Write(nodeBinary);
            }
            else
            {
                long weight = curNode.getWeight();
                weight = weight << 1;
                weight = weight + 1;
                byte[] nodeBinary = BitConverter.GetBytes(weight);
                nodeBinary[7] = Convert.ToByte(curNode.getCodeNumber());
                codingTable[curNode.getCodeNumber()] = codes;
                fs.Write(nodeBinary);
            }
            if (curNode.getLeftSon() != null)
            {
                WriteByteTree(curNode.getLeftSon(), nodesCountForPrint, fs, 0, bitCount + 1, new List<bool>(codes), false);
            }
            if (curNode.getRightSon() != null)
            {
                WriteByteTree(curNode.getRightSon(), nodesCountForPrint, fs, 0, bitCount + 1, new List<bool>(codes), true);
            }
        }
        private void WriteHeader(FileStream fs)
        {
            byte[] header = { 0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66 };
            fs.Write(header);
        }
        private void WriteZerosArterTree(FileStream fs)
        {
            byte[] header = { 0, 0, 0, 0, 0, 0, 0, 0 };
            fs.Write(header);
        }
        private int Transform(List<bool> bitsBuffer)
        {
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                if (bitsBuffer[i] == true)
                {
                    sum += Convert.ToInt32(Math.Pow(2, i));
                }
            }
            return sum;
        }
        private Int32 reverse(Int32 currentByte)
        {
            currentByte =
            ((currentByte & 0x01) << 7) |
            ((currentByte & 0x02) << 5) |
            ((currentByte & 0x04) << 3) |
            ((currentByte & 0x08) << 1) |
            ((currentByte & 0x10) >> 1) |
            ((currentByte & 0x20) >> 3) |
            ((currentByte & 0x40) >> 5) |
            ((currentByte & 0x80) >> 7);
            return currentByte;
        }
        private void OverflowBuffer(ref Int32 bitsBuffer, FileStream fw, ref int bitscount, int currentSymbolBitsCount)
        {
            int shiftForWrite = bitscount - 8;
            Int32 tempByte = bitsBuffer >> shiftForWrite;
            tempByte = reverse(tempByte);
            byte byteToFile = Convert.ToByte(tempByte);
            fw.WriteByte(byteToFile);
            Int32 mask = 0;
            for (int i = 0; i < shiftForWrite; i++)
            {
                mask = mask << 1;
                mask += 1;
            }
            bitsBuffer = bitsBuffer & mask;
            bitscount -= 8;
            if (bitscount > 8)
            {
                OverflowBuffer(ref bitsBuffer, fw, ref bitscount, bitscount);
            }
        }
        private Int32 FromListToCode(List<bool> codeList)
        {
            Int32 result = 0;
            for (int i = 0; i < codeList.Count(); i++)
            {
                result = result << 1;
                if (codeList[i] == true)
                {
                    result += 1;
                }
            }
            return result;
        }
        private void WriteHuffmanCodedText(FileStream fw, string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] buffer = new byte[4096];
                int count = 0;
                byte symbCode = 0;
                byte byteToFile = 0;
                Int32 bitsBuffer = 0;
                int bitscount = 0;
                int counter = 0;
                int currentSymbolBitsCount = 0;
                while ((count = fs.Read(buffer, 0, 4096)) != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        counter++;
                        symbCode = buffer[i];
                        bitsBuffer = bitsBuffer << codingTable[symbCode].Count();
                        bitsBuffer = bitsBuffer | FromListToCode(codingTable[symbCode]);
                        codingTable[symbCode].Count();
                        currentSymbolBitsCount = codingTable[symbCode].Count();
                        bitscount += currentSymbolBitsCount;
                        if (bitscount == 8)
                        {
                            byteToFile = Convert.ToByte(reverse(bitsBuffer));
                            fw.WriteByte(byteToFile);
                            bitscount = 0;
                            bitsBuffer = 0;
                        }
                        if (bitscount > 8)
                        {
                            OverflowBuffer(ref bitsBuffer, fw, ref bitscount, currentSymbolBitsCount);
                        }
                        if (counter == count && count != 4096)
                        {
                            if (bitscount != 0)
                            {
                                bitsBuffer = bitsBuffer << (8 - bitscount);
                                byteToFile = Convert.ToByte(reverse(bitsBuffer));
                                fw.WriteByte(byteToFile);
                            }
                            break;
                        }
                    }
                    counter = 0;
                }
            }
        }
        public void PrintHuffmanCode(Node curNode, string path)
        {
            string pathToWrite = path + ".huff";
            if (File.Exists(pathToWrite))
            {
                File.Delete(pathToWrite);
            }
            using (FileStream fs = File.Create(pathToWrite))
            {
                WriteHeader(fs);
                WriteByteTree(fs);
                WriteZerosArterTree(fs);
                WriteHuffmanCodedText(fs, path);
            }
        }

    }
}
