using System;

namespace UnityEngine.XR.AREngine
{
    [Flags]
    public enum HwArEnableItem
    {
        HWAR_ENABLE_NULL = 0,
        HWAR_ENABLE_DEPTH = 1 << 0,
        HWAR_ENABLE_MASK = 1 << 1,
        HWAR_ENABLE_SCENE_MESH = 1 << 2,
        HWAR_ENABLE_CLOUD_AUGMENTED_IMAGE = 1 << 5,
        HWAR_ENABLE_HEALTH_DEVICE = 1 << 6,
        HWAR_ENABLE_CLOUD_OBJECT_RECOGNITION = 1 << 10,
    }
}