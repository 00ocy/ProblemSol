using DataStructure;
using UnityEngine;

namespace OCY_ProblemSol
{
    public class MemoryPool_Stack : MonoBehaviour
    {
        public Stack<GameObject> bulletStack;            // �Ѿ� ����
        public GameObject bulletPrefab;                  // �Ѿ� ����
    
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