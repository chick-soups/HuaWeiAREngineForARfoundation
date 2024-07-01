using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace UnityEngine.XR.AREngine
{
    public class UnityAREngineAPI
    {
        [DllImport(Constants.UNITY_ARENGINE_DLL_NAME)]
        public static extern IntPtr UnityAREngine_GetJavaVM();
        [DllImport(Constants.UNITY_ARENGINE_DLL_NAME)]
        public static extern IntPtr UnityAREngine_GetJNIEnv();
        [DllImport(Constants.UNITY_ARENGINE_DLL_NAME)]
        public static extern IntPtr UnityAREngine_GetContext();
        [DllImport(Constants.UNITY_ARENGINE_DLL_NAME)]
        public static extern uint UnityAREngine_GenerateGLTexture();
        [DllImport(Constants.UNITY_ARENGINE_DLL_NAME)]
        public static extern void UnityAREngine_DeleteGlTexture(uint texture);
         [DllImport(Constants.UNITY_ARENGINE_DLL_NAME)]
        public static extern void UnityAREngine_SetReferences(IntPtr arSession, IntPtr arFrame);
    }
}


