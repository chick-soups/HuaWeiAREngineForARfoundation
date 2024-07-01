using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
namespace UnityEngine.XR.AREngine
{
    public class AREngineRaycastSubsystem : XRRaycastSubsystem
    {
        public const string SubsystemId = "AREngine-Raycast";
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RegisterDescriptor()
        {

            XRRaycastSubsystemDescriptor.RegisterDescriptor(new XRRaycastSubsystemDescriptor.Cinfo
            {
                id = SubsystemId,
                providerType = typeof(AREngineRaycastSubsystem.AREngineProvider),
                subsystemTypeOverride = typeof(AREngineRaycastSubsystem),
                supportsViewportBasedRaycast = true,
                supportsWorldBasedRaycast = true,
                supportedTrackableTypes =
                    (TrackableType.Planes & ~TrackableType.PlaneWithinInfinity) |
                    TrackableType.FeaturePoint |
                    TrackableType.Depth,
                supportsTrackedRaycasts = true,
            });
        }
        public class AREngineProvider : Provider
        {

            public override void Start()
            {
               ARConfigHandler.Instance.AddFeature(this,Feature.Raycast);
            }
            public override void Stop()
            {
                ARConfigHandler.Instance.RemoveFeature(this);
            }

        }

    }
}

