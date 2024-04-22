using DataStructure;
using UnityEngine;

namespace OCY_ProblemSol
{
    public class MemoryPool_Stack : MonoBehaviour
    {
        public Stack<GameObject> bulletStack;            // �Ѿ� ����
        public GameObject bulletPrefab;                  // �Ѿ� ����
        public GameObject InitPos;

        void Start()
        {
            bulletStack = new Stack<GameObject>();

            AddBullet();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && bulletStack.Count() > 0)
            {
                GameObject bulletToActivate = bulletStack.Pop();

                // �Ѿ��� ��ġ�� InitPos ��ġ�� ����
                bulletToActivate.transform.position = InitPos.transform.position;
                // �Ѿ� ����
                Vector3 direction = InitPos.transform.forward;

                // �Ѿ��� ������ ����
                bulletToActivate.GetComponent<bullet>().SetDirection(direction);

                // �Ѿ� Ȱ��ȭ
                bulletToActivate.SetActive(true);
                Debug.Log(bulletStack.Count());
            }
        }

        void AddBullet()
        {
            // �ʱ⿡ 10���� �Ѿ��� Stack�� �߰�
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.GetComponent<bullet>().Init(transform.position, bulletStack);
                bulletStack.Push(obj);
            }
         
        }
    }
}