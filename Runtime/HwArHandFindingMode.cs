using System;

namespace UnityEngine.XR.AREngine
{
    [Flags]
    public enum HwArHandFindingMode
    {
        HwAr_HAND_FINDING_MODE_DISABLED = 0x0,
        HwAr_HAND_FINDING_MODE_2D_ENABLED = 0x1,
        HwAr_HAND_FINDING_MODE_3D_ENABLED = 0x2
    }
}