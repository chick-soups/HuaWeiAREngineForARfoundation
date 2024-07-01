using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.AREngine
{
    public class AREngineCameraSubsystem : XRCameraSubsystem
    {
        public const string SubsystemId = "AREngine-Camera";
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            XRCameraSubsystemCinfo xRCameraSubsystemCinfo = new XRCameraSubsystemCinfo()
            {
                id = SubsystemId,
                providerType = typeof(AREngineCameraSubsystem.AREngineProvider),
                subsystemTypeOverride = typeof(AREngineCameraSubsystem),
                supportsAverageBrightness = true,
                supportsAverageColorTemperature = false,
                supportsColorCorrection = true,
                supportsDisplayMatrix = true,
                supportsProjectionMatrix = true,
                supportsTimestamp = true,
                supportsCameraConfigurations = true,
                supportsCameraImage = true,
                supportsAverageIntensityInLumens = false,
                supportsFocusModes = true,
                supportsFaceTrackingAmbientIntensityLightEstimation = true,
                supportsFaceTrackingHDRLightEstimation = false,
                supportsWorldTrackingAmbientIntensityLightEstimation = true,
                supportsWorldTrackingHDRLightEstimation = true,
                supportsCameraGrain = false,
            };
            if (XRCameraSubsystem.Register(xRCameraSubsystemCinfo) == false)
            {
                throw new System.Exception($"fail to register {SubsystemId} subsystem.");
            }
        }

        public class AREngineProvider : Provider
        {
            public const string BEFORE_OPAQUES_SHADER_NAME = "Unlit/AREngineBackground";
            private bool isUVInited;

            private const int FACING_BACK_CAMERA_ROTATION = 90;
            private const int FACING_FRONT_CAMERA_ROTATION = 270;

            private Material m_beforeOpaquesMaterial;
            private ScreenOrientation screenOrientaion;
            private Vector2Int screenDimension;
            private XRSupportedCameraBackgroundRenderingMode m_RequestedBackgroundRenderingMode;
            private Feature m_RequestedFeatures;
            private Feature lastFrameCamera=Feature.None;


            //TODO:handle the autoFocus status
            public override bool autoFocusEnabled
            {
                get
                {
                    HwArConfig hwArConfig = AREngineSessionSubsystem.AREngineProvider.currentConfig;
                    if (hwArConfig == null || hwArConfig.GetFocusMode() == HwArFocusMode.HWAR_FOCUS_MODE_FIXED)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            }
            public override bool autoFocusRequested
            {
                get
                {
                    return m_RequestedFeatures.HasFlag(Feature.AutoFocus);
                }
                set
                {
                   if(value){
                    m_RequestedFeatures= m_RequestedFeatures.Union(Feature.AutoFocus);
                   }else{
                    m_RequestedFeatures= m_RequestedFeatures.SetDifference(Feature.AutoFocus);
                   }
                   Debug.Log(SubsystemId+"autoFocusRequested "+value+" hasflag"+m_RequestedFeatures.HasFlag(Feature.AutoFocus));
                   
                    ARConfigHandler.Instance.AddFeature(this, m_RequestedFeatures);
                }
            }
            public override Material cameraMaterial
            {
                get
                {
                    switch (currentBackgroundRenderingMode)
                    {
                        case XRCameraBackgroundRenderingMode.BeforeOpaques:
                            if (m_beforeOpaquesMaterial == null)
                            {
                                m_beforeOpaquesMaterial = CreateCameraMaterial(BEFORE_OPAQUES_SHADER_NAME);
                            }
                            return m_beforeOpaquesMaterial;
                        default:
                            throw new System.NotImplementedException($"{currentBackgroundRenderingMode} is not support");
                    }
                }
            }
            public override XRCpuImage.Api cpuImageApi
            {
                get
                {
                    return AREngineCpuImageApi.Instance;
                }
            }
            public override XRCameraBackgroundRenderingMode currentBackgroundRenderingMode
            {
                get
                {
                    switch (m_RequestedBackgroundRenderingMode)
                    {
                        case XRSupportedCameraBackgroundRenderingMode.Any:
                        case XRSupportedCameraBackgroundRenderingMode.BeforeOpaques:
                            return XRCameraBackgroundRenderingMode.BeforeOpaques;

                        case XRSupportedCameraBackgroundRenderingMode.AfterOpaques:
                            return XRCameraBackgroundRenderingMode.AfterOpaques;

                        case XRSupportedCameraBackgroundRenderingMode.None:
                        default:
                            return XRCameraBackgroundRenderingMode.None;
                    }
                }

            }
            public override Feature currentCamera
            {

                 get
                {
                    HwArConfig hwArConfig = AREngineSessionSubsystem.AREngineProvider.currentConfig;
                    if (hwArConfig == null || hwArConfig.GetCameraLensFacing() == HwArCameraLensFacing.HWAR_CAMERA_FACING_REAR)
                    {
                        return Feature.WorldFacingCamera;
                    }
                    else
                    {
                        return Feature.UserFacingCamera;
                    }

                }
            }
            //TODO:currentConfiguration 
            public override XRCameraConfiguration? currentConfiguration { get => base.currentConfiguration; set => base.currentConfiguration = value; }
            //TODO:currentLightEstimation
            public override Feature currentLightEstimation => base.currentLightEstimation;

            public override void Destroy()
            {
                base.Destroy();
            }
            public override NativeArray<XRCameraConfiguration> GetConfigurations(XRCameraConfiguration defaultCameraConfiguration, Allocator allocator)
            {
                return base.GetConfigurations(defaultCameraConfiguration, allocator);
            }
            public override void GetMaterialKeywords(out List<string> enabledKeywords, out List<string> disabledKeywords)
            {
                base.GetMaterialKeywords(out enabledKeywords, out disabledKeywords);
            }
            public override NativeArray<XRTextureDescriptor> GetTextureDescriptors(XRTextureDescriptor defaultDescriptor, Allocator allocator)
            {
                XRTextureDescriptor[] xrTextureDescriptors = new XRTextureDescriptor[1];
                if (Utility.GLTextureName != 0)
                {

                    xrTextureDescriptors[0] = new XRTextureDescriptor((IntPtr)Utility.GLTextureName
                    , 0, 0, 0, TextureFormat.RGBA32, Shader.PropertyToID("_MainTex"), 1, Rendering.TextureDimension.Tex2D);
                }
                else
                {
                    xrTextureDescriptors = new XRTextureDescriptor[0];
                }

                return new NativeArray<XRTextureDescriptor>(xrTextureDescriptors, allocator);
            }

            public override bool permissionGranted
            {
                get
                {
                    return Permission.HasUserAuthorizedPermission(Permission.Camera);
                }
            }
            public override XRSupportedCameraBackgroundRenderingMode requestedBackgroundRenderingMode
            {
                get
                {
                    return m_RequestedBackgroundRenderingMode;
                }
                set
                {
                    m_RequestedBackgroundRenderingMode = value;
                }
            }
            public override Feature requestedCamera
            {
                get
                {
                    return m_RequestedFeatures.Cameras();
                }
                set
                {
                    Feature  requested = value.Cameras();
                     m_RequestedFeatures= m_RequestedFeatures.SetDifference(Feature.AnyCamera);
                    m_RequestedFeatures= m_RequestedFeatures.Union(requested);
                    ARConfigHandler.Instance.AddFeature(this, m_RequestedFeatures);
                    Debug.Log(" requestedCamera:" + m_RequestedFeatures);


                }
            }
            public override Feature requestedLightEstimation
            {
                get
                {
                    return m_RequestedFeatures.LightEstimation();
                }
                set
                {
                    Feature requested = value.LightEstimation();
                    m_RequestedFeatures= m_RequestedFeatures.SetDifference(Feature.AnyLightEstimation);
                    m_RequestedFeatures= m_RequestedFeatures.Union(requested);
                    ARConfigHandler.Instance.AddFeature(this, m_RequestedFeatures);
                }
            }
    


        public override void Start()
            {
                Debug.Log("AREngineCameraSubsystem " + m_RequestedFeatures);
                ARConfigHandler.Instance.AddFeature(this, m_RequestedFeatures);
            }
            public override void Stop()
            {
                ARConfigHandler.Instance.RemoveFeature(this);
            }
            public override XRSupportedCameraBackgroundRenderingMode supportedBackgroundRenderingMode => XRSupportedCameraBackgroundRenderingMode.BeforeOpaques;
            public override bool TryAcquireLatestCpuImage(out XRCpuImage.Cinfo cameraImageCinfo)
            {
                try
                {
                    HwArFrame hwArFrame = Utility.LastARFrame;
                    HwArCamera arCamera = hwArFrame.AcquireCamera();
                    bool isSuccess=false;
                    if (arCamera.GetTrackingState() == HwArTrackingState.HWAR_TRACKING_STATE_TRACKING)
                    {
                        HwArImage hwArImage = hwArFrame.AcquireCameraImage();
                        AImage aImage = hwArImage.GetNdkImage();
                        aImage=hwArImage.GetNdkImage();
                        int width = aImage.GetWidth();
                        int height = aImage.GetHeight();
                        byte[] bytes = TextureUtils.GetBytesFromImageAsType(aImage,TextureUtils.YuvType.NV12);
                        Vector2Int dimensions = new Vector2Int(width, height);
                        int planeCount = 3;
                        XRCpuImage.Format format = XRCpuImage.Format.AndroidYuv420_888;
                        long timeStamp = hwArFrame.GetTimestamp();
                         GCHandle gCHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                        IntPtr intPtr =  GCHandle.ToIntPtr(gCHandle);
                        arCamera.Dispose();
                        hwArImage.Dispose();
                        cameraImageCinfo = new XRCpuImage.Cinfo((int)intPtr, dimensions, planeCount, timeStamp, format);
                        isSuccess=true;
                    }
                    else
                    {
                        cameraImageCinfo = new XRCpuImage.Cinfo();
                    }
                    arCamera.Dispose();
                    return isSuccess;

                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    Debug.LogError(e.StackTrace);
                    cameraImageCinfo = new XRCpuImage.Cinfo();
                    return false;
                }

            }
            public override bool TryGetFrame(XRCameraParams cameraParams, out XRCameraFrame cameraFrame)
            {
                HwArFrame arFrame = Utility.LastARFrame;
                if (arFrame != null)
                {
                    HwArStatus status = Utility.ARSession.Update(arFrame);
                    if (status == HwArStatus.HWAR_SUCCESS)
                    {

                        XRCameraFrameProperties properties = XRCameraFrameProperties.Timestamp;
                        long timestamp = arFrame.GetTimestamp();

                        UpdateDisplayGeometry(cameraParams);
                        bool isChanged = arFrame.GetDisplayGeometryChanged();
                        Matrix4x4 displayMatrix = Matrix4x4.identity;
                        if (isChanged || isUVInited == false)
                        {
                             
                            properties |= XRCameraFrameProperties.DisplayMatrix;
                            int androidDisplayRotation = GetAndroidDisplayRotation(cameraParams.screenOrientation);
                            int displayRotation = GetDisplayRotation(currentCamera, androidDisplayRotation);
                            bool shouldMirror=currentCamera.HasFlag(Feature.UserFacingCamera);
                            displayMatrix = GetDisplayMatrix(Utility.ARSession, displayRotation,shouldMirror);
                            isUVInited = true;
                        }


                        HwArCamera arCamera = arFrame.AcquireCamera();

                        HwArTrackingState hwArTrackingState = arCamera.GetTrackingState();
                        TrackingState trackingState = TrackingState.None;
                        switch (hwArTrackingState)
                        {
                            case HwArTrackingState.HWAR_TRACKING_STATE_PAUSED:
                                trackingState = TrackingState.Limited;
                                break;
                            case HwArTrackingState.HWAR_TRACKING_STATE_STOPPED:
                                trackingState = TrackingState.None;
                                break;
                            case HwArTrackingState.HWAR_TRACKING_STATE_TRACKING:
                                trackingState = TrackingState.Tracking;
                                break;
                            default:
                                trackingState = TrackingState.None;
                                break;
                        }

                        properties |= XRCameraFrameProperties.ProjectionMatrix;

                        Matrix4x4 projectionMatrix = arCamera.GetProjectionMatrix(Camera.main.nearClipPlane, Camera.main.farClipPlane);
                        //Matrix4x4 projectionMatrix =Matrix4x4.identity;

                        arCamera.Dispose();



                        cameraFrame = new XRCameraFrame(timestamp, 0, 0, Color.white,
                    projectionMatrix, displayMatrix, trackingState, arFrame.Pointer, properties, 0, 0, 0, 0, Color.white, Vector3.zero, new SphericalHarmonicsL2(), new XRTextureDescriptor(), 0f);


                        return true;
                    }
                    else
                    {
                        Debug.LogError("fail to update session.Status:" + status);
                    }

                }
                cameraFrame = new XRCameraFrame();
                return false;
            }
            public override bool TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics)
            {
                HwArFrame arFrame = Utility.LastARFrame;
                if (arFrame != null)
                {
                    using (HwArCamera arCamera = Utility.LastARFrame.AcquireCamera())
                    {
                        using (HwArCameraIntrinsics hwArCameraIntrinsics = arCamera.GetImageIntrinsics())
                        {
                            Vector2 focalLength = hwArCameraIntrinsics.GetFocalLength();
                            Vector2 principalPoint = hwArCameraIntrinsics.GetPrincipalPoint();
                            Vector2Int diamensions = hwArCameraIntrinsics.GetImageDimensions();
                            cameraIntrinsics = new XRCameraIntrinsics(focalLength, principalPoint, diamensions);
                        }

                    }
                    return true;
                }
                else
                {
                    cameraIntrinsics = new XRCameraIntrinsics();
                    return false;
                }

            }
            private int GetAndroidDisplayRotation(ScreenOrientation screenOrientation)
            {
                int rotation = 0;
                switch (screenOrientaion)
                {
                    case ScreenOrientation.Portrait:
                        rotation = 0;
                        break;
                    case ScreenOrientation.LandscapeLeft:
                        rotation = 1;
                        break;
                    case ScreenOrientation.PortraitUpsideDown:
                        rotation = 2;
                        break;
                    case ScreenOrientation.LandscapeRight:
                        rotation = 3;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return rotation;
            }
            private Matrix4x4 GetDisplayMatrix(HwArSession arSession, int displayRotation,bool shouldMirror)
            {
                float radians = Mathf.Deg2Rad * displayRotation;
                //right hand,around the z axis clockwise
                float cos = Mathf.Cos(-radians);
                float sin = Mathf.Sin(-radians);
                Matrix4x4 rotationMatrix = new Matrix4x4(
                new Vector4(cos, sin, 0, 0),
                new Vector4(-sin, cos, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1));

                HwArCameraConfig cameraConfig = arSession.GetCameraConfig();
                Vector2Int gpuTextureSize = cameraConfig.GetTextureDimensions();
                cameraConfig.Dispose();
                int screenWidth = Screen.width;
                int screenHeight = Screen.height;
                bool isSwitch = false;
                if (screenWidth < screenHeight)
                {
                    int temp = screenWidth;
                    screenWidth = screenHeight;
                    screenHeight = temp;
                    isSwitch = true;
                }
                float ratioScreen = screenWidth / (float)screenHeight;
                float ratioGpu = gpuTextureSize.x / (float)gpuTextureSize.y;
                Vector3 scale = Vector3.one;
                if (ratioScreen > ratioGpu)
                {
                    if (isSwitch)
                    {
                        scale.x = gpuTextureSize.x * screenHeight / (float)screenWidth / (float)gpuTextureSize.y;
                    }
                    else
                    {
                        scale.y = gpuTextureSize.x * screenHeight / (float)screenWidth / (float)gpuTextureSize.y;
                    }

                }
                else if (ratioScreen < ratioGpu)
                {
                    if (isSwitch)
                    {
                        scale.y = gpuTextureSize.y * screenWidth / (float)screenHeight / (float)gpuTextureSize.x;
                    }
                    else
                    {
                        scale.x = gpuTextureSize.y * screenWidth / (float)screenHeight / (float)gpuTextureSize.x;
                    }
                }

                Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotationMatrix.rotation, scale);
                Vector4 center = new Vector4(0.5f, 0.5f, 0, 1);
                Vector4 rotatedCenter = m * center;
                Vector4 translation = center - rotatedCenter;
                m=  Matrix4x4.TRS(translation, rotationMatrix.rotation, scale);
                
                if (shouldMirror)
                {
                    Matrix4x4 mirror = new Matrix4x4(new Vector4(1,0,0,0),
                    new Vector4(0,-1,0,0),
                    new Vector4(0,0,1,0),
                    new Vector4(0,1,0,1));
                    m = mirror * m;
                    
                }
                

                return m;
            }
            private int GetDisplayRotation(Feature camera, int androidDisplayRotation)
            {
                if (androidDisplayRotation > 3 || androidDisplayRotation < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                int sensorOrientationDegrees=FACING_BACK_CAMERA_ROTATION;
                int sign=-1;
                if (camera.HasFlag(Feature.UserFacingCamera))
                {
                    sensorOrientationDegrees = FACING_FRONT_CAMERA_ROTATION;
                    sign = 1;
                }
                 int displayRotation = androidDisplayRotation * 90;
                displayRotation=(sensorOrientationDegrees-displayRotation*sign+360)%360;
                return displayRotation;
            }

            private void UpdateDisplayGeometry(XRCameraParams cameraParams)
            {
                if (screenOrientaion != cameraParams.screenOrientation || screenDimension.x != cameraParams.screenWidth || screenDimension.y != cameraParams.screenHeight)
                {

                    screenOrientaion = cameraParams.screenOrientation;
                    int rotation = GetAndroidDisplayRotation(screenOrientaion);

                    screenDimension = new Vector2Int((int)cameraParams.screenWidth, (int)cameraParams.screenHeight);
                    Utility.ARSession.SetDisplayGeometry(rotation, screenDimension.x, screenDimension.y);
                }
            }
        }
    }
}