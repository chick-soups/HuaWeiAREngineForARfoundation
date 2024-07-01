using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngine.XR.AREngine
{
    public class HwArCamera : IDisposable
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

        internal HwArCamera(HwArSession arSession, IntPtr pointer)
        {
            this.arSession = arSession;
            this.pointer = pointer;
        }
        ~HwArCamera()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public HwArCameraIntrinsics GetImageIntrinsics()
        {
            HwArCameraIntrinsics hwArCameraIntrinsics = new HwArCameraIntrinsics(arSession);
            HwArCamera_getImageIntrinsics(arSession.Pointer, pointer, hwArCameraIntrinsics.Pointer);
            return hwArCameraIntrinsics;
        }
        public Pose GetPose()
        {
            using (HwArPose arPose = new HwArPose(arSession))
            {
                HwArCamera_getPose(arSession.Pointer, pointer, arPose.Pointer);
                return arPose.GetPose();
            }
        }

        public Pose GetDisplayOrientedPose()
        {
            using (HwArPose arPose = new HwArPose(arSession))
            {
                HwArCamera_getDisplayOrientedPose(arSession.Pointer, pointer, arPose.Pointer);
                return arPose.GetPose();
            }

        }
        public Matrix4x4 GetProjectionMatrix(float near, float far)
        {
            if (near < 0 || far <= 0 || near >= far)
            {
                throw new ArgumentOutOfRangeException($"near:{near} far:{far}");
            }
            Matrix4x4 matrix = Matrix4x4.identity;
            HwArCamera_getProjectionMatrix(arSession.Pointer, pointer, near, far, ref matrix);
            //In Unity, projection matrices follow OpenGL convention.So we don't need to transfer right-handed to left-handed.
            return matrix;
        }
        public Matrix4x4 GetViewMatrix()
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            HwArCamera_getViewMatrix(arSession.Pointer, pointer, ref matrix);
            matrix = MathfExtenstion.AREngineToUnity(matrix.inverse);
            return matrix.inverse;
        }

        public HwArTrackingState GetTrackingState()
        {
            int state = 0;
            HwArCamera_getTrackingState(arSession.Pointer, pointer, ref state);
            return (HwArTrackingState)state;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                 HwArCamera_release(pointer);
            }
            arSession = null;
            pointer = IntPtr.Zero;
            disposed = true;
        }
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCamera_getImageIntrinsics(IntPtr session, IntPtr camera, IntPtr outIntrinsics);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCamera_getTrackingState(IntPtr session, IntPtr camera, ref int outTrackingState);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCamera_release(IntPtr camera);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCamera_getPose(IntPtr session, IntPtr camera, IntPtr outPose);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCamera_getDisplayOrientedPose(IntPtr session, IntPtr camera,
                                   IntPtr outPose);
        //this function is unused in unity
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCamera_getViewMatrix(IntPtr session, IntPtr camera,
                          ref Matrix4x4 destColMajor4x4);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArCamera_getProjectionMatrix(IntPtr session, IntPtr camera,
                                float near, float far, ref Matrix4x4 outColMajor4x4);



    }
}

/*
void

HwArCamera_getDisplayOrientedPose(const HwArSession *session, const HwArCamera *camera, HwArPose *outPose)

设置outPose为虚拟相机在世界空间中的位姿，用以将AR内容渲染到最新帧中。

void

HwArCamera_getImageIntrinsics(const HwArSession *session, const HwArCamera *camera, HwArCameraIntrinsics *outIntrinsics)

获取物理相机离线内参对象。

void

HwArCamera_getPose(const HwArSession *session, const HwArCamera *camera, HwArPose *outPose)

设置outPose为最新帧中物理相机在世界空间中的位姿。

void

HwArCamera_getProjectionMatrix(const HwArSession *session, const HwArCamera *camera, float near, float far, float *destColMajor4x4)

获取用于在相机图像上层渲染虚拟内容的投影矩阵。

void

HwArCamera_getTrackingState(const HwArSession *session, const HwArCamera *camera, HwArTrackingState *outTrackingState)

获取相机的当前追踪状态。

void

HwArCamera_getViewMatrix(const HwArSession *session, const HwArCamera *camera, float *outColMajor4x4)

获取最新帧中相机的视图矩阵。

void

HwArCamera_release(HwArCamera *camera)

释放对相机的引用。
*/