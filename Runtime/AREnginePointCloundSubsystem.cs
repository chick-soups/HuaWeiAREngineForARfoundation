using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.AREngine
{
    public class AREnginePointCloundSubsystem : XRPointCloudSubsystem
    {
        public const string SystemId = "AREngine-PointCloud";
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {

#if !UNITY_EDITOR && UNITY_ANDROID
            var info = new XRPointCloudSubsystemDescriptor.Cinfo
            {
                id = SystemId,
                providerType = typeof(AREngineProvider),
                subsystemTypeOverride = typeof(AREnginePointCloundSubsystem),
                supportsFeaturePoints = true,
                supportsUniqueIds = true,
                supportsConfidence = true
            };
            XRPointCloudSubsystemDescriptor.RegisterDescriptor(info);
#endif

        }

        public class AREngineProvider : Provider
        {
            public override void Start()
            {
                ARConfigHandler.Instance.AddFeature(this,Feature.PointCloud);
            }
            public override void Stop()
            {
                ARConfigHandler.Instance.RemoveFeature(this);
            }
            public override TrackableChanges<XRPointCloud> GetChanges(XRPointCloud defaultPointCloud, Allocator allocator)
            {
                return new TrackableChanges<XRPointCloud>(0, 0, 0, allocator);
            }

            public override XRPointCloudData GetPointCloudData(TrackableId trackableId, Allocator allocator)
            {
                return new XRPointCloudData();
            }

        }
    }

}

