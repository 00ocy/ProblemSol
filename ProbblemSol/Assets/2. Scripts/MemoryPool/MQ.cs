using DataStructure;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OCY_ProblemSol
{
    public class MQ : MonoBehaviour
    {
        public Queue<GameObject> bulletQueue;            // �Ѿ� ť
        public GameObject bulletPrefab;                  // �Ѿ� ����
        public GameObject InitPos;

        void Start()
        {
            bulletQueue = new Queue<GameObject>();

            AddBullet();

            // 3�ʸ��� Shoot �޼��带 ����
            //InvokeRepeating("Shoot", 1f, 1f);
        }

        public void Shoot()
        {
            if (bulletQueue.Count() > 0)
            {
                GameObject bulletToActivate = bulletQueue.Dequeue();

                // �Ѿ��� ��ġ�� InitPos ��ġ�� ����
                bulletToActivate.transform.position = InitPos.transform.position;

                // �Ѿ� ����
                Vector3 direction = InitPos.transform.forward;

                // �Ѿ��� ������ ����
                bulletToActivate.GetComponent<bullet>().SetDirection(direction);

                // �Ѿ� Ȱ��ȭ
                bulletToActivate.SetActive(true);

                Debug.Log(bulletQueue.Count());
            }
        }

        // �ʱ⿡ �Ѿ��� �����Ͽ� ť�� �߰�
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