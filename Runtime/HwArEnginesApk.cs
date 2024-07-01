using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngine.XR.AREngine
{
  public class HwArEnginesApk
  {
    public static bool isAREngineApkReady()
    {
#if UNITY_EDITOR
      return false;
#else
     return HwArEnginesApk_isAREngineApkReady(UnityAREngineAPI.UnityAREngine_GetJNIEnv(),
      UnityAREngineAPI.UnityAREngine_GetContext());
#endif

    }
    [DllImport(Constants.ARENGINE_DLL_NAME)]
    private static extern bool HwArEnginesApk_isAREngineApkReady(IntPtr env, IntPtr applicationContext);
  }

}

// bool

// HwArEnginesApk_isAREngineApkReady(void *env, void *applicationContext)

// 检查设备是否已安装AR Engine Service，并且版本与AR Engine SDK兼容。