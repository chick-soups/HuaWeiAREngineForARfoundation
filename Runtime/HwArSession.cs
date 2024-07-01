using System;
using System.Runtime.InteropServices;
using UnityEditor;

namespace UnityEngine.XR.AREngine
{
    public class HwArSession : IDisposable
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

        public HwArSession()
        {
            pointer = IntPtr.Zero;
            HwArStatus status = HwArSession_create(UnityAREngineAPI.UnityAREngine_GetJNIEnv(),
            UnityAREngineAPI.UnityAREngine_GetContext(), ref pointer);
            switch (status)
            {
                case HwArStatus.HWAR_SUCCESS:
                    break;
                default:
                    throw new ARUnavailableException(status);

            }
        }
        ~HwArSession()
        {
            Dispose(false);
        }

        public HwArCameraConfig GetCameraConfig()
        {
            HwArCameraConfig hwArCameraConfig = new HwArCameraConfig(this);
            HwArStatus status = HwArSession_getCameraConfig(pointer, hwArCameraConfig.Pointer);
            if (status != HwArStatus.HWAR_SUCCESS)
            {
                throw new ARUnavailableException(status);
            }
            return hwArCameraConfig;
        }
        public HwArStatus Configure(HwArConfig config)
        {
            HwArStatus hwArStatus = HwArSession_configure(pointer, config.Pointer);
            if (hwArStatus != HwArStatus.HWAR_SUCCESS)
            {
                throw new ARUnavailableException(hwArStatus);
            }
            return hwArStatus;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public HwArStatus Pause()
        {
            HwArStatus hwArStatus = HwArSession_pause(pointer);
            if (hwArStatus != HwArStatus.HWAR_SUCCESS)
            {
                throw new ARUnavailableException(hwArStatus);
            }
            return hwArStatus;
        }

        public HwArStatus Resume()
        {
            HwArStatus hwArStatus = HwArSession_resume(pointer);
            if (hwArStatus != HwArStatus.HWAR_SUCCESS)
            {
                throw new ARUnavailableException(hwArStatus);
            }
            return hwArStatus;
        }


        public void SetCameraTextureName(uint textureId)
        {
            HwArSession_setCameraTextureName(pointer, textureId);
        }
        public void SetDisplayGeometry(int rotation, int width, int height)
        {
            if (rotation < 0 || rotation > 4)
            {
                throw new ArgumentException("");
            }
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException($"width:{width} or height:{height} is less than 0.");
            }
            HwArSession_setDisplayGeometry(pointer, rotation, width, height);

        }
        public void Stop()
        {
            HwArSession_stop(pointer);
        }

        public HwArStatus Update(HwArFrame arFrame)
        {
            if (arFrame == null || arFrame.Pointer == IntPtr.Zero)
            {
                throw new ArgumentException("arframe is null or arframe's pointer is 0");
            }
            HwArStatus hwArStatus = HwArSession_update(pointer, arFrame.Pointer);
            if (hwArStatus != HwArStatus.HWAR_SUCCESS)
            {
                throw new ARUnavailableException(hwArStatus);
            }
            Debug.Log("zxh update arframe success");
            return hwArStatus;
        }
        protected void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if(disposing){
               HwArSession_destroy(pointer);
            }
           
