using DataStructure;
using UnityEngine;

namespace OCY_ProblemSol
{
    public class MemoryPool_Queue : MonoBehaviour
    {
        public Queue<GameObject> bulletQueue;            // 총알 큐
        public GameObject bulletPrefab;                  // 총알 원본
    
        void Start()
        {
            bulletQueue = new Queue<GameObject>();

            AddBullet();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && bulletQueue.Count() > 0)
            {
                GameObject bulletToActivate = bulletQueue.Dequeue();
                bulletToActivate.SetActive(true);
                Debug.Log(bulletQueue.Count());
            }
        }

        void AddBullet()
        {
            // 초기에 10개의 총알을 Queue에 추가
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.GetComponent<bullet>().Init(transform.position, bulletQueue);
                bulletQueue.Enqueue(obj);
            }
         
        }
    }
}