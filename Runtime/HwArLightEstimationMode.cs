using System;

namespace UnityEngine.XR.AREngine
{
    [Flags]
    public enum HwArLightEstimationMode
    {
        HwAr_LIGHT_ESTIMATION_MODE_DISABLED = 0,
        HwAr_LIGHT_ESTIMATION_MODE_AMBIENT_INTENSITY = 1,
        HwAr_LIGHT_ESTIMATION_MODE_ENVIRONMENT_LIGHTING = 2,
        HwAr_LIGHT_ESTIMATION_MODE_ENVIRONMENT_TEXTURE = 4,
    }
}