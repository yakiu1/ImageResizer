﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Common
{
    class AsyncImageProcess
    {
        /// <summary>
        /// 非同步執行影像縮略
        /// </summary>
        /// <param name="sourcePath">來源路徑</param>
        /// <param name="destinationPath">目標路徑</param>
        /// <returns></returns>
        public static async Task DoImageProcess(string sourcePath, string destinationPath)
        {
            ImageProcess imageProcess = new ImageProcess();
            imageProcess.Clean(destinationPath);
            await Task.WhenAll(imageProcess.ResizeImages(sourcePath, destinationPath, 2.0));
        }

        /// <summary>
        /// 非同步執行影像縮略2
        /// </summary>
        /// <param name="sourcePath">來源路徑</param>
        /// <param name="destinationPath">目標路徑</param>
        public static async Task DoImageProcessV2(string sourcePath, string destinationPath)
        {
            ImageProcess imageProcess = new ImageProcess();
            imageProcess.Clean(destinationPath);
            await imageProcess.ResizeImages2(sourcePath, destinationPath, 2.0);
        }

        /// <summary>
        /// 無法控制別人的方法有沒有非同步的範例
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        public static async Task DoImageProcessDefault(string sourcePath, string destinationPath)
        {
            ImageProcess imageProcess = new ImageProcess();
            imageProcess.Clean(destinationPath);

            await Task.Run(() =>
             {
                 imageProcess.ResizeImagesDefault(sourcePath, destinationPath, 2.0);
             });
        }
    }
}