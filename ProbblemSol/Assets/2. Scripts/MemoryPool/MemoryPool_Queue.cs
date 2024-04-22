using DataStructure;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OCY_ProblemSol
{
    public class MemoryPool_Queue : MonoBehaviour
    {
        public Queue<GameObject> bulletQueue;            // 총알 큐
        public GameObject bulletPrefab;                  // 총알 원본
        public GameObject InitPos;

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

                // 총알의 위치를 InitPos 위치로 설정
                bulletToActivate.transform.position = InitPos.transform.position;

                // 총알 방향
                Vector3 direction = InitPos.transform.forward;

                // 총알의 방향을 설정
                bulletToActivate.GetComponent<bullet>().SetDirection(direction);
                

                // 총알 활성화
                bulletToActivate.SetActive(true);

                Debug.Log(bulletQueue.Count());
            }
        }

        // 초기에 총알을 생성하여 큐에 추가
        void AddBullet()
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.GetComponent<bullet>().Init(InitPos.transform.position, bulletQueue);
                bulletQueue.Enqueue(obj);
            }
        }

    }
}
