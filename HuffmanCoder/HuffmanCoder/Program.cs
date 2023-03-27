using System.IO;
using System.Text;
using System.Collections;
using System;
using System.Collections.Generic;

namespace HuffmanCoder
{
    internal class Program
    {
        static void Huffman(string path)
        {
            int[] arrCountOfNumbers = new int[256];
            try
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    while (true)
                    {
                        int symbCode = fs.ReadByte();
                        if (symbCode < 65535 && symbCode > -1)
                        {
                            arrCountOfNumbers[symbCode]++;
                        }
                        if (symbCode == -1)
                        {
                            break;
                        }
                    }
                    int miniTreesCount = 0;
                    Tree huffmanTree = new Tree();
                    for (int i = 0; i < 256; i++)
                    {
                        if (arrCountOfNumbers[i] > 0)
                        {
                            miniTreesCount++;
                            huffmanTree.append(new Node(null, null, arrCountOfNumbers[i], i, 0));
                        }
                    }
                    if (miniTreesCount > 0)
                    {
                        huffmanTree.TransformToHuffman();
                        Node root = huffmanTree.GetRoot();
                        huffmanTree.PrintHuffmanCode(root, path);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.Write("File Error");
            }
            catch (UnauthorizedAccessException)
            {
                Console.Write("File Error");
            }
        }
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Write("Argument Error");
            }
            else
            {
                Huffman(args[0]);
            }
        }
    }
}