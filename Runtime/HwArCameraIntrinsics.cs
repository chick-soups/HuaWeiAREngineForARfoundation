using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.AREngine
{
    public class HwArCameraIntrinsics:IDisposable
    {
        internal IntPtr Pointer
        {
            get
            {
                return pointer;
            }
        }
        private HwArSession arSession;
        private IntPtr pointer;
        private bool disposed;

        internal HwArCameraIntrinsics(HwArSession arSession)
        {
            this.arSession = arSession;
            HwArCameraIntrinsics_create(arSession.Pointer, ref pointer);
        }
        internal HwArCameraIntrinsics(HwArSession arSession, IntPtr pointer)
        {
            this.arSession = arSession;
            this.pointer = pointer;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public Vector2Int GetImageDimensions()
        {
            int width = 0, height = 0;
            HwArCameraIntrinsics_getImageDimensions(arSession.Pointer, pointer, ref width, ref height);
            return new Vector2Int(width, height);
        }

        public Vector2 GetPrincipalPoint()
        {
            Vector2 vector = Vector2.zero;
            HwArCameraIntrinsics_getPrincipalPoint(arSession.Pointer, pointer, ref vector.x, ref vector.y);
            return vector;
        }

        public Vector2 GetFocalLength()
        {
            Vector2 vector = Vector2.zero;
            HwArCameraIntrinsics_getFocalLength(arSession.Pointer, pointer, ref vector.x, ref vector.y);
            return vector;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                arSession = null;
            }
            HwArCameraIntrinsics_destroy(pointer);
            pointer = IntPtr.Zero;
            disposed = true;
        }

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraIntrinsics_create(IntPtr session, ref IntPtr outIntrinsics);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraIntrinsics_destroy(IntPtr intrinsics);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraIntrinsics_getFocalLength(IntPtr session, IntPtr intrinsics,
            ref float outFocalX, ref float outFocalY);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraIntrinsics_getImageDimensions(IntPtr session, IntPtr intrinsics,
            ref int outWidth, ref int outHeight);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraIntrinsics_getPrincipalPoint(IntPtr session, IntPtr intrinsics,
       ref float outPrincipalX, ref float outPrincipalY);
    }
}
/*
void

HwArCameraIntrinsics_create(const HwArSession *session, HwArCameraIntrinsics **outIntrinsics)

创建一个相机内参对象。

void

HwArCameraIntrinsics_destroy(const HwArSession *session, HwArCameraIntrinsics *intrinsics)

释放指定的相机内参对象。

void

HwArCameraIntrinsics_getDistortion(const HwArSession *session, const HwArCameraIntrinsics *intrinsics, float (&outDistortion)[5])

获取相机的畸变系数。

void

HwArCameraIntrinsics_getFocalLength(const HwArSession *session, const HwArCameraIntrinsics *intrinsics, float *outFocalX, float *outFocalY)

获取指定相机的焦距。

void

HwArCameraIntrinsics_getImageDimensions(const HwArSession *session, const HwArCameraIntrinsics *intrinsics, int32_t *outWidth, int32_t *outHeight)

获取相机图像的尺寸，包括宽度和高度（以像素为单位）。

void

HwArCameraIntrinsics_getPrincipalPoint(const HwArSession *session, const HwArCameraIntrinsics *intrinsics, float *outPrincipalX, float *outPrincipalY)

获取指定相机的主轴点。
*/