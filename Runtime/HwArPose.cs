using System;
using System.Runtime.InteropServices;
namespace UnityEngine.XR.AREngine
{
    public class HwArPose : IDisposable
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
        private float[] poseRaw;
        private GCHandle gcHandle;
       
        public HwArPose(HwArSession arSession)
        {
            this.arSession = arSession;
            poseRaw = new float[7];
            gcHandle = GCHandle.Alloc(poseRaw, GCHandleType.Pinned);
            IntPtr a = gcHandle.AddrOfPinnedObject();
            HwArPose_create(arSession.Pointer, a, ref pointer);
        }

        public HwArPose(HwArSession arSession, Pose leftHandPose)
        {
            this.arSession = arSession;
            Pose rightHandPose = MathfExtenstion.UnityToAREngine(leftHandPose);
            poseRaw = new float[]{
                rightHandPose.rotation.x,
                rightHandPose.rotation.y,
                rightHandPose.rotation.z,
                rightHandPose.rotation.w,
                rightHandPose.position.x,
                rightHandPose.position.y,
                rightHandPose.position.z,
            };
            gcHandle = GCHandle.Alloc(poseRaw, GCHandleType.Pinned);
            IntPtr a = gcHandle.AddrOfPinnedObject();
            HwArPose_create(arSession.Pointer, a, ref pointer);
        }

        public Pose GetPose()
        {
            HwArPose_getPoseRaw(arSession.Pointer, pointer, gcHandle.AddrOfPinnedObject());
            Pose pose = new Pose(new Vector3(poseRaw[4], poseRaw[5], poseRaw[6]),
            new Quaternion(poseRaw[0], poseRaw[1], poseRaw[2], poseRaw[3]));
            return MathfExtenstion.AREngineToUnity(pose);
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
                poseRaw = null;
            }
            HwArPose_destroy(pointer);
            pointer = IntPtr.Zero;
            gcHandle.Free();


            disposed = true;
        }

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        public static extern void HwArPose_create(IntPtr session,
                    IntPtr poseRaw, ref IntPtr outPose);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        public static extern void HwArPose_destroy(IntPtr pose);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        public static extern void HwArPose_getPoseRaw(IntPtr session, IntPtr pose,
                     IntPtr outPoseRaw);


        //   void

        // HwArPose_create(const HwArSession *session, const float *poseRaw, HwArPose **outPose)

        // 分配并初始化一个新的位姿对象。

        // void

        // HwArPose_destroy(HwArPose *pose)

        // 释放位姿对象使用的内存。

        // void

        // HwArPose_getMatrix(const HwArSession *session, const HwArPose *pose, float *outMatrixColMajor4x4)

        // 将位姿转换为4x4转换矩阵。

        // void

        // HwArPose_getPoseRaw(const HwArSession *session, const HwArPose *pose, float *outPoseRaw)

        // 从位姿对象中提取旋转与平移四元数。
    }
}