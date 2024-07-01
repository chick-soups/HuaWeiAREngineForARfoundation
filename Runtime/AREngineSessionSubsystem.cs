using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.AREngine;
using Unity.Collections;
using System;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARSubsystems;
namespace UnityEngine.XR.AREngine
{
    public class AREngineSessionSubsystem : XRSessionSubsystem
    {
        public const string SubsystemId = "AREngine-Session";
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            XRSessionSubsystemDescriptor.RegisterDescriptor(new XRSessionSubsystemDescriptor.Cinfo
            {
                id = SubsystemId,
                providerType = typeof(AREngineProvider),
                subsystemTypeOverride = typeof(AREngineSessionSubsystem),
                supportsInstall = false,
                supportsMatchFrameRate = false
            });
#endif
        }

        public class AREngineProvider : Provider
        {

            public static HwArFrame lastFrame;
            public static HwArSession arSession;
            public static HwArConfig currentConfig;
            private ScreenOrientation screenOrientaion;
            public static uint glTexture;
            private Vector2Int screenDimension;

            private HwArConfig[] arConfigs;
            private Feature m_RequestedFeature;
            private Feature currentFeatures = Feature.None;


            public override void Destroy()
            {
                if (glTexture > 0)
                {
                    UnityAREngineAPI.UnityAREngine_DeleteGlTexture(glTexture);
                    glTexture = 0;
                }
                currentConfig?.Dispose();
                if (arConfigs != null)
                {
                    foreach (var config in arConfigs)
                    {
                        config.Dispose();
                    }
                }

                arConfigs = null;
                lastFrame?.Dispose();
                lastFrame = null;
                arSession?.Dispose();
                arSession = null;
                screenDimension = Vector2Int.zero;
                screenOrientaion = ScreenOrientation.Unknown;

            }
            //TODO:handle frame rate;
            public override int frameRate => base.frameRate;
            public override Promise<SessionAvailability> GetAvailabilityAsync()
            {
                bool isReady = HwArEnginesApk.isAREngineApkReady();
                SessionAvailability availability = isReady ? SessionAvailability.Supported | SessionAvailability.Installed : SessionAvailability.None;
                return Promise<SessionAvailability>.CreateResolvedPromise(availability);
            }
            public override NativeArray<ConfigurationDescriptor> GetConfigurationDescriptors(Allocator allocator)
            {
                if (arSession == null)
                {
                    return new NativeArray<ConfigurationDescriptor>(0, allocator);
                }
                if (arConfigs == null)
                {
                    arConfigs = new HwArConfig[2];
                    HwArConfig worldARConfig = new HwArConfig(arSession);
                    worldARConfig.SetArType(HwArType.HWAR_TYPE_WORLD);
                    worldARConfig.SetPlaneFindingMode(HwArPlaneFindingMode.HWAR_PLANE_FINDING_MODE_HORIZONTAL_AND_VERTICAL);
                    worldARConfig.SetSemanticMode(HwArSemanticMode.HwAr_SEMANTIC_PLANE);
                    worldARConfig.SetFocusMode(HwArFocusMode.HWAR_FOCUS_MODE_FIXED);
                    worldARConfig.SetCameraLensFacing(HwArCameraLensFacing.HWAR_CAMERA_FACING_REAR);
                    worldARConfig.SetLightingMode(HwArLightEstimationMode.HwAr_LIGHT_ESTIMATION_MODE_AMBIENT_INTENSITY);
                    worldARConfig.SetUpdateMode(HwArUpdateMode.HWAR_UPDATE_MODE_BLOCKING);
                    worldARConfig.SetPreviewSize(1280, 720);
                    worldARConfig.SetImageInputMode(HwArImageInputMode.NON_INPUT);
                    worldARConfig.SetEnableItem(HwArEnableItem.HWAR_ENABLE_DEPTH);



                    HwArConfig faceTrackingConfig = new HwArConfig(arSession);
                    faceTrackingConfig.SetArType(HwArType.HWAR_TYPE_FACE);
                    faceTrackingConfig.SetFocusMode(HwArFocusMode.HWAR_FOCUS_MODE_AUTO);
                    faceTrackingConfig.SetCameraLensFacing(HwArCameraLensFacing.HWAR_CAMERA_FACING_FRONT);
                    faceTrackingConfig.SetSemanticMode(HwArSemanticMode.HwAr_SEMANTIC_NONE);
                    faceTrackingConfig.SetLightingMode(HwArLightEstimationMode.HwAr_LIGHT_ESTIMATION_MODE_AMBIENT_INTENSITY);
                    faceTrackingConfig.SetEnableItem(HwArEnableItem.HWAR_ENABLE_DEPTH);
                    faceTrackingConfig.SetUpdateMode(HwArUpdateMode.HWAR_UPDATE_MODE_BLOCKING);
                    faceTrackingConfig.SetPreviewSize(1280, 720);
                    faceTrackingConfig.SetImageInputMode(HwArImageInputMode.NON_INPUT);
                    faceTrackingConfig.SetPlaneFindingMode(HwArPlaneFindingMode.HWAR_PLANE_FINDING_MODE_DISABLED);



                    arConfigs[0] = worldARConfig;
                    arConfigs[1] = faceTrackingConfig;
                }
                ConfigurationDescriptor[] _descriptors = new ConfigurationDescriptor[2];

                _descriptors = new ConfigurationDescriptor[2];
                _descriptors[0] = new ConfigurationDescriptor(arConfigs[0].Pointer,
                 Feature.AnyTrackingMode |
                   Feature.AutoFocus |
                   Feature.EnvironmentDepth |
                Feature.LightEstimationAmbientIntensity |
                Feature.PlaneTracking |
                Feature.PointCloud |
                Feature.Raycast |
                Feature.WorldFacingCamera
                , 1);


                _descriptors[1] = new ConfigurationDescriptor(arConfigs[1].Pointer,
                Feature.AnyTrackingMode |
                Feature.AutoFocus |
                Feature.EnvironmentDepth |
                Feature.FaceTracking |
                Feature.LightEstimationAmbientIntensity |
                Feature.PointCloud |
                Feature.Raycast |
                Feature.UserFacingCamera, 2);


                return new NativeArray<ConfigurationDescriptor>(_descriptors, allocator);


            }
            public override bool matchFrameRateEnabled => false;
            public override bool matchFrameRateRequested => false;