            pointer = IntPtr.Zero;
            disposed = true;
        }
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_configure(IntPtr session, IntPtr config);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_create(IntPtr env, IntPtr applicationContext, ref IntPtr outSessionPointer);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArSession_destroy(IntPtr session);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_pause(IntPtr session);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_resume(IntPtr session);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArSession_setDisplayGeometry(IntPtr session, int rotation, int width, int height);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArSession_setCameraTextureName(IntPtr session, uint textureId);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArSession_stop(IntPtr session);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_update(IntPtr session, IntPtr outFrame);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_checkSupported(IntPtr session, IntPtr config);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArSession_setCameraTextureName(IntPtr session, int textureId);


        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_acquireNewAnchor(IntPtr session, IntPtr pose,
            ref IntPtr outAnchor);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArSession_getAllAnchors(IntPtr session, IntPtr outAnchorList);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArSession_getAllTrackables(IntPtr session,
                              HwArTrackableType filterType, IntPtr outTrackableList);


        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_getCameraConfig(IntPtr session, IntPtr outCameraConfig);

        // [DllImport(AdapterConstants.UnityPluginApi)]
        // private static extern IntPtr GetRenderEventFunc();

        // [DllImport(AdapterConstants.UnityPluginApi)]
        // private static extern void SetCurrentSessionHandle(IntPtr session);

        // [DllImport(AdapterConstants.UnityPluginApi)]
        // private static extern void GetCurrentFrameHandleAndStatus(ref IntPtr frame, ref HwArStatus status);

        // [DllImport(AdapterConstants.UnityPluginApi)]
        // private static extern void WaitForRenderingThreadFinished();

        // [DllImport(AdapterConstants.UnityPluginApi)]
        // private static extern void HwArSession_getSupportedSemanticMode(IntPtr session, ref int mode);                                           
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern HwArStatus HwArSession_setCloudServiceAuthInfo(IntPtr session,
        string authInfo);

    }
}
/*
HwArStatus

HwArSession_acquireNewAnchor(HwArSession *session, const HwArPose *pose, HwArAnchor **outAnchor)

使用pose创建一个持续跟踪的锚点。

HwArStatus

HwArSession_checkSupported(const HwArSession *session, const HwArConfig *config)

检查提供的配置在此设备上是否可用。

注意
该方法已废弃，无替代。

HwArStatus

HwArSession_configure(HwArSession *session, const HwArConfig *config)

配置会话。

HwArStatus

HwArSession_create(void *env, void *applicationContext, HwArSession **outSessionPointer)

创建一个新的HwArSession会话。

void

HwArSession_destroy(HwArSession *session)

释放HwArSession会话使用的资源。

void

HwArSession_getAllAnchors(const HwArSession *session, HwArAnchorList *outAnchorList)

获取所有的锚点。

void

HwArSession_getAllTrackables(const HwArSession *session, HwArTrackableType filterType, HwArTrackableList *outTrackableList)

获取所有指定类型的可跟踪对像集合。

void

HwArSession_getCameraConfig(const HwArSession *session, HwArCameraConfig *outCameraConfig)

返回当前Session对应的相机配置实例。

void

HwArSession_getSupportedSemanticMode(const HwArSession *session, int32_t *mode)

获取支持的语义模式。

HwArStatus

HwArSession_pause(HwArSession *session)

暂停会话，停止相机预览流，不清除平面和锚点数据，释放相机（否则其他应用无法使用相机服务），不会断开与服务端的连接。

HwArStatus

HwArSession_resume(HwArSession *session)

开始运行ARSession，或者在调用HwArSession_pause以后恢复HwArSession的运行状态。

void

HwArSession_setCameraTextureName(HwArSession *session, uint32_t textureId)

设置可用于存储相机预览流数据的openGL textureId。

void

HwArSession_setDisplayGeometry(HwArSession *session, int32_t rotation, int32_t width, int32_t height)

设置显示的高和宽（以像素为单位）。该高和宽是显示view的高和宽，如果不一致，会导致显示相机预览出错。

void

HwArSession_setEnvironmentTextureProbe(const HwArSession *session, const float *boundBox)

设置环境纹理探针。用于EnvironmentTexture。

void

HwArSession_setEnvironmentTextureUpdateMode(const HwArSession *session, HwArEnvironmentTextureUpdateMode mode)

设置环境纹理更新模式。

void

HwArSession_stop(HwArSession *session)

停止AR Engine，停止相机预览流，清除平面和锚点数据，并释放相机，终止本次会话。调用后，如果要再次启动，需要新建HwArSession。

HwArStatus

HwArSession_update(HwArSession *session, HwArFrame *outFrame)

更新AR Engine的计算结果，应用应在需要获取最新的数据时调用此接口，如相机发生移动以后，使用此接口可以获取新的锚点坐标、平面坐标、相机获取的图像帧等。

HwArStatus

HwArSession_setCloudServiceAuthInfo(const HwArSession *session, const char *authInfo)

设置云服务认证信息。该方法AR Engine Server 3.0.0.3后可用。
*/