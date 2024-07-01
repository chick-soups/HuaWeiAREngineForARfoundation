namespace UnityEngine.XR.AREngine
{
    public class HwArAnchor
    {

    }
}
// HwArAnchor_detach(HwArSession *session, HwArAnchor *anchor)

// 通知AR Engine停止跟踪并解绑一个锚点。但是该函数并不会释放该锚点，这需要通过HwArAnchor_release单独来实现。

// void

// HwArAnchor_getPose(const HwArSession *session, const HwArAnchor *anchor, HwArPose *outPose)

// 获取指定锚点在世界坐标空间中的位姿信息。

// void

// HwArAnchorList_getSize(const HwArSession *session, const HwArAnchorList *anchorList, int32_t *outSize)

// 获取锚点列表中包含锚点的数量。

// void

// HwArAnchor_getTrackingState(const HwArSession *session, const HwArAnchor *anchor, HwArTrackingState *outTrackingState)

// 获取指定锚点位姿的追踪状态。

// void

// HwArAnchor_release(HwArAnchor *anchor)

// 释放指定锚点对象的内存。