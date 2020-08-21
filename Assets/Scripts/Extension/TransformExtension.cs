using UnityEngine;

namespace Extension
{
    public static class TransformExtension
    {
        public static void DestroyAllChildrenObjects(this Transform transform)
        {
            foreach (Transform child in transform) UnityEngine.Object.Destroy(child.gameObject);
        }
    }
}