using Newskies.WebApi.Services;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class GetBarCodedBoardingPassesResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var result = response as Newskies.WebApi.Contracts.GetBarCodedBoardingPassesResponse;
            if (result == null)
                return await Task.FromResult(response);
            foreach (var barcodedBP in result.BarCodedBoardingPasses)
            {
                if (barcodedBP.BarCode != null && !string.IsNullOrEmpty(barcodedBP.BarCode.BarCodeData))
                {
                    var barcodeWriter = new ZXing.BarcodeWriterPixelData
                    {
                        Format = ZXing.BarcodeFormat.PDF_417,
                        Options = new ZXing.Common.EncodingOptions
                        {
                            Height = result.BarcodeHeight > 0 ? result.BarcodeHeight : 100,
                            Width = result.BarcodeWidth > 0 ? result.BarcodeWidth : 300,
                            Margin = 0
                        }
                    };
                    var pixelData = barcodeWriter.Write(barcodedBP.BarCode.BarCodeData);

                    // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
                    // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
                    // the System.Drawing.Bitmap class is provided by the CoreCompat.System.Drawing package
                    using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
                    {
                        using (var ms = new MemoryStream())
                        {
                            // lock the data area for fast access
                            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                            try
                            {
                                // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                            }
                            finally
                            {
                                bitmap.UnlockBits(bitmapData);
                            }
                            // save to stream as PNG
                            bitmap.Save(ms, ImageFormat.Png);
                            var base64string = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
                            barcodedBP.BarCode.BarCodeImageBase64 = base64string;
                        }
                    }
                }
            }
            return await Task.FromResult(result);
        }
    }
}
