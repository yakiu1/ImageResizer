using ImageResizer.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output"); ;

            float oldElapsedMilliseconds = 3240;
            float effective = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            // AsyncImageProcess.DoImageProcessDefault(sourcePath, destinationPath).Wait(); 3132ms
            // AsyncImageProcess.DoImageProcess(sourcePath, destinationPath).Wait(); 2232ms
            AsyncImageProcess.DoImageProcessV2(sourcePath, destinationPath).Wait(); // 2185ms
            sw.Stop();

            effective = (oldElapsedMilliseconds - float.Parse(sw.ElapsedMilliseconds.ToString())) / oldElapsedMilliseconds * 100;

            Console.WriteLine($"花費時間: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"提升:{ effective }" + "%");
            Console.ReadKey();
        }


    }
}
