using System;
using System.Drawing;
using System.Text;

class Steganography
{
    static void Main()
    {
        Console.WriteLine("1. Encode text into image");
        Console.WriteLine("2. Decode text from image");
        Console.Write("Choose option: ");
        string option = Console.ReadLine();

        if (option == "1")
        {
            Console.Write("Enter path to input image: ");
            string inputPath = Console.ReadLine();
            Console.Write("Enter message to hide: ");
            string message = Console.ReadLine();
            Console.Write("Enter path to save encoded image: ");
            string outputPath = Console.ReadLine();

            EncodeMessage(inputPath, message, outputPath);
            Console.WriteLine("Message encoded successfully.");
        }
        else if (option == "2")
        {
            Console.Write("Enter path to encoded image: ");
            string encodedPath = Console.ReadLine();

            string decoded = DecodeMessage(encodedPath);
            Console.WriteLine("Decoded message: " + decoded);
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }

    static void EncodeMessage(string imagePath, string message, string outputPath)
    {
        Bitmap bmp = new Bitmap(imagePath);
        message += '\0'; // نهاية الرسالة

        int charIndex = 0, charValue = 0, pixelElementIndex = 0;
        bool messageComplete = false;

        for (int i = 0; i < bmp.Height; i++)
        {
            for (int j = 0; j < bmp.Width; j++)
            {
                Color pixel = bmp.GetPixel(j, i);
                int R = pixel.R - pixel.R % 2;
                int G = pixel.G - pixel.G % 2;
                int B = pixel.B - pixel.B % 2;

                for (int n = 0; n < 3; n++)
                {
                    if (charIndex < message.Length)
                    {
                        charValue = message[charIndex];
                        int bit = (charValue >> (7 - pixelElementIndex)) & 1;

                        if (n == 0) R += bit;
                        else if (n == 1) G += bit;
                        else B += bit;

                        pixelElementIndex++;
                        if (pixelElementIndex == 8)
                        {
                            charIndex++;
                            pixelElementIndex = 0;
                        }
                    }
                    else
                    {
                        messageComplete = true;
                        break;
                    }
                }

                bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                if (messageComplete) break;
            }
            if (messageComplete) break;
        }

        bmp.Save(outputPath);
    }

    static string DecodeMessage(string imagePath)
    {
        Bitmap bmp = new Bitmap(imagePath);
        int colorUnitIndex = 0;
        int charValue = 0;
        string message = "";

        for (int i = 0; i < bmp.Height; i++)
        {
            for (int j = 0; j < bmp.Width; j++)
            {
                Color pixel = bmp.GetPixel(j, i);
                for (int n = 0; n < 3; n++)
                {
                    int val = n == 0 ? pixel.R : n == 1 ? pixel.G : pixel.B;
                    charValue = (charValue << 1) | (val & 1);
                    colorUnitIndex++;

                    if (colorUnitIndex == 8)
                    {
                        char c = (char)charValue;
                        if (c == '\0') return message;
                        message += c;
                        colorUnitIndex = 0;
                        charValue = 0;
                    }
                }
            }
        }
        return message;
    }
}
