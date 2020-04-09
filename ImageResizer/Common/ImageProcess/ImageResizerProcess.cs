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
    // IDE會幫我實作應具備的抽象類別
    // 根據 ISP 原則，抽象類別我只定義該功能必須的兩個抽象類別
    class ImageResizerProcess : IImageProcesser
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

        /// <summary>
        /// 針對指定圖片進行縮放作業
        /// </summary>
        /// <param name="img">圖片來源</param>
        /// <param name="srcWidth">原始寬度</param>
        /// <param name="srcHeight">原始高度</param>
        /// <param name="newWidth">新圖片的寬度</param>
        /// <param name="newHeight">新圖片的高度</param>
        /// <returns></returns>
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

        /// <summary>
        /// 進行圖片的縮放作業
        /// </summary>
        /// <param name="scale">縮放比例</param>
        public void ResizeImagesDefault(double scale)
        {
            var allFiles = FindImages();
            foreach (var filePath in allFiles)
            {
                Console.WriteLine(String.Format("{0:D2}", Thread.CurrentThread.ManagedThreadId) + "開始:" + filePath);
                Image imgPhoto = Image.FromFile(filePath);
                string imgName = Path.GetFileNameWithoutExtension(filePath);

                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;

                int destionatonWidth = (int)(sourceWidth * scale);
                int destionatonHeight = (int)(sourceHeight * scale);

                Bitmap processedImage = processBitmap((Bitmap)imgPhoto,
                    sourceWidth, sourceHeight,
                    destionatonWidth, destionatonHeight);

                string destFile = Path.Combine(_destinationPath, imgName + ".jpg");
                processedImage.Save(destFile, ImageFormat.Jpeg);
                Console.WriteLine(String.Format("{0:D2}", Thread.CurrentThread.ManagedThreadId) + "結束:" + filePath);
            }
        }

        /// <summary>
        /// 進行圖片處理
        /// </summary>
        /// <param name="source">來源路徑</param>
        /// <param name="distPath">實際路徑</param>
        public override void ProcessImage(string source, string distPath)
        {
            _sourcePath = source;
            _destinationPath = distPath;
            ResizeImagesDefault(2.0);
            Clean();
        }
    }
}
