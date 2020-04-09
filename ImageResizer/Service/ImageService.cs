using ImageResizer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Service
{
    class ImageService
    {
        readonly private IImageProcesser _ImageProcesser;

        public ImageService(IImageProcesser imageProcesser)
        {
            this._ImageProcesser = imageProcesser;
        }

        public void DoImageProcess(string sourcePath,string distPath) {
            this._ImageProcesser.ProcessImage(sourcePath,distPath);
        }

        public void Clean()
        {
            this._ImageProcesser.Clean();
        }
    }
}
