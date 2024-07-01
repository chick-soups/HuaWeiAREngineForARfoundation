using UnityEngine;
namespace UnityEngine.XR.AREngine
{
    public class MathfExtenstion
    {
        //TODO:Figure out why rotation is needed?
        public static Matrix4x4 rotationMatrix = new Matrix4x4(
           new Vector4(0, 0, -1, 0),
           new Vector4(0, 1, 0, 0),
           new Vector4(1, 0, 0, 0),
           new Vector4(0, 0, 0, 1));
        private static Vector3 TransferLeftRight(Vector3 vector)
        {
            vector.z = -vector.z;
            return vector;
        }
          private static Vector4 TransferLeftRight(Vector4 vector)
        {
            vector.z = -vector.z;
            return vector;
        }

        private static Quaternion TransferLeftRight(Quaternion rotation)
        {
            rotation.x = -rotation.x;
            rotation.y = -rotation.y;
            return rotation;
        }
        private static Pose TransferLeftRight(Pose pose)
        {
            pose.position.z = -pose.position.z;
            pose.rotation.x = -pose.rotation.x;
            pose.rotation.y = -pose.rotation.y;
            return pose;
        }
        private static Matrix4x4 TransferLeftRight(Matrix4x4 matrix4X4)
        {
            Matrix4x4 matrix = Matrix4x4.Scale(new Vector3(1, 1, -1));
            return matrix * matrix4X4 * matrix.inverse;
        }

        public static Vector3 AREngineToUnity(Vector3 vector)
        {
            vector = TransferLeftRight(vector);
            vector = rotationMatrix * vector;
            return vector;
        }
          public static Vector4 AREngineToUnity(Vector4 vector)
        {
            vector = TransferLeftRight(vector);
            vector = rotationMatrix * vector;
            return vector;
        }

        public static Quaternion AREngineToUnity(Quaternion quaternion)
        {
            quaternion = TransferLeftRight(quaternion);
            quaternion = rotationMatrix.rotation * quaternion;
            return quaternion;
        }
        public static Pose AREngineToUnity(Pose pose)
        {
            pose = MathfExtenstion.TransferLeftRight(pose);
            pose.position = rotationMatrix * pose.position;
            pose.rotation = rotationMatrix.rotation * pose.rotation;
            return pose;
        }

        public static Matrix4x4 AREngineToUnity(Matrix4x4 matrix4X4)
        {
            Matrix4x4 matrix = Matrix4x4.Scale(new Vector3(1, 1, -1));
            matrix = matrix * matrix4X4 * matrix.inverse;
            return rotationMatrix * matrix;
        }

        public static Vector3 UnityToAREngine(Vector3 vector)
        {
            vector = rotationMatrix.inverse * vector;
            vector = TransferLeftRight(vector);
            return vector;
        }
        public static Quaternion UnityToAREngine(Quaternion quaternion)
        {
            quaternion = rotationMatrix.inverse.rotation * quaternion;
            quaternion = TransferLeftRight(quaternion);
            return quaternion;
        }
        public static Pose UnityToAREngine(Pose pose)
        {
            pose.position = rotationMatrix.inverse * pose.position;
            pose.rotation = rotationMatrix.inverse.rotation * pose.rotation;
            pose = TransferLeftRight(pose);
            return pose;
        }

        public static Matrix4x4 UnityToAREngine(Matrix4x4 matrix4X4)
        {
            matrix4X4 = rotationMatrix.inverse * matrix4X4;
            Matrix4x4 matrix = Matrix4x4.Scale(new Vector3(1, 1, -1));
            matrix4X4 = matrix * matrix4X4 * matrix.inverse;
            return matrix4X4;
        }
    }
}