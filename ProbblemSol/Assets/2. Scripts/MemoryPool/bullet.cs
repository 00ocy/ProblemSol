using DataStructure;
using UnityEngine;

namespace OCY_ProblemSol
{
    public class bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public Vector3 BulletDirection;
        private Vector3 BulletInitialPos;

        public Queue<GameObject> list_Q;
        public Stack<GameObject> list_S;

        void Start()
        {
            BulletDirection.Normalize();
        }
        public void Init(Vector3 InitialPos, Queue<GameObject> Queue)
        {
            BulletInitialPos = InitialPos;
            gameObject.transform.position = BulletInitialPos;
            list_Q = Queue;
        }
        public void Init(Vector3 InitialPos, Stack<GameObject> Stack)
        {
            BulletInitialPos = InitialPos;
            gameObject.transform.position = BulletInitialPos;
            list_S = Stack;
        }
        // ���� ���� �Լ�
        public void SetDirection(Vector3 direction)
        {
            BulletDirection = direction.normalized;
        }

        void Update()
        {
            Vector3 movement = BulletDirection * BulletSpeed * Time.deltaTime;
            transform.Translate(movement);

            // ���� ������Ʈ�� ��ġ�� �������� �ֺ��� �ִ� ��� Collider���� �浹�� �˻�
            Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("red"))
                {
                    if(list_Q != null)
                    {
                        gameObject.SetActive(false);
                        gameObject.transform.position = BulletInitialPos;

                        list_Q.Enqueue(gameObject);
                        Debug.Log(list_Q.Count());
                        break; 
                    }
                    else
                    {
                        gameObject.SetActive(false);
                        gameObject.transform.position = BulletInitialPos;

                        list_S.Push(gameObject);
                        Debug.Log(list_S.Count());
                        break;
                    }
                }
            }
        }
       
    }
}