using System;
using System.Runtime.InteropServices;
namespace UnityEngine.XR.AREngine
{
    public class HwArImage : IDisposable
    {
        internal IntPtr Pointer
        {
            get
            {
                return pointer;
            }
        }
        private bool disposed;
        private IntPtr pointer;
        public HwArImage(IntPtr ptr)
        {
            pointer = ptr;
            disposed = false;
        }
        ~HwArImage()
        {
            Debug.Log("zxh HwArImage finalize:"+pointer);
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public AImage GetNdkImage()
        {
            IntPtr aImage = IntPtr.Zero;
            HwArImage_getNdkImage(pointer, ref aImage);
            return new AImage(aImage);
        }
        protected void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if(disposing){
                Debug.Log("zxh HwArImage Dispose:"+pointer);
               HwArImage_release(pointer);
            }
            
            pointer = IntPtr.Zero;
            disposed = true;
        }
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArImage_getNdkImage(IntPtr image, ref IntPtr outNdkImage);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArImage_release(IntPtr image);
    }
}

/*
void

HwArImage_getNdkImage(const HwArImage *image, const AImage **outNdkImage)

获取ArImage对象对应的Android NDK AImage对象。

void

HwArImage_release(HwArImage *image)

释放由HwArFrame_acquireCameraImage返回的实例。
*/