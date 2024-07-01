namespace UnityEngine.XR.AREngine
{
    public enum HwArCoordinateSystemType
    {
        HwAr_Coordinate_System_Type_Unknown = -1,
        HwAr_Coordinate_System_Type_3D_World, // world coordinate system
        HwAr_Coordinate_System_Type_3D_Self, // 3d image coordinate system for bodypose
        HwAr_Coordinate_System_Type_2D_Image, // 2D image coordinate system for gesture (openGL)
        HwAr_Coordinate_System_Type_3D_Camera
    }
}