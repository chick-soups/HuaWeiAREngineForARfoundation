using System;
namespace UnityEngine.XR.AREngine
{
    public class ARUnavailableException : Exception
    {
        public HwArStatus Status { get; private set; }
        public ARUnavailableException(HwArStatus hwArStatus){
           this.Status = hwArStatus;
        }
    }
}
