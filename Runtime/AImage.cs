using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.AREngine;
using System.Diagnostics;
using UnityEngine;

public class AImage

{
    public IntPtr Pointer =>pointer;
    private IntPtr pointer;
    public AImage(IntPtr pointer)
    {
        this.pointer = pointer;
    }

    public int GetFormat()
    {
        int format = 0;
        int flag = AImage_getFormat(pointer, ref format);
        HandleFlag(flag);
        return format;
    }
    public int GetWidth()
    {
        int width = 0;
        int flag = AImage_getWidth(pointer, ref width);
        HandleFlag(flag);
        return width;


    }
    public int GetHeight()
    {
        int height = 0;
        int flag = AImage_getHeight(pointer, ref height);
        HandleFlag(flag);
        return height;
    }
    public void GetPlaneData(int planeIndex, ref IntPtr data, ref int dataLength)
    {
        int flag = AImage_getPlaneData(pointer, planeIndex, ref data, ref dataLength);
        HandleFlag(flag);

    }
    public int GetPlanePixelStride(int planeIndex)
    {
        int pixelStride = 0;
        int flag = AImage_getPlanePixelStride(pointer, planeIndex, ref pixelStride);
        HandleFlag(flag);
        return pixelStride;
    }

    public int GetPlaneRowStride(int planeIndex)
    {
        int rowStride = 0;
        int flag = AImage_getPlaneRowStride(pointer, planeIndex, ref rowStride);
        HandleFlag(flag);
        return rowStride;
    }
    private void HandleFlag(int flag)
    {
        if (MediaStatus.AMEDIA_OK != (MediaStatus)flag)
        {
            throw new Exception(String.Format("AImage error: {0}", (MediaStatus)flag));
        }
    }


    [DllImport(Constants.MEDIA_NDK_NAME)]
    private static extern int AImage_getFormat(IntPtr image,
               ref int format);
    [DllImport(Constants.MEDIA_NDK_NAME)]
    private static extern int AImage_getWidth(IntPtr image, ref int width);
    [DllImport(Constants.MEDIA_NDK_NAME)]
    private static extern int AImage_getHeight(IntPtr image, ref int height);
    [DllImport(Constants.MEDIA_NDK_NAME)]
    private static extern int AImage_getPlaneData(IntPtr image, int planeIdx, ref IntPtr data,
        ref int dataLength);
    [DllImport(Constants.MEDIA_NDK_NAME)]
    private static extern int AImage_getPlanePixelStride(IntPtr image, int planeIdx, ref int pixelStride);
    [DllImport(Constants.MEDIA_NDK_NAME)]
    private static extern int AImage_getPlaneRowStride(IntPtr image, int planeIdx, ref int rowStride);
}
