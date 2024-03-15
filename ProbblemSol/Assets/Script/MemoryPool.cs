using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OCY_ProblemSol
{
    public class MemoryPool : MonoBehaviour
    {
        public Transform spawnPoint;                     // �Ѿ� ���� ��ġ
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
                GameObject bulletToActivate = bulletQueue.Peek();
                bulletToActivate.SetActive(true);
                bulletQueue.Dequeue();
                Debug.Log(bulletQueue.Count());
            }
        }

        void AddBullet()
        {
            // �ʱ⿡ 10���� �Ѿ��� Queue�� �߰�
            for (int i = 0; i < 10; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
                bullet.SetActive(false); // ��Ȱ��ȭ ���·� ����
                bulletQueue.Enqueue(bullet);
            }
        }
    }
}