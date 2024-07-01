
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Data;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityEngine.XR.AREngine
{
    /// <summary>
    /// Utility methods for working with texture objects.
    /// </summary>
    public static class TextureUtils
    {

        /// <summary>
        /// Filtering to use when resizing a texture
        /// </summary>
        public enum ImageFilterMode
        {
            /// <summary>
            /// Nearest pixel, fastest but gets aliased
            /// </summary>
            Nearest = 0,
            /// <summary>
            /// Use bilinear scaling
            /// </summary>
            Biliner = 1,
            /// <summary>
            /// Average of all nearby pixels
            /// </summary>
            Average = 2
        }
        [Flags]
        public enum Transform
        {
            None,
            MirrorX,
            MirrorY,

        }


        private static int logTime = 0;
        public static byte[] ResizeTexture(byte[] pSource, int width, int height, ImageFilterMode pFilterMode, int targetWidth, int targetHeight, bool mirrorX, bool mirrorY)
        {




            Vector2Int vSourceSize = new Vector2Int(width, height);
            int xWidth = targetWidth;
            int xHeight = targetHeight;
            byte[] oNewTex = new byte[xWidth * xHeight * 3];


            //*** Check if we need to resize
            if (xWidth == width && xHeight == height)
            {
                if (mirrorX == false && mirrorY == false)
                {
                    return pSource;
                }
                else
                {
                    Parallel.For(0, xHeight, xY =>
          {

              for (int xX = 0; xX < xWidth; xX++)
              {
                  int indexX = xX;
                  if (mirrorY)
                  {
                      indexX = xWidth - 1 - indexX;
                  }
                  int indexY = xY;
                  if (mirrorX)
                  {
                      indexY = xHeight - 1 - indexY;
                  }
                  int destIndex = (xY * xWidth + xX) * 3;
                  int sourceIndex = (indexY * xWidth + indexX) * 3;
                  Array.Copy(pSource, sourceIndex, oNewTex, destIndex, 3);

              }
          });
                    return oNewTex;

                }
            }

            Vector2 vCenter = new Vector2();
            Vector2 vPixelSize = new Vector2(vSourceSize.x / (float)xWidth, vSourceSize.y / (float)xHeight);

            Parallel.For(0, xHeight, xY =>
            {
                for (int xX = 0; xX < xWidth; xX++)
                {
                    vCenter.x = (xX / (float)xWidth) * vSourceSize.x;
                    vCenter.x = Mathf.Clamp(vCenter.x, 0, vSourceSize.x - 1);
                    if (mirrorY)
                    {
                        vCenter.x = vSourceSize.x - 1 - vCenter.x;
                    }

                    vCenter.y = (xY / (float)xHeight) * vSourceSize.y;
                    vCenter.y = Mathf.Clamp(vCenter.y, 0, vSourceSize.y - 1);
                    if (mirrorX)
                    {
                        vCenter.y = vSourceSize.y - 1 - vCenter.y;
                    }

                    int destIndex = (xY * xWidth + xX) * 3;
                    if (pFilterMode == ImageFilterMode.Nearest)
                    {
                        int x = Mathf.RoundToInt(vCenter.x);
                        int y = Mathf.RoundToInt(vCenter.y);
                        int xSourceIndex = (int)(y * vSourceSize.x) + x;
                        xSourceIndex *= 3;

                        Array.Copy(pSource, xSourceIndex, oNewTex, destIndex, 3);
                    }

                    //*** Bilinear
                    else if (pFilterMode == ImageFilterMode.Biliner)
                    {

                        int floorX = Mathf.FloorToInt(vCenter.x);
                        int floorY = Mathf.FloorToInt(vCenter.y);
                        floorX = Mathf.Clamp(floorX, 0, vSourceSize.x - 1);
                        floorY = Mathf.Clamp(floorY, 0, vSourceSize.y - 1);
                        int ceilX = floorX + 1;
                        int ceilY = floorY + 1;
                        ceilX = Mathf.Clamp(ceilX, 0, vSourceSize.x - 1);
                        ceilY = Mathf.Clamp(ceilY, 0, vSourceSize.y - 1);
                        float xRatioX = vCenter.x - floorX;
                        float xRatioY = vCenter.y - floorY;
                        long xIndexTL = floorY * vSourceSize.x + floorX;
                        long xIndexBL = ceilY * vSourceSize.x + floorX;

                        long pixelIndex = xIndexTL * 3L;

                        Color32 tl = new Color32(pSource[pixelIndex++], pSource[pixelIndex++], pSource[pixelIndex++], 255);
                        if (pixelIndex >= pSource.Length - 1)
                        {
                            pixelIndex -= 3;
                        }

                        Color32 tr = new Color32(pSource[pixelIndex++], pSource[pixelIndex++], pSource[pixelIndex++], 255);
                        pixelIndex = xIndexBL * 3L;

                        Color32 bl = new Color32(pSource[pixelIndex++], pSource[pixelIndex++], pSource[pixelIndex++], 255);
                        if (pixelIndex >= pSource.Length - 1)
                        {
                            pixelIndex -= 3;
                        }
                        Color32 br = new Color32(pSource[pixelIndex++], pSource[pixelIndex++], pSource[pixelIndex++], 255);

                        Color32 oColor = Color32.Lerp(Color32.Lerp(tl, tr, xRatioX), Color32.Lerp(bl, br, xRatioX), xRatioY);
                        oNewTex[destIndex++] = oColor.r;
                        oNewTex[destIndex++] = oColor.g;
                        oNewTex[destIndex++] = oColor.b;
                    }

                    //*** Average
                    else if (pFilterMode == ImageFilterMode.Average)
                    {
                        //*** Calculate grid around point
                        int xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - (vPixelSize.x * 0.5f)), 0);
                        int xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x - 1);
                        int xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - (vPixelSize.y * 0.5f)), 0);
                        int xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y - 1);


                        //*** Loop and accumulate
                        int[] oColorTemp = new int[3];
                        int xGridCount = 0;
                        for (int iy = xYFrom; iy <= xYTo; iy++)
                        {
                            long offset = (iy * vSourceSize.x + xXFrom) * 3;
                            for (int ix = xXFrom; ix <= xXTo; ix++)
                            {
                                oColorTemp[0] += (int)pSource[offset++];
                                oColorTemp[1] += (int)pSource[offset++];
                                oColorTemp[2] += (int)pSource[offset++];

                                xGridCount++;
                            }
                        }
                        oNewTex[destIndex++] = (byte)(oColorTemp[0] / xGridCount);
                        oNewTex[destIndex++] = (byte)(oColorTemp[1] / xGridCount);
                        oNewTex[destIndex++] = (byte)(oColorTemp[2] / xGridCount);
                    }

                }


            });
            return oNewTex;

        }

        public enum YuvType
        {
            YU12,
            NV12,
            NV21
        }

        public static byte[] GetBytesFromImageAsType(AImage aImage, YuvType targetType)
        {
            int width = aImage.GetWidth();
            int height = aImage.GetHeight();
            int size = sizeof(byte);
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();
            Task<byte[]> task = Task<byte[]>.Factory.StartNew(() =>
            {
                byte[] ybtes = new byte[width * height];
                try
                {
                    int pixelsStride = 0, rowStride = 0, dataLength = 0, yIndex = 0;
                    IntPtr source = IntPtr.Zero;
                    aImage.GetPlaneData(0, ref source, ref dataLength);
                    rowStride = aImage.GetPlaneRowStride(0);
                    pixelsStride = aImage.GetPlanePixelStride(0);
                    for (int j = 0; j < height; j++)
                    {
                        Marshal.Copy(source, ybtes, yIndex, width);
                        source += size * rowStride;
                        yIndex += width;
                    }

                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e.Message);
                    UnityEngine.Debug.LogError(e.StackTrace);
                }
                return ybtes;

            });
            tasks.Add(task);
            task = Task<byte[]>.Factory.StartNew(() =>
            {
                byte[] uBytes = new byte[width * height / 4];
                try
                {
                    int pixelsStride = 0, rowStride = 0, dataLength = 0, uIndex = 0;
                    IntPtr source = IntPtr.Zero;
                    aImage.GetPlaneData(1, ref source, ref dataLength);
                    rowStride = aImage.GetPlaneRowStride(1);
                    pixelsStride = aImage.GetPlanePixelStride(1);
                    for (int j = 0; j < height / 2; j++)
                    {
                        for (int k = 0; k < width / 2; k++)
                        {
                            uBytes[uIndex++] = Marshal.ReadByte(source);
                            source += size * pixelsStride;
                        }
                        if (pixelsStride == 2)
                        {
                            source += (rowStride - width) * size;
                        }
                        else if (pixelsStride == 1)
                        {
                            source += (rowStride - width / 2) * size;
                        }
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e.Message);
                    UnityEngine.Debug.LogError(e.StackTrace);

                }
                return uBytes;

            });
            tasks.Add(task);
            task = Task<byte[]>.Factory.StartNew(() =>
            {
                byte[] vBytes = new byte[width * height / 4];
                try
                {
                    int pixelsStride = 0, rowStride = 0, dataLength = 0, vIndex = 0;
                    IntPtr source = IntPtr.Zero;
                    aImage.GetPlaneData(2, ref source, ref dataLength);
                    rowStride = aImage.GetPlaneRowStride(2);
                    pixelsStride = aImage.GetPlanePixelStride(2);
                    for (int j = 0; j < height / 2; j++)
                    {
                        for (int k = 0; k < width / 2; k++)
                        {
                            vBytes[vIndex++] = Marshal.ReadByte(source);
                            source += size * pixelsStride;
                        }
                        if (pixelsStride == 2)
                        {
                            source += (rowStride - width) * size;
                        }
                        else if (pixelsStride == 1)
                        {
                            source += (rowStride - width / 2) * size;
                        }
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e.Message);
                    UnityEngine.Debug.LogError(e.StackTrace);

                }
                return vBytes;

            });
            tasks.Add(task);
            Task<byte[]> finialTask = Task<byte[][]>.WhenAll(tasks).ContinueWith((task) =>
            {
                byte[] yuvBytes = new byte[width * height * 3 / 2];
                try
                {
                    int dstIndex = 0;
                    byte[] yBytes = task.Result[0];
                    byte[] uBytes = task.Result[1];
                    byte[] vBytes = task.Result[2];
                    Array.Copy(yBytes, 0, yuvBytes, 0, yBytes.Length);
                    dstIndex = yBytes.Length;
                    switch (targetType)
                    {
                        case YuvType.YU12:
                            Array.Copy(uBytes, 0, yuvBytes, dstIndex, uBytes.Length);
                            dstIndex += uBytes.Length;
                            Array.Copy(vBytes, 0, yuvBytes, dstIndex, vBytes.Length);
                            break;
                        case YuvType.NV12:
                            for (int i = 0; i < vBytes.Length; i++)
                            {
                                yuvBytes[dstIndex++] = uBytes[i];
                                yuvBytes[dstIndex++] = vBytes[i];
                            }
                            break;
                        case YuvType.NV21:
                            for (int i = 0; i < vBytes.Length; i++)
                            {
                                yuvBytes[dstIndex++] = vBytes[i];
                                yuvBytes[dstIndex++] = uBytes[i];
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e.Message);
                    UnityEngine.Debug.LogError(e.StackTrace);

                }
                return yuvBytes;

            });
            return finialTask.Result;
        }


        public static byte[] DecodeYUV2RGB(byte[] yuvBytes, YuvType yuvType, int width, int height)
        {
            byte[] rgbBytes = new byte[width * height * 3];
            int length = width * height;
            int originalvIndex = 0;
            int originaluIndex = 0;
            int stride = 1;
            int rowStride = 0;
            switch (yuvType)
            {
                case YuvType.YU12:
                    originaluIndex = length;
                    originalvIndex = length + length / 4;
                    rowStride = width / 2;
                    break;
                case YuvType.NV12:
                    originaluIndex = length;
                    originalvIndex = length + 1;
                    stride = 2;
                    rowStride = width;
                    break;
                case YuvType.NV21:
                    originalvIndex = length;
                    originaluIndex = length + 1;
                    stride = 2;
                    rowStride = width;
                    break;
            }
            Parallel.For(0, height, j =>
            {
                int uIndex = originaluIndex + (j / 2) * rowStride;
                int vIndex = originalvIndex + (j / 2) * rowStride;
                int yindex = j * width;
                for (int i = 0; i < width; i++)
                {
                    int y = yuvBytes[yindex];
                    int currentUIndex = (i / 2) * stride + uIndex;
                    int currentVIndex = (i / 2) * stride + vIndex;
                    int u = yuvBytes[currentUIndex];
                    int v = yuvBytes[currentVIndex];
                    byte[] rgb = yuvTorgb(y, u, v);
                    Array.Copy(rgb, 0, rgbBytes, yindex * 3, 3);
                    yindex++;
                }
            });
            return rgbBytes;
        }




        private static byte[] yuvTorgb(int y, int u, int v)
        {
            int r = Mathf.RoundToInt(y + 1.403f * (v - 128));
            int g = Mathf.RoundToInt(y - 0.343f * (u - 128) - 0.714f * (v - 128));
            int b = Mathf.RoundToInt(y + 1.770f * (u - 128));
            r = Mathf.Clamp(r, 0, 255);
            g = Mathf.Clamp(g, 0, 255);
            b = Mathf.Clamp(b, 0, 255);
            return new byte[] { (byte)r, (byte)g, (byte)b };
        }

         public static byte[] RGB2RGBA32(byte[] rgb)
        {
            
            int length=rgb.Length/3;
            byte[] rgbaBytes=new byte[length*4];
            Parallel.For(0,length,i=> {
                int i4=i*4;
                int i3=i*3;
                rgbaBytes[i4] = rgb[i3];
                rgbaBytes[i4 + 1] = rgb[i3 + 1];
                rgbaBytes[i4 + 2] = rgb[i3 + 2];
                rgbaBytes[i4 + 3] = 255;
            });
            return rgbaBytes;
        }


        public static byte[] RGB2ARGB32(byte[] rgb)
        {
            int length = rgb.Length / 3;
            byte[] rgbaBytes = new byte[length * 4];
            Parallel.For(0, length, i =>
            {
                int i4 = i * 4;
                int i3 = i * 3;
                rgbaBytes[i4] = 255;
                rgbaBytes[i4 + 1] = rgb[i3];
                rgbaBytes[i4 + 2] = rgb[i3 + 1];
                rgbaBytes[i4 + 3] = rgb[i3 + 2];
            });
            return rgbaBytes;
        }
        public static byte[] RGB2BGRA32(byte[] rgb)
        {
            int length = rgb.Length / 3;
            byte[] rgbaBytes = new byte[length * 4];
            Parallel.For(0, length, i =>
            {
                int i4 = i * 4;
                int i3 = i * 3;
                rgbaBytes[i4] = rgb[i3 + 2];
                rgbaBytes[i4 + 1] = rgb[i3 + 1];
                rgbaBytes[i4 + 2] = rgb[i3];
                rgbaBytes[i4 + 3] = 255;
            });
            return rgbaBytes;
        }
    }
}
