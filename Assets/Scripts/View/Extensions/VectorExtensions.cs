using System.Numerics;

namespace View.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 ToNumericVector(this UnityEngine.Vector2 unityVector)
        {
            return new Vector2(unityVector.x, unityVector.y);
        }
        
        public static Vector3 ToNumericVector(this UnityEngine.Vector3 unityVector)
        {
            return new Vector3(unityVector.x, unityVector.y, unityVector.z);
        }

        public static UnityEngine.Vector2 ToUnityVector(this Vector2 numericVector)
        {
            return new UnityEngine.Vector2(numericVector.X, numericVector.Y);
        }
        
        public static UnityEngine.Vector3 ToUnityVector(this Vector3 numericVector)
        {
            return new UnityEngine.Vector3(numericVector.X, numericVector.Y, numericVector.Z);
        }

        public static Vector2 ToVector2(this Vector3 numericVector3)
        {
            return new Vector2(numericVector3.X, numericVector3.Y);
        }
    }
}