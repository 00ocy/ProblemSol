using DataStructure;
using UnityEngine;

namespace OCY_ProblemSol
{
    public class MemoryPool_Queue : MonoBehaviour
    {
        public Queue<GameObject> bulletQueue;            // �Ѿ� ť
        public GameObject bulletPrefab;                  // �Ѿ� ����
    
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
            // �ʱ⿡ 10���� �Ѿ��� Queue�� �߰�
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.GetComponent<bullet>().Init(transform.position, bulletQueue);
                bulletQueue.Enqueue(obj);
            }
         
        }
    }
}