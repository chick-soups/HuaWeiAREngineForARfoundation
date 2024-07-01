using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.AREngine
{
    public class AREnginePlaneSubsystem : XRPlaneSubsystem
    {

        public const string SubsystemId = "AREngine-Plane";


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RegisterDescriptor()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            var cinfo = new XRPlaneSubsystemDescriptor.Cinfo
            {
                id =SubsystemId,
                providerType = typeof(AREnginePlaneSubsystem.ARengineProvider),
                subsystemTypeOverride = typeof(AREnginePlaneSubsystem),
                supportsHorizontalPlaneDetection = true,
                supportsVerticalPlaneDetection = true,
                supportsArbitraryPlaneDetection = false,
                supportsBoundaryVertices = true,
                supportsClassification = false,
            };

            XRPlaneSubsystemDescriptor.Create(cinfo);
#endif

        }
        public class ARengineProvider : Provider
        {
            private PlaneDetectionMode requestedMode;
            private PlaneDetectionMode currentMode;
            public override void Destroy()
            {

            }

            public override TrackableChanges<BoundedPlane> GetChanges(BoundedPlane defaultPlane, Allocator allocator)
            {
                return new TrackableChanges<BoundedPlane>(0, 0, 0, allocator);
            }

            public override void Start()
            {
                ARConfigHandler.Instance.AddFeature(this, Feature.PlaneTracking);

            }

            public override void Stop()
            {
                ARConfigHandler.Instance.RemoveFeature(this);

            }
            public override PlaneDetectionMode requestedPlaneDetectionMode
            {
                get
                {
                    return requestedMode;
                }
                set
                {

                    requestedMode = value;
                    if (requestedMode != PlaneDetectionMode.None)
                    {

                    }
                }
            }
            public override PlaneDetectionMode currentPlaneDetectionMode
            {
                get
                {
                    return currentMode;
                }
            }
        }
    }
}

