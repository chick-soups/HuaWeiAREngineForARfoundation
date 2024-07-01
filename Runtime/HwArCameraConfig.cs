using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngine.XR.AREngine
{
    public class HwArCameraConfig:IDisposable
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

        internal HwArCameraConfig(HwArSession arSession)
        {
            this.arSession = arSession;
            HwArCameraConfig_create(arSession.Pointer, ref pointer);
        }

        ~HwArCameraConfig()
        {
            Dispose(false);
        }

        public Vector2Int GetImageDimensions()
        {
            int width = 0;
            int height = 0;
            HwArCameraConfig_getImageDimensions(arSession.Pointer, pointer, ref width, ref height);
            return new Vector2Int(width, height);
        }
        public Vector2Int GetTextureDimensions()
        {
            int width = 0;
            int height = 0;
            HwArCameraConfig_getTextureDimensions(arSession.Pointer, pointer, ref width, ref height);
            return new Vector2Int(width, height);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
            HwArCameraConfig_destroy(pointer);
            pointer = IntPtr.Zero;

            disposed = true;
        }

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraConfig_create(IntPtr session, ref IntPtr outCameraConfig);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraConfig_destroy(IntPtr cameraConfig);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraConfig_getImageDimensions(IntPtr session, IntPtr cameraConfig,
            ref int outWidth, ref int outHeight);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCameraConfig_getTextureDimensions(IntPtr session, IntPtr cameraConfig,
            ref int outWidth, ref int outHeight);



    }
}

// void HwArCameraConfig_create(const HwArSession *session,
//                              HwArCameraConfig **outCameraConfig);

// void HwArCameraConfig_destroy(HwArCameraConfig *cameraConfig);

// void HwArCameraConfig_getImageDimensions(const HwArSession *session,
//                                          const HwArCameraConfig *cameraConfig,
//                                          int32_t *outWidth,
//                                          int32_t *outHeight);

// void HwArCameraConfig_getTextureDimensions(const HwArSession *session,
//                                            const HwArCameraConfig *cameraConfig,
//                                            int32_t *outWidth,
//                                            int32_t *outHeight);