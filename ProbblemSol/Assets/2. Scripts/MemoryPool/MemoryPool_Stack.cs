using DataStructure;
using UnityEngine;

namespace OCY_ProblemSol
{
    public class MemoryPool_Stack : MonoBehaviour
    {
        public Stack<GameObject> bulletStack;            // 총알 스택
        public GameObject bulletPrefab;                  // 총알 원본
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

                // 총알의 위치를 InitPos 위치로 설정
                bulletToActivate.transform.position = InitPos.transform.position;
                // 총알 방향
                Vector3 direction = InitPos.transform.forward;

                // 총알의 방향을 설정
                bulletToActivate.GetComponent<bullet>().SetDirection(direction);

                // 총알 활성화
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