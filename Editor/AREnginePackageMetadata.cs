using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.XR.AREngine;

using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;

namespace UnityEditor.XR.AREngine.Editor
{
    class XRPackage : IXRPackage
    {
        class AREngineLoaderMetadata : IXRLoaderMetadata
        {
            public string loaderName { get; set; }
            public string loaderType { get; set; }
            public List<BuildTargetGroup> supportedBuildTargets { get; set; }
        }

        class AREnginePackageMetadata : IXRPackageMetadata
        {
            public string packageName { get; set; }
            public string packageId { get; set; }
            public string settingsType { get; set; }
            public List<IXRLoaderMetadata> loaderMetadata { get; set; }
        }

        static IXRPackageMetadata s_Metadata = new AREnginePackageMetadata()
        {
            packageName = "HuaWei AREngine XR Plug-in",
            packageId = "com.unity.xr.huaweiarengine",
            settingsType = typeof(AREngineSettings).FullName,
            loaderMetadata = new List<IXRLoaderMetadata>()
            {
                new AREngineLoaderMetadata()
                {
                    loaderName = "HuaWei AREngine",
                    loaderType = typeof(AREngineLoader).FullName,
                    supportedBuildTargets = new List<BuildTargetGroup>()
                    {
                        BuildTargetGroup.Android
                    }
                },
            }
        };

        public IXRPackageMetadata metadata => s_Metadata;

        public bool PopulateNewSettingsInstance(ScriptableObject obj)
        {
            if(obj is AREngineSettings settings)
            {
                settings.requirement = AREngineSettings.Requirement.Required;
                AREngineSettings.currentSettings = settings;
                return true;
            }

            return false;
        }
    }
}
