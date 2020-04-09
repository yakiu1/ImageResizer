using ImageResizer.Common;
using ImageResizer.Common.ImageProcess;
using ImageResizer.Interface;
using ImageResizer.Service;
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
            // ImageService imageService = new ImageService(new ImageResizerProcess());
            ImageService imageService = new ImageService(new ImageResizerProcessWithAsyn());

            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output"); ;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                imageService.DoImageProcess(sourcePath, destinationPath);
            }
            catch (OperationCanceledException)
            {
                imageService.Clean();
                Console.WriteLine($"{Environment.NewLine}下載已經取消");
            }
            catch (Exception ex)
            {
                imageService.Clean();
                Console.WriteLine($"{Environment.NewLine}發現例外異常 {ex.Message}");
            }

            sw.Stop();
            Console.WriteLine($"花費時間: {sw.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }


    }
}
