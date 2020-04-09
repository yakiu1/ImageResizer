using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Interface
{
    abstract class IImageProcesser
    {
        public string _sourcePath;
        public string _destinationPath;

        /// <summary>
        /// 圖片檔案清空
        /// </summary>
        abstract public void Clean();

        /// <summary>
        /// 找出指定目錄下圖片
        /// </summary>
        abstract public List<string> FindImages();

        /// <summary>
        /// 進行圖片處理
        /// </summary>
        abstract public void ProcessImage(string source, string distPath);

    }
}
