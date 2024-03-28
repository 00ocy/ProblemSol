using DataStructure;
using UnityEngine;

namespace OCY_ProblemSol
{
    public class MemoryPool_Stack : MonoBehaviour
    {
        public Stack<GameObject> bulletStack;            // 총알 스택
        public GameObject bulletPrefab;                  // 총알 원본
    
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
            // 초기에 10개의 총알을 Stack에 추가
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.GetComponent<bullet>().Init(transform.position, bulletStack);
                bulletStack.Push(obj);
            }
         
        }
    }
}