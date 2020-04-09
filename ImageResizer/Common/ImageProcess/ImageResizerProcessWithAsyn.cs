using ImageResizer.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageResizer.Common.ImageProcess
{
    class ImageResizerProcessWithAsyn : IImageProcesser
    {
        public override void Clean()
        {
            if (!Directory.Exists(_destinationPath))
            {
                Directory.CreateDirectory(_destinationPath);
            }
            else
            {
                var allImageFiles = Directory.GetFiles(_destinationPath, "*", SearchOption.AllDirectories);

                foreach (var item in allImageFiles)
                {
                    File.Delete(item);
                }
            }
        }

        public override List<string> FindImages()
        {
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(_sourcePath, "*.png", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(_sourcePath, "*.jpg", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(_sourcePath, "*.jpeg", SearchOption.AllDirectories));
            return files;
        }

        public override void ProcessImage(string source, string distPath)
        {
            _sourcePath = source;
            _destinationPath = distPath;
            ResizeImages2().Wait();
            Clean();
        }

        /// <summary>
        /// 進行圖片的縮放作業，回傳Task
        /// </summary>
        /// <param name="sourcePath">圖片來源目錄路徑</param>
        /// <param name="destPath">產生圖片目的目錄路徑</param>
        /// <param name="scale">縮放比例</param>
        public Task ResizeImages2(CancellationToken token)
        {
            var allFiles = FindImages();
            List<Task> result = new List<Task>();
            foreach (var filePath in allFiles)
            {
                result.Add(Task.Run(() =>
                {
                    if (token.IsCancellationRequested == true)
                    {
                        Console.WriteLine(filePath + "作業中斷");
                        return;
                    }
                    Console.WriteLine("【" + String.Format("{0:D2}", Thread.CurrentThread.ManagedThreadId) + "】" + "開始:" + filePath);
                    Image imgPhoto = Image.FromFile(filePath);
                    string imgName = Path.GetFileNameWithoutExtension(filePath);

                    int sourceWidth = imgPhoto.Width;
                    int sourceHeight = imgPhoto.Height;

                    int destionatonWidth = (int)(sourceWidth * 2.0);
                    int destionatonHeight = (int)(sourceHeight * 2.0);

                    Bitmap processedImage = processBitmap((Bitmap)imgPhoto,
                        sourceWidth, sourceHeight,
                        destionatonWidth, destionatonHeight);

                    string destFile = Path.Combine(_destinationPath, imgName + ".jpg");
                    processedImage.Save(destFile, ImageFormat.Jpeg);
                    Console.WriteLine("【" + String.Format("{0:D2}", Thread.CurrentThread.ManagedThreadId) + "】" + "結束:" + filePath);
                }, token));

            }
            return Task.WhenAll(result);
        }

        /// <summary>
        /// 進行圖片的縮放作業，回傳Task
        /// </summary>
        /// <param name="sourcePath">圖片來源目錄路徑</param>
        /// <param name="destPath">產生圖片目的目錄路徑</param>
        /// <param name="scale">縮放比例</param>
        public Task ResizeImages2()
        {
            return ResizeImages2(CancellationToken.None);
        }

        Bitmap processBitmap(Bitmap img, int srcWidth, int srcHeight, int newWidth, int newHeight)
        {
            Bitmap resizedbitmap = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(resizedbitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(img,
                new Rectangle(0, 0, newWidth, newHeight),
                new Rectangle(0, 0, srcWidth, srcHeight),
                GraphicsUnit.Pixel);
            return resizedbitmap;
        }
    }
}
