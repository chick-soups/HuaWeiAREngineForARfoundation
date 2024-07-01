using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.XR.ARSubsystems;
using UnityEditor.XR.ARSubsystems;
using UnityEngine.XR.AREngine;
namespace UnityEditor.XR.AREngine.Editor
{
    public class AREngineBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
#if UNITY_ANDROID
            BuildHelper.RemoveShaderFromProject(AREngineCameraSubsystem.AREngineProvider.BEFORE_OPAQUES_SHADER_NAME);
#endif
        }

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_ANDROID
            BuildHelper.AddBackgroundShaderToProject(AREngineCameraSubsystem.AREngineProvider.BEFORE_OPAQUES_SHADER_NAME);
#endif
        }
    }
}

