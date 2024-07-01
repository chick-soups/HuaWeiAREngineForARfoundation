using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace UnityEngine.XR.AREngine
{
    public class HwArImageMetadata
    {

        // public void int[] GetAllKeys(){

        // }
        // public void GetConstEntry(int key){

        // }
        // [DllImport(Constants.ARENGINE_DLL_NAME)]
        // private static extern void HwArImageMetadata_getNdkCameraMetadata(IntPtr session, IntPtr imageMetadata, ref  IntPtr outNdkMetadata);



    }
}

/*
void

HwArImageMetadata_getNdkCameraMetadata(const HwArSession *session, const HwArImageMetadata *imageMetadata, const ACameraMetadata **outNdkMetadata)

获取当前相机图像的元数据。

void

HwArImageMetadata_release(HwArImageMetadata *metadata)

释放由HwArFrame_acquireCamera返回的HwArImageMetadata实例。
*/