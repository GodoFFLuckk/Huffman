using System;
using System.Xml.Linq;
using System.IO;
namespace HuffmanDecoder
{
    internal class Program
    {
        static bool CheckFirstBytes(byte[] buffer)
        {
            if (buffer[0] == 0x7B &&
                buffer[1] == 0x68 &&
                buffer[2] == 0x75 &&
                buffer[3] == 0x7C &&
                buffer[4] == 0x6D &&
                buffer[5] == 0x7D &&
                buffer[6] == 0x66 &&
                buffer[7] == 0x66
                )
                return true;

            return false;
        }
        static bool IsZeroEnd(byte[] b)
        {
            for (int i = 0; i < 8; i++)
            {
                if (b[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }
        static void Huffman(string path)
        {
            int[] arrCountOfNumbers = new int[256];
            try
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] buffer = new byte[8];
                    fs.Read(buffer, 0, 8);
                    if (!CheckFirstBytes(buffer))
                    {
                        Console.WriteLine("File Error");
                    }
                    else
                    {
                        HuffmanTree tree = new HuffmanTree(fs, path);
                        fs.Read(buffer, 0, 8);
                        if (!IsZeroEnd(buffer))
                        {
                            Console.WriteLine("File Error");
                        }
                        else
                        {
                            tree.ReadCodedText(fs);
                        }
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
            if (args.Length == 1 && args[0].Substring(args[0].Length - 5, 5) == ".huff" && args[0].Length != 5)
            {
                Huffman(args[0]);
            }
            else Console.WriteLine("Argument Error");
        }
    }
}