            public override IntPtr nativePtr
            {
                get
                {
                    if (arSession != null)
                    {
                        return arSession.Pointer;
                    }
                    else
                    {
                        return IntPtr.Zero;
                    }
                }
            }
            public override NotTrackingReason notTrackingReason => base.notTrackingReason;
            public override void OnApplicationPause()
            {
                arSession?.Pause();
                UnityAREngineAPI.UnityAREngine_DeleteGlTexture(glTexture);
                glTexture = 0;
            }
            public override void OnApplicationResume()
            {
                glTexture = UnityAREngineAPI.UnityAREngine_GenerateGLTexture();
                arSession?.SetCameraTextureName(glTexture);
                arSession?.Resume();
            }
            public override Feature requestedFeatures
            {
                get
                {
                    return ARConfigHandler.Instance.allFeatures;
                }
            }

            //TODO:handle requestedTrackingMode
            public override Feature requestedTrackingMode
            {
                get
                {
                    return m_RequestedFeature.TrackingModes();
                }
                set
                {
                    m_RequestedFeature = m_RequestedFeature.SetDifference(m_RequestedFeature.TrackingModes());
                    m_RequestedFeature.Union(requestedTrackingMode);
                    ARConfigHandler.Instance.AddFeature(this, m_RequestedFeature);

                }
            }
            public override void Reset()
            {
                Destroy();
                Start();
            }
            public override Guid sessionId => base.sessionId;
            public override void Start()
            {

                if (arSession == null)
                {
                    try
                    {
                        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
                        {
                            arSession = new HwArSession();
                            currentConfig = new HwArConfig(arSession);
                            SetConfigByFeature(currentConfig, requestedFeatures);
                            currentFeatures=requestedFeatures;
                            lastFrame = new HwArFrame(arSession);
                            
                            InitARSession(arSession, currentConfig, lastFrame);
                        }
                        else
                        {
                            PermissionCallbacks permissionCallbacks = new PermissionCallbacks();
                            permissionCallbacks.PermissionGranted += (value) =>
                            {

                                arSession = new HwArSession();
                                currentConfig = new HwArConfig(arSession);
                                SetConfigByFeature(currentConfig, requestedFeatures);
                                currentFeatures=requestedFeatures;
                                lastFrame = new HwArFrame(arSession);
                                InitARSession(arSession, currentConfig, lastFrame);
                            };
                            permissionCallbacks.PermissionDenied += (value) =>
                            {
                                //TODO: handle the denied case
                                Debug.LogError("Permission Denied");
                            };
                            Permission.RequestUserPermission(Permission.Camera, permissionCallbacks);
                        }

                    }
                    catch (ARUnavailableException e)
                    {
                        Debug.LogError("fail to create arSession" + e.Status);
                        arSession = null;
                    }


                }
                try
                {

                    //TODO:handle multithread rendering
                    if (glTexture <= 0)
                    {
                        glTexture = UnityAREngineAPI.UnityAREngine_GenerateGLTexture();
                        arSession.SetCameraTextureName(glTexture);
                    }
                    arSession.Resume();

                }
                catch (ARUnavailableException e)
                {
                    Debug.LogError("fail to resume arSession" + e.Status);
                }


            }
            public override void Stop()
            {
                try
                {
                    arSession.Pause();
                }
                catch (ARUnavailableException e)
                {
                    Debug.LogError("fail to pause arSession" + e.Status);
                }
                UnityAREngineAPI.UnityAREngine_DeleteGlTexture(glTexture);
                glTexture = 0;
                HandheldARInputDevice handheldARInputDevice = UnityEngine.InputSystem.InputSystem.GetDevice<HandheldARInputDevice>();
                if (handheldARInputDevice != null)
                {
                    InputSystem.InputSystem.DisableDevice(handheldARInputDevice);
                }
                ARConfigHandler.Instance.RemoveFeature(this);
            }
            public override TrackingState trackingState => base.trackingState;


