using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    public MemoryPool MP;

    void Update()
    {
        Vector3 movement = new Vector3(2f, 0f, 0f) * 4 * Time.deltaTime;
        transform.Translate(movement);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("red"))
        {
            gameObject.SetActive(false);
            gameObject.transform.position = MP.spawnPoint.position;
            MP.bulletQueue.Enqueue(gameObject);
            Debug.Log(MP.bulletQueue.Count());
        }
    }
}