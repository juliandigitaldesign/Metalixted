using UnityEngine;

public static class Transforms
{
    public static void DestroyChildren(this Transform t, bool destryInmediately = false)
    {
        foreach (Transform child in t)
        {
            if (destryInmediately)
                MonoBehaviour.DestroyImmediate(child.gameObject);
            else
                MonoBehaviour.Destroy(child.gameObject);
        }
    }
}
