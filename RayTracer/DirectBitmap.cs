using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RayTracer
{
    /// <summary>
    /// Bitmap with faster Get and Set Pixel method
    /// </summary>
    class DirectBitmap : IDisposable
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public DirectBitmap(int width, int height)
        {
            source = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            LockBits();
        }

        public DirectBitmap(string filename)
        {
            var fromFile = (Bitmap)Image.FromFile(filename);
            var copyAble = new Bitmap(fromFile);
            source = copyAble.Clone(new Rectangle(0, 0, copyAble.Width, copyAble.Height), PixelFormat.Format32bppArgb);
            LockBits();
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                // create byte array to copy pixel values
                int step = Depth / 8;
                Pixels = new byte[PixelCount * step];
                Iptr = bitmapData.Scan0;

                // Copy data from pointer to array
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                // Copy data from byte array to pointer
                Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                // Unlock bitmap data
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            int i = ((y * Width) + x) * 4;
            return new Color(Pixels[i + 2] / 255.0, Pixels[i + 1] / 255.0, Pixels[i] / 255.0);
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * 4;


            Pixels[i] = (byte)Math.Min(255, 255*color.B);
            Pixels[i + 1] = (byte)Math.Min(255, 255*color.G);
            Pixels[i + 2] = (byte)Math.Min(255, 255*color.R);
            Pixels[i + 3] = 255;
        }

        /// <summary>
        /// Save bitmap to file
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            source.Save(filename);
        }
        /// <summary>
        /// Dispose bitmap and unlock bits
        /// </summary>
        public void Dispose()
        {
            UnlockBits();
            source.Dispose();
        }
    }
}
