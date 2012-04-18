using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.DwayneNeed.Media.Imaging;

namespace WPFTestHarness
{
	/// <summary>
	///     The GrayscaleBitmap class processes a source and converts the
	///     image to a grayscale color scheme.
	/// </summary>
	/// <remarks>
	///     For simplicity, we only process Bgr32 or Bgra32 formatted bitmaps,
	///     anything else is converted to one of those formats. Bgr32 and
	///     Bgra32 share the same memory layout for the RGB channels.
	/// </remarks>
	public class Grayscale4Bitmap : ChainedBitmap
	{
		#region BitmapSource Properties

		/// <summary>
		///     Pixel format of the bitmap.
		/// </summary>
		/// <remarks>
		///     We will work natively with either Bgr32 or Bgra32, since both
		///     formats have the same memory layout.
		/// </remarks>
		public sealed override PixelFormat Format
		{
			get
			{
				// Preserve an alpha channel, if there is one.
				PixelFormat format = base.Format;
				if (format == PixelFormats.Bgra32 ||
					format == PixelFormats.Prgba64 ||
					format == PixelFormats.Pbgra32 ||
					format == PixelFormats.Prgba128Float ||
					format == PixelFormats.Rgba128Float ||
					format == PixelFormats.Rgba64)
				{
					format = PixelFormats.Bgra32;
				}
				else
				{
					format = PixelFormats.Bgr32;
				}

				return format;
			}
		}

		/// <summary>
		///     Palette of the bitmap.
		/// </summary>
		/// <remarks>
		///     We only support Bgr32 and Bgra32 pixel formats, so a palette
		///     is never needed, so we return null.
		/// </remarks>
		public override BitmapPalette Palette
		{
			get
			{
				return null;
			}
		}

		#endregion BitmapSource Properties

		#region BitmapSource CopyPixels

		/// <summary>
		///     Requests pixels from the ChainedCustomBitmapSource.
		/// </summary>
		/// <param name="sourceRect">
		///     The rectangle of pixels being requested. 
		/// </param>
		/// <param name="stride">
		///     The stride of the destination buffer.
		/// </param>
		/// <param name="bufferSize">
		///     The size of the destination buffer.
		/// </param>
		/// <param name="buffer">
		///     The destination buffer.
		/// </param>
		/// <remarks>
		///     Converts the contents of the source bitmap into a grayscale
		///     color scheme.
		/// 
		///     The algorithm is taken from here:
		///     http://en.wikipedia.org/wiki/Grayscale
		/// </remarks>
		protected override void CopyPixelsCore(Int32Rect sourceRect, int stride, int bufferSize, IntPtr buffer)
		{
			BitmapSource source = Source;
			if (source != null)
			{
				// First defer to the base implementation, which will fill in
				// the buffer from the source and convert the pixel format as
				// needed.
				base.CopyPixelsCore(sourceRect, stride, bufferSize, buffer);

				// The buffer has been filled with Bgr32 or Bgra32 pixels.
				// Now process those pixels into grayscale.  Ignore the
				// alpha channel.
				//
				// Note: if this buffer pointer came from a managed array, the
				// array has already been pinned.
				unsafe
				{
					byte* pBytes = (byte*)buffer.ToPointer();
					for (int y = 0; y < sourceRect.Height; y++)
					{
						Bgra32Pixel* pPixel = (Bgra32Pixel*)pBytes;

						for (int x = 0; x < sourceRect.Width; x++)
						{
							System.Diagnostics.Debug.Assert(pPixel->Red == pPixel->Green);
							System.Diagnostics.Debug.Assert(pPixel->Red == pPixel->Blue);

							float gray;
							if (pPixel->Red < 16)
								gray = 0.0000f;
							else if (pPixel->Red < 32)
								gray = 0.0667f;
							else if (pPixel->Red < 48)
								gray = 0.1333f;
							else if (pPixel->Red < 64)
								gray = 0.2000f;
							else if (pPixel->Red < 80)
								gray = 0.2667f;
							else if (pPixel->Red < 96)
								gray = 0.3333f;
							else if (pPixel->Red < 112)
								gray = 0.4000f;
							else if (pPixel->Red < 128)
								gray = 0.4667f;
							else if (pPixel->Red < 144)
								gray = 0.5333f;
							else if (pPixel->Red < 160)
								gray = 0.6000f;
							else if (pPixel->Red < 176)
								gray = 0.6667f;
							else if (pPixel->Red < 192)
								gray = 0.7333f;
							else if (pPixel->Red < 208)
								gray = 0.8000f;
							else if (pPixel->Red < 224)
								gray = 0.8667f;
							else if (pPixel->Red < 240)
								gray = 0.9333f;
							else
								gray = 1f;

							Color cGray = Color.FromScRgb(1.0f, gray, gray, gray);

							// Write sRGB (non-linear) since it is implied by
							// the pixel format we chose.
							pPixel->Red = cGray.R;
							pPixel->Green = cGray.G;
							pPixel->Blue = cGray.B;

							pPixel++;
						}

						pBytes += stride;
					}
				}
			}
		}

		#endregion BitmapSource CopyPixels
	}
}
