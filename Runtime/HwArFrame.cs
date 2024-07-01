using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.AREngine
{
    public class HwArFrame : IDisposable
    {
        internal IntPtr Pointer
        {
            get
            {
                return pointer;
            }
        }
        private HwArSession arSession;
        private bool disposed;
        private IntPtr pointer;

        public HwArFrame(HwArSession arSession)
        {
            if (arSession == null || arSession.Pointer == IntPtr.Zero)
            {
                throw new ArgumentNullException("");
            }
            this.arSession = arSession;
            HwArFrame_create(arSession.Pointer, ref pointer);

        }
        ~HwArFrame()
        {
            Dispose(false);
        }
        public HwArCamera AcquireCamera()
        {
            IntPtr cameraPointer = IntPtr.Zero;
            HwArFrame_acquireCamera(arSession.Pointer, pointer, ref cameraPointer);
            return new HwArCamera(arSession, cameraPointer);
        }
        public HwArImage AcquireCameraImage()
        {
            IntPtr imagePointer = IntPtr.Zero;
            HwArFrame_acquireCameraImage(arSession.Pointer, pointer, ref imagePointer);
            return new HwArImage(imagePointer);
        }
        public HwArImage AcquireDepthImage()
        {
            IntPtr imagePointer = IntPtr.Zero;
            HwArFrame_acquireDepthImage(arSession.Pointer, pointer, ref imagePointer);
            return new HwArImage(imagePointer);
        }
        // public HwArImageMetadata AcquireImageMetadata()
        // {
        //     IntPtr imageMetadataPointer = IntPtr.Zero;
        //     HwArFrame_acquireImageMetadata(arSession.Pointer, pointer, ref imageMetadataPointer);
        //     return new HwArImageMetadata(arSession, imageMetadataPointer);
        // }
        // public HwArPointCloud AcquirePointCloud()
        // {
        //     IntPtr pointCloudPointer = IntPtr.Zero;
        //     HwArFrame_acquirePointCloud(arSession.Pointer, pointer, ref pointCloudPointer);
        //     return new HwArPointCloud(
        // }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool GetDisplayGeometryChanged()
        {
            int changed = 0;
            HwArFrame_getDisplayGeometryChanged(arSession.Pointer, pointer, ref changed);
            return changed != 0;
        }

        public long GetTimestamp()
        {
            long timestamp = 0;
            HwArFrame_getTimestamp(arSession.Pointer, pointer, ref timestamp);
            return timestamp;
        }

        public void TransformDisplayUvCoords(float[] uvsIn, float[] uvsOut)
        {
            if (uvsIn == null || uvsOut == null)
            {
                throw new ArgumentNullException("");
            }
            int length = uvsIn.Length;
            if (length != uvsOut.Length)
            {
                uvsOut = new float[length];
            }
            GCHandle a = GCHandle.Alloc(uvsIn);
            GCHandle b = GCHandle.Alloc(uvsOut);
            IntPtr aPointer = Marshal.UnsafeAddrOfPinnedArrayElement(uvsIn, 0);
            IntPtr bPointer = Marshal.UnsafeAddrOfPinnedArrayElement(uvsOut, 0);
            HwArFrame_transformDisplayUvCoords(arSession.Pointer, pointer, length, aPointer, bPointer);
            a.Free();
            b.Free();

        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                HwArFrame_destroy(pointer);
            }
            arSession = null;
            pointer = IntPtr.Zero;

            disposed = true;
        }

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_acquireCamera(IntPtr session, IntPtr frame, ref IntPtr outCamera);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        public static extern HwArStatus HwArFrame_acquireCameraImage(IntPtr session, IntPtr frame, ref IntPtr outImage);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArFrame_acquireDepthImage(IntPtr session, IntPtr frame,
            ref IntPtr Image);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArFrame_acquireImageMetadata(IntPtr session, IntPtr frame, ref IntPtr outMetadata);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArFrame_acquirePointCloud(IntPtr session, IntPtr frame, ref IntPtr outPointCloud);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArFrame_acquireSceneMesh(IntPtr session, IntPtr frame, ref IntPtr outSceneMesh);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_create(IntPtr session, ref IntPtr outFrame);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_destroy(IntPtr frame);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_getAlignState(IntPtr session, IntPtr frame, ref int outAlignState);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_getDisplayGeometryChanged(IntPtr session, IntPtr frame, ref int outGeometryChanged);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_getLightEstimate(IntPtr session, IntPtr frame, IntPtr outLightEstimate);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_getMappingState(IntPtr session, IntPtr frame, ref int outMappingState);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_getTimestamp(IntPtr session, IntPtr frame, ref long outTimestampNs);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_getUpdatedAnchors(IntPtr session, IntPtr frame, ref IntPtr outAnchorList);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_getUpdatedTrackables(IntPtr session, IntPtr frame, HwArTrackableType filterType, IntPtr outTrackableList);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_hitTest(IntPtr session, IntPtr frame, float pixelX, float pixelY, IntPtr hitResultList);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArFrame_hitTestArea(IntPtr session, IntPtr frame, IntPtr input2DPoints, int inputLength, IntPtr hitResultList);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArFrame_transformDisplayUvCoords(IntPtr session, IntPtr frame, int numElements, IntPtr uvsIn, IntPtr uvsOut);

    }

}
/*
void

HwArFrame_acquireCamera(const HwArSession *session, const HwArFrame *frame, HwArCamera **outCamera)

获取当前帧的相机参数对象。

HwArStatus

HwArFrame_acquireCameraImage(HwArSession *session, HwArFrame *frame, HwArImage **outImage)

在camera状态为tracking状态下，获取当前帧对应的图像。

void

HwArFrame_acquireDepthImage(HwArSession *session, HwArFrame *frame, HwArImage **outImage)

在相机为tracking状态下，获取当前帧对应的深度图像。

HwArStatus

HwArFrame_acquireImageMetadata(const HwArSession *session, const HwArFrame *frame, HwArImageMetadata **outMetadata)

获取图像的Metadata信息。

HwArStatus

HwArFrame_acquirePointCloud(const HwArSession *session, const HwArFrame *frame, HwArPointCloud **outPointCloud)

返回当前帧的点云数据。

HwArStatus

HwArFrame_acquirePreviewImage(HwArSession *session, HwArFrame *frame, HwArImage **outImage)

在camera为tracking状态下，获取当前帧对应的高清预览图。

HwArStatus

HwArFrame_acquireSceneMesh(const HwArSession *session, const HwArFrame *frame, HwArSceneMesh **outSceneMesh)

返回当前帧对应的环境Mesh对象。

void

HwArFrame_create(const HwArSession *session, HwArFrame **outFrame)

创建一个新的HwArFrame对象，将指针存储到中*outFrame。

void

HwArFrame_destroy(HwArFrame *frame)

删除当前HwArFrame对象。

void

HwArFrame_getDisplayGeometryChanged(const HwArSession *session, const HwArFrame *frame, int32_t *outGeometryChanged)

获取显示（长宽和旋转）是否发生变化。

void

HwArFrame_getLightEstimate(const HwArSession *session, const HwArFrame *frame, HwArLightEstimate *outLightEstimate)

获取当前帧对应的光照估计对象。

void

HwArFrame_getTimestamp(const HwArSession *session, const HwArFrame *frame, int64_t *outTimestampNs)

获取当前帧对应的时间戳信息。

void

HwArFrame_getUpdatedAnchors(const HwArSession *session, const HwArFrame *frame, HwArAnchorList *outAnchorList)

获取在两次HwArSession_update之间更新过的锚点信息。

void

HwArFrame_getUpdatedTrackables(const HwArSession *session, const HwArFrame *frame, HwArTrackableType filterType, HwArTrackableList *outTrackableList)

获取HwArSession_update更新后发生变化的指定类型的可跟踪对象。

void

HwArFrame_hitTest(const HwArSession *session, const HwArFrame *frame, float pixelX, float pixelY, HwArHitResultList *hitResultList)

从摄像头发射一条射线，该射线的方向由屏幕上的点(axisX, axisY)确定。

HwArStatus

HwArFrame_hitTestArea(const HwArSession *session, const HwArFrame *frame, float *input2DPoints, int32_t length, HwArHitResultList *hitResultList)

接受屏幕上的一系列点击位置，返回对应的碰撞结果。

void

HwArFrame_transformDisplayUvCoords(const HwArSession *session, const HwArFrame *frame, int32_t numElements, const float *uvsIn, float *uvsOut)

调整纹理映射坐标，以便可以正确地显示相机捕捉到的背景图片。
*/