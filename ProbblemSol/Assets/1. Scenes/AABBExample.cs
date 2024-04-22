using UnityEngine;

public class AABBExample : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;

    void Update()
    {
        Bounds bounds1 = object1.GetComponent<Renderer>().bounds;
        Bounds bounds2 = object2.GetComponent<Renderer>().bounds;

        if (bounds1.Intersects(bounds2))
        {
            Debug.Log("AABB Collision Detected!");
        }
    }
}
