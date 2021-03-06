﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogDS.Models
{
    public class ImageUploadValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            //check for actual object
            if (file == null)
                return false;

            //check size
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                { return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                         ImageFormat.Png.Equals(img.RawFormat) ||
                         ImageFormat.Gif.Equals(img.RawFormat);
                }           
            }
        catch
        {
        return false;
        }
        }
    }
}