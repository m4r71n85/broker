using Broker.Web.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Broker.Web.Helpers
{
    public static class MvcHtmlHelpers
    {
        public static string Image(this HtmlHelper htmlHelper,
            string source, string type, ImageSettings size)
        {
            var builder = new TagBuilder("image");
            var absolutePath = VirtualPathUtility.ToAbsolute(ImageSizes.GetPath(type, source, size));
            return absolutePath;
        }

        public static string GoogleStaticImage(this HtmlHelper htmlHelper, string address)
        {
            return @"https://maps.googleapis.com/maps/api/staticmap?zoom=17&size=400x400&maptype=roadmap\&markers=size:mid%7Ccolor:red%7C" + address;
        }

        public static string GoogleQrImage(this HtmlHelper htmlHelper, string address)
        {
            return @"http://chart.googleapis.com/chart?cht=qr&chld=L|1&chs=130x130&chl=" + address;
        }
    }
}