            public override void Update(XRSessionUpdateParams updateParams, Configuration configuration)
            {

                if (currentConfig == null || currentConfig.Pointer != configuration.descriptor.identifier)
                {
                    for (int i = 0; i < arConfigs.Length; i++)
                    {
                        if (arConfigs[i].Pointer == configuration.descriptor.identifier)
                        {
                            currentConfig = arConfigs[i];
                            break;
                        }
                    }
                    if (currentConfig == null)
                    {
                        throw new ArgumentException("Invalid Configuration");
                    }
                }


                //update config according to features
                Feature differentFeature = currentFeatures.SymmetricDifference(configuration.features);
                if (differentFeature != Feature.None)
                {
                    HwArStatus hwArStatus = arSession.Pause();
                    Debug.Log("Update1 :" + hwArStatus);
                    currentFeatures = configuration.features;
                    Feature cameraFeature = differentFeature.Cameras();
                    if (cameraFeature != Feature.None)
                    {
                        Debug.Log("current feature:"+currentFeatures);

                         Debug.Log("Update4 :" + cameraFeature);

                        Destroy();
                        arSession = new HwArSession();
                        currentConfig = new HwArConfig(arSession);
                        SetConfigByFeature(currentConfig, requestedFeatures);
                        lastFrame = new HwArFrame(arSession);
                        InitARSession(arSession, currentConfig, lastFrame);
                        glTexture = UnityAREngineAPI.UnityAREngine_GenerateGLTexture();
                        arSession.SetCameraTextureName(glTexture);


                    }
                    else
                    {
                        SetConfigByFeature(currentConfig, currentFeatures);
                        Debug.Log("Update with Configuration:" + currentConfig + " features:" + configuration.features);
                        hwArStatus = arSession.Configure(currentConfig);
                        Debug.Log("Update2 :" + hwArStatus);

                    }

                    hwArStatus = arSession.Resume();
                    Debug.Log("Update3 :" + hwArStatus);
                    Debug.Log("Current config:" + currentConfig);

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
                        rotation = 0;
                        break;
                }
                return rotation; ;
            }

            private void InitARSession(HwArSession arSession, HwArConfig arconfig, HwArFrame hwArFrame)
            {

                arSession.Configure(arconfig);
                int displayRotation = GetAndroidDisplayRotation(Screen.orientation);
                arSession.SetDisplayGeometry(displayRotation, Screen.width, Screen.height);
                UnityAREngineAPI.UnityAREngine_SetReferences(arSession.Pointer, hwArFrame.Pointer);
                // the QueryEnabledStateCommand hasn't been handled yet,so enable the input manaually
                //TODO:hanle the QueryEnabledStateCommand
                HandheldARInputDevice handheldARInputDevice = UnityEngine.InputSystem.InputSystem.GetDevice<HandheldARInputDevice>();
                if (handheldARInputDevice != null)
                {
                    InputSystem.InputSystem.EnableDevice(handheldARInputDevice);
                }

            }

            private void SetConfigByFeature(HwArConfig arconfig, Feature features)
            {
                if (features.HasFlag(Feature.UserFacingCamera))
                {
                    arconfig.SetCameraLensFacing(HwArCameraLensFacing.HWAR_CAMERA_FACING_FRONT);
                }
                else
                {
                    arconfig.SetCameraLensFacing(HwArCameraLensFacing.HWAR_CAMERA_FACING_REAR);
                }
                if (features.HasFlag(Feature.AutoFocus))
                {
                    arconfig.SetFocusMode(HwArFocusMode.HWAR_FOCUS_MODE_AUTO);
                }
                else
                {
                    arconfig.SetFocusMode(HwArFocusMode.HWAR_FOCUS_MODE_FIXED);
                }

            }


        }

    }
}