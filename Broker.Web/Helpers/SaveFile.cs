using Broker.Web.Constants;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Broker.Web.Helpers
{
    public static class SaveImage
    {
        public static string Save(HttpPostedFileBase file, string imageType)
        {
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                var imageSettings = ImageSizes.GetSizes(ImageType.AgencyLogo);
                foreach (var imageSetting in imageSettings)
                {
                    var job = new ImageJob(file, imageSetting.Path, new Instructions(imageSetting.Instructions));
                    job.CreateParentDirectory = true;
                    job.AddFileExtension = true;
                    job.Build();
                    fileName = Path.GetFileName(job.FinalPath);
                }
            }
            return fileName;
        }
    }
}