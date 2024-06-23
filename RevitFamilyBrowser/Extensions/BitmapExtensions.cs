using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;
using System;

namespace zRevitFamilyBrowser.Extensions
{
    public static class BitmapExtensions
    {
        public static BitmapSource GetImage(this Bitmap bitmap) // что значит this в этом контексте?
        {
            BitmapSource bmSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return bmSource;
        }
    }
}