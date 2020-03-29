using ImageResizer.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ImageProcess imgProcess = new ImageProcess();

            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output"); ;

            float oldElapsedMilliseconds = 3240;
            float effective = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            // AsyncImageProcess.DoImageProcessDefault(sourcePath, destinationPath).Wait(); 3132ms
            // AsyncImageProcess.DoImageProcess(sourcePath, destinationPath).Wait(); 2232ms
            
            try {
                await AsyncImageProcess.DoImageProcessV2Async(sourcePath, destinationPath);
            }
            catch (OperationCanceledException)
            {
                imgProcess.Clean(destinationPath);
                Console.WriteLine($"{Environment.NewLine}下載已經取消");
            }
            catch (Exception ex)
            {
                imgProcess.Clean(destinationPath);
                Console.WriteLine($"{Environment.NewLine}發現例外異常 {ex.Message}");
            }

            sw.Stop();

            effective = (oldElapsedMilliseconds - float.Parse(sw.ElapsedMilliseconds.ToString())) / oldElapsedMilliseconds * 100;

            Console.WriteLine($"花費時間: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"提升:{ effective }" + "%");
            Console.ReadKey();
        }


    }
}
