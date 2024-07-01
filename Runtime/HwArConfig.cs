
using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.AREngine
{
    public class HwArConfig : IDisposable
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
        public HwArConfig(HwArSession arSession)
        {
            this.arSession = arSession;
            HwArConfig_create(arSession.Pointer, ref pointer);
            SetArType(HwArType.HWAR_TYPE_WORLD);
            SetCameraLensFacing(HwArCameraLensFacing.HWAR_CAMERA_FACING_REAR);
            SetEnableItem(HwArEnableItem.HWAR_ENABLE_DEPTH);
            SetFocusMode(HwArFocusMode.HWAR_FOCUS_MODE_FIXED);
            SetHandFindingMode(HwArHandFindingMode.HwAr_HAND_FINDING_MODE_DISABLED);
            SetImageInputMode(HwArImageInputMode.NON_INPUT);
            SetLightingMode(HwArLightEstimationMode.HwAr_LIGHT_ESTIMATION_MODE_AMBIENT_INTENSITY);
            SetPlaneFindingMode(HwArPlaneFindingMode.HWAR_PLANE_FINDING_MODE_DISABLED);
            SetPowerMode(HwArPowerMode.HWAR_POWER_MODE_NORMAL);
            SetPreviewSize(1280, 720);
            SetSemanticMode(HwArSemanticMode.HwAr_SEMANTIC_NONE);
            SetUpdateMode(HwArUpdateMode.HWAR_UPDATE_MODE_BLOCKING);
        }
        public HwArConfig(HwArSession arSession, IntPtr pointer)
        {
            this.arSession = arSession;
            this.pointer = pointer;
        }


        public HwArType GetArType()
        {
            int arType = 0;
            HwArConfig_getArType(arSession.Pointer, pointer, ref arType);
            return (HwArType)arType;
        }

        public HwArCameraLensFacing GetCameraLensFacing()
        {
            int facing = 0;
            HwArConfig_getCameraLensFacing(arSession.Pointer, pointer, ref facing);
            return (HwArCameraLensFacing)facing;
        }

        public HwArEnableItem GetEnableItem()
        {
            UInt64 enableItem = 0;
            HwArConfig_getEnableItem(arSession.Pointer, pointer, ref enableItem);
            return (HwArEnableItem)enableItem;
        }

        public HwArFocusMode GetFocusMode()
        {
            int focusMode = 0;
            HwArConfig_getFocusMode(arSession.Pointer, pointer, ref focusMode);
            return (HwArFocusMode)focusMode;
        }

        public HwArHandFindingMode GetHandFindingMode()
        {
            int handFindingMode = 0;
            HwArConfig_getHandFindingMode(arSession.Pointer, pointer, ref handFindingMode);
            return (HwArHandFindingMode)handFindingMode;
        }
        public HwArImageInputMode GetImageInputMode()
        {
            int inputMode = 0;
            HwArConfig_getImageInputMode(arSession.Pointer, pointer, ref inputMode);
            return (HwArImageInputMode)inputMode;
        }

        public HwArLightEstimationMode getLightingMode()
        {
            int mode = 0;
            HwArConfig_getLightingMode(arSession.Pointer, pointer, ref mode);
            return (HwArLightEstimationMode)mode;
        }

        public UInt64 getMaxMapSize()
        {
            UInt64 maxMapSize = 0;
            HwArConfig_getMaxMapSize(arSession.Pointer, pointer, ref maxMapSize);
            return maxMapSize;
        }

        public HwArPlaneFindingMode GetPlaneFindingMode()
        {
            int planeFindingMode = 0;
            HwArConfig_getPlaneFindingMode(arSession.Pointer, pointer, ref planeFindingMode);
            return (HwArPlaneFindingMode)planeFindingMode;
        }

        public HwArPowerMode GetPowerMode()
        {
            int powerMode = 0;
            HwArConfig_getPowerMode(arSession.Pointer, pointer, ref powerMode);
            return (HwArPowerMode)powerMode;
        }

        public HwArSemanticMode GetSemanticMode()
        {
            int mode = 0;
            HwArConfig_getSemanticMode(arSession.Pointer, pointer, ref mode);
            return (HwArSemanticMode)mode;
        }

        public HwArUpdateMode GetUpdateMode()
        {
            int updateMode = 0;
            HwArConfig_getUpdateMode(arSession.Pointer, pointer, ref updateMode);
            return (HwArUpdateMode)updateMode;
        }

        // public void SetAugmentedImageDatabase(HwArAugmentedImageDatabase database)
        // {
        //     HwArConfig_setAugmentedImageDatabase(arSession.Pointer, pointer, database.Pointer);
        // }

        public void SetArType(HwArType type)
        {
            HwArConfig_setArType(arSession.Pointer, pointer, (int)type);
        }

        public void SetCameraLensFacing(HwArCameraLensFacing facing)
        {
            HwArConfig_setCameraLensFacing(arSession.Pointer, pointer, (int)facing);
        }
        public void SetEnableItem(HwArEnableItem item)
        {
            HwArConfig_setEnableItem(arSession.Pointer, pointer, (UInt64)item);
        }

        public void SetFocusMode(HwArFocusMode focusMode)
        {
            HwArConfig_setFocusMode(arSession.Pointer, pointer, (int)focusMode);
        }

        public void SetHandFindingMode(HwArHandFindingMode mode)
        {
            HwArConfig_setHandFindingMode(arSession.Pointer, pointer, (int)mode);
        }

        public void SetImageInputMode(HwArImageInputMode mode)
        {
            HwArConfig_setImageInputMode(arSession.Pointer, pointer, (int)mode);
        }

        public void SetLightingMode(HwArLightEstimationMode mode)
        {
            HwArConfig_setLightingMode(arSession.Pointer, pointer, (int)mode);
        }
        public void SetMaxMapSize(UInt64 maxMapSize)
        {
            HwArConfig_setMaxMapSize(arSession.Pointer, pointer, maxMapSize);
        }

        public void SetPlaneFindingMode(HwArPlaneFindingMode planeFindingMode)
        {
            HwArConfig_setPlaneFindingMode(arSession.Pointer, pointer, (int)planeFindingMode);
        }

        public void SetPowerMode(HwArPowerMode powerMode)
        {
            HwArConfig_setPowerMode(arSession.Pointer, pointer, (int)powerMode);
        }

        public void SetSemanticMode(HwArSemanticMode mode)
        {
            HwArConfig_setSemanticMode(arSession.Pointer, pointer, (int)mode);
        }

        public void SetUpdateMode(HwArUpdateMode updateMode)
        {
            HwArConfig_setUpdateMode(arSession.Pointer, pointer, (int)updateMode);
        }

        public void SetPreviewSize(uint width, uint height)
        {
            HwArConfig_setPreviewSize(arSession.Pointer, pointer, width, height);
        }

        public override string ToString()
        {
            return String.Format(@"HwArConfig:
             pointer={0},
             ArType={1},
             CameraLensFacing={2},
             EnableItem={3},
             FocusMode={4},
             HandFindingMode={5},
             ImageInputMode={6},
             LightingMode={7},
             PlaneFindingMode={8},
             PowerMode={9},
             SemanticMode={10},
             UpdateMode={11}", pointer, GetArType(), GetCameraLensFacing(), GetEnableItem(), GetFocusMode(), GetHandFindingMode(), GetImageInputMode(), getLightingMode(), GetPlaneFindingMode(), GetPowerMode(), GetSemanticMode(), GetUpdateMode());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }




        protected void Dispose(bool disposing)
        {

            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                HwArConfig_destroy(pointer);
            }

            pointer = IntPtr.Zero;
            disposed = true;
        }

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_create(IntPtr session, ref IntPtr outConfig);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_destroy(IntPtr config);
        // [DllImport(Constants.ARENGINE_DLL_NAME)]
        // private static extern void HwArConfig_getAugmentedImageDatabase(IntPtr session, IntPtr config, HwArAugmentedImageDatabase* outAugmentedImageDatabase);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_getArType(IntPtr session, IntPtr config, ref int type);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_getCameraLensFacing(IntPtr session, IntPtr config, ref int facing);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_getEnableItem(IntPtr session, IntPtr config, ref UInt64 item);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_getFocusMode(IntPtr session, IntPtr config, ref int focusMode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_getHandFindingMode(IntPtr session, IntPtr config, ref int mode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_getImageInputMode(IntPtr session, IntPtr config, ref int mode);

        // [DllImport(Constants.ARENGINE_DLL_NAME)]

        // private static extern int HwArConfig_getInputNativeWindows(IntPtr session, IntPtr config, int32_t count, int64_t** windows);

        // [DllImport(Constants.ARENGINE_DLL_NAME)]

        // private static extern int HwArConfig_getInputNativeWindowTypes(IntPtr session, IntPtr config, int32_t count, HwArNativeWindowType** types);
        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_getLightingMode(IntPtr session, IntPtr config, ref int lightEstimationMode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_getMaxMapSize(IntPtr session, IntPtr config, ref UInt64 maxMapSize);

        [DllImport(Constants.ARENGINE_DLL_NAME)]
        private static extern void HwArConfig_getPlaneFindingMode(IntPtr session, IntPtr config, ref int planeFindingMode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_getPowerMode(IntPtr session, IntPtr config, ref int powerMode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_getSemanticMode(IntPtr session, IntPtr config, ref int mode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_getUpdateMode(IntPtr session, IntPtr config, ref int updateMode);

        // [DllImport(Constants.ARENGINE_DLL_NAME)]

        // private static extern void HwArConfig_setAugmentedImageDatabase(IntPtr session, IntPtr config, const HwArAugmentedImageDatabase* augmentedImageDatabase);


        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setArType(IntPtr session, IntPtr config, int type);


        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setCameraLensFacing(IntPtr session, IntPtr config, int facing);


        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setEnableItem(IntPtr session, IntPtr config, UInt64 item);


        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setFocusMode(IntPtr session, IntPtr config, int focusMode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setHandFindingMode(IntPtr session, IntPtr config, int mode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setImageInputMode(IntPtr session, IntPtr config, int mode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setLightingMode(IntPtr session, IntPtr config, int lightEstimationMode);

        private static extern void HwArConfig_setMaxMapSize(IntPtr session, IntPtr config, UInt64 maxMapSize);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setPlaneFindingMode(IntPtr session, IntPtr config, int planeFindingMode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setPowerMode(IntPtr session, IntPtr config, int powerMode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setSemanticMode(IntPtr session, IntPtr config, int mode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setUpdateMode(IntPtr session, IntPtr config, int updateMode);

        [DllImport(Constants.ARENGINE_DLL_NAME)]

        private static extern void HwArConfig_setPreviewSize(IntPtr session, IntPtr config, uint width, uint heigth);



    }
}
/*
void

HwArConfig_create(const HwArSession *session, HwArConfig **outConfig)

创建一个拥有合理默认配置的配置对象。

void

HwArConfig_destroy(HwArConfig *config)

释放指定的配置对象的内存空间。

void

HwArConfig_getAugmentedImageDatabase(const HwArSession *session, const HwArConfig *config, HwArAugmentedImageDatabase *outAugmentedImageDatabase)

获取当前配置的图像数据库。

void

HwArConfig_getArType(const HwArSession *session, const HwArConfig *config, HwArType *type)

获取当前使用的AR能力类型。

void

HwArConfig_getCameraLensFacing(const HwArSession *session, const HwArConfig *config, HwArCameraLensFacing *facing)

获取当前配置的摄像头类型（前置或后置）。

void

HwArConfig_getEnableItem(const HwArSession *session, const HwArConfig *config, uint64_t *item)

获取当前配置的AR能力项。

void

HwArConfig_getFocusMode(const HwArSession *session, const HwArConfig *config, HwArFocusMode *focusMode)

获取当前配置的相机对焦模式。

void

HwArConfig_getHandFindingMode(const HwArSession *session, HwArConfig *config, HwArHandFindingMode *mode)

获取当前配置的手部识别模式。

void

HwArConfig_getImageInputMode(const HwArSession *session, const HwArConfig *config, HwArImageInputMode *mode)

获取当前配置输入给Server的图像输入模式。

int32_t

HwArConfig_getInputNativeWindows(const HwArSession *session, const HwArConfig *config, int32_t count, int64_t **windows)

从Server获取待输入到Native Window的Surface。

int32_t

HwArConfig_getInputNativeWindowTypes(const HwArSession *session, const HwArConfig *config, int32_t count, HwArNativeWindowType **types)

从Server获取待输入到本地窗口的预览画面类型数。

void

HwArConfig_getLightingMode(const HwArSession *session, const HwArConfig *config, int32_t *lightEstimationMode)

获取当前配置的光照估计模式。

void

HwArConfig_getLightEstimationMode(const HwArSession *session, const HwArConfig *config, HwArLightEstimationMode *lightEstimationMode)

获取当前配置的光照估计模式。

注意
该方法已废弃，请使用HwArConfig_getLightingMode代替。

void

HwArConfig_getMaxMapSize(const HwArSession *session, const HwArConfig *config, uint64_t *maxMapSize)

获取目前生效的地图数据最大使用内存大小，该接口在ARSession.configure后生效，若配置的地图数据最大使用内存范围不合法，则配置最接近用户配置的有效值，默认内存大小800MB。

void

HwArConfig_getPlaneFindingMode(const HwArSession *session, const HwArConfig *config, HwArPlaneFindingMode *planeFindingMode)

获取当前配置的平面识别模式。

void

HwArConfig_getPowerMode(const HwArSession *session, const HwArConfig *config, HwArPowerMode *powerMode)

获取当前配置的功耗模式。

void

HwArConfig_getSemanticMode(const HwArSession *session, const HwArConfig *config, int32_t *mode)

获取当前配置的语义识别模式。

void

HwArConfig_getUpdateMode(const HwArSession *session, const HwArConfig *config, HwArUpdateMode *updateMode)

获取当前配置的HwArSession_update更新模式。

void

HwArConfig_setAugmentedImageDatabase(const HwArSession *session, HwArConfig *config, const HwArAugmentedImageDatabase *augmentedImageDatabase)

设置图像识别能力的图像数据库。

void

HwArConfig_setArType(const HwArSession *session, HwArConfig *config, HwArType type)

设置当前要使用的AR能力类型。

void

HwArConfig_setCameraLensFacing(const HwArSession *session, HwArConfig *config, HwArCameraLensFacing facing)

设置当前所需的前置或后置摄像头。

void

HwArConfig_setEnableItem(const HwArSession *session, HwArConfig *config, uint64_t item)

设置当前所需的AR Engine对应使能项。

void

HwArConfig_setFocusMode(const HwArSession *session, HwArConfig *config, HwArFocusMode focusMode)

配置当前所需的相机对焦模式。

void

HwArConfig_setHandFindingMode(const HwArSession *session, HwArConfig *config, HwArHandFindingMode mode)

设置当前所需的手部识别模式。

void

HwArConfig_setImageInputMode(const HwArSession *session, HwArConfig *config, HwArImageInputMode mode)

配置当前所需的输入给Server的图像输入模式。

void

HwArConfig_setLightingMode(const HwArSession *session, HwArConfig *config, int32_t lightEstimationMode)

配置当前所需的光照估计模式。

void

HwArConfig_setLightEstimationMode(const HwArSession *session, HwArConfig *config, HwArLightEstimationMode lightEstimationMode)

配置当前所需的光照估计模式。

注意
该方法已废弃，请使用HwArConfig_setLightingMode代替。

void

HwArConfig_setMaxMapSize(const HwArSession *session, HwArConfig *config, uint64_t maxMapSize)

设置地图数据最大使用内存大小。

void

HwArConfig_setPlaneFindingMode(const HwArSession *session, HwArConfig *config, HwArPlaneFindingMode planeFindingMode)

设置当前所需的平面识别模式。

void

HwArConfig_setPowerMode(const HwArSession *session, HwArConfig *config, HwArPowerMode powerMode)

设置当前所需的功耗模式。

void

HwArConfig_setSemanticMode(const HwArSession *session, const HwArConfig *config, int32_t *mode)

设置当前所需的语义模式。

void

HwArConfig_setUpdateMode(const HwArSession *session, HwArConfig *config, HwArUpdateMode updateMode)

设置每一帧更新的更新模式。

void

HwArConfig_setPreviewSize(const HwArSession *session, HwArConfig *config, uint32_t width, uint32_t heigth)

设置预览画面尺寸。
*/