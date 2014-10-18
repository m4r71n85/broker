using ImageResizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Broker.Web.Constants
{
    public static class ImageSizes
    {
        public static ImageSettings Original = new ImageSettings() { Name = "Original", Instructions = new Instructions(){ Mode = FitMode.Max, Width = 200, Height = 200 } };
        public static ImageSettings Big = new ImageSettings() { Name = "Big", Instructions = new Instructions(){ Mode = FitMode.Max, Width = 200, Height = 200 } };
        public static ImageSettings Normal = new ImageSettings() { Name = "Normal", Instructions = new Instructions(){ Mode = FitMode.Max, Width = 400, Height = 250 } };
        public static ImageSettings Small = new ImageSettings() { Name = "Small", Instructions = new Instructions() { Mode = FitMode.Max, Width = 20, Height = 20 } };
        public static ImageSettings Tiny = new ImageSettings() { Name = "Tiny", Instructions = new Instructions() { Mode = FitMode.Max, Width = 50, Height = 50 } };

        public static List<ImageSettings> GetSizes(string imageType)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            return GetSizes(imageType, fileName);
        }
        public static List<ImageSettings> GetSizes(string imageType, string fileName)
        {
            var sizes = new List<ImageSettings>() { Original, Big, Normal, Small };
            foreach (var size in sizes)
            {
                size.Path = GetPath(imageType, fileName, size);
            }
            return sizes;
        }

        public static string GetPath(string imageType, string fileName, ImageSettings size)
        {
            return string.Format("~/Content/Files/Images/{0}/{1}/{2}", imageType, size.Name, fileName);

        }
    }

    public class ImageSettings
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Instructions Instructions { get; set; }
    }

    public class ImageType
    {
        public const string AgencyLogo = "AgencyLogo";
        public const string ProfilePhoto = "Profile";
    }
}