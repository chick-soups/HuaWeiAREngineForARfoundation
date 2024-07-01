namespace UnityEngine.XR.AREngine
{
    public class Utility
    {
        public static HwArSession ARSession
        {
            get
            {
                return AREngineSessionSubsystem.AREngineProvider.arSession;
            }

        }

        public static HwArFrame LastARFrame
        {
            get
            {
                return AREngineSessionSubsystem.AREngineProvider.lastFrame;
            }

        }
        public static uint GLTextureName
        {
            get
            {
                return AREngineSessionSubsystem.AREngineProvider.glTexture;
            }
        }
    }
}