using UnityEngine;

public class SphereCollisionExample : MonoBehaviour
{
    public Transform sphere1;
    public Transform sphere2;
    public float radius1 = 1f;
    public float radius2 = 1f;

    void Update()
    {
        Vector3 center1 = sphere1.position;
        Vector3 center2 = sphere2.position;

        float distance = Vector3.Distance(center1, center2);
        if (distance <= radius1 + radius2)
        {
            Debug.Log("Sphere Collision Detected!");
        }
    }
}
