using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace zRevitFamilyBrowser.Extensions
{
    /// <summary>
    ///     Класс для методов расширений для класса Bitmap
    /// </summary>
    /// <remarks>
    ///     почитать про методы расширения можно здесь
    ///     https://learn.microsoft.com/ru-ru/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
    /// </remarks>
    public static class BitmapExtensions
    {
        /// <summary>
        ///     Получение BitmapSource из Bitmap
        /// </summary>
        /// <param name="bitmap">изображение</param>
        /// <returns>источник изображения, который можно использовать на UI</returns>
        public static BitmapSource GetImage(this Bitmap bitmap)
        {
            var bmSource = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return bmSource;
        }
    }
}