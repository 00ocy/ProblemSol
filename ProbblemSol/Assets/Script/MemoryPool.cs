using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OCY_ProblemSol
{
    public class MemoryPool : MonoBehaviour
    {
        public Transform spawnPoint;                     // 총알 생성 위치
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
                GameObject bulletToActivate = bulletQueue.Peek();
                bulletToActivate.SetActive(true);
                bulletQueue.Dequeue();
                Debug.Log(bulletQueue.Count());
            }
        }

        void AddBullet()
        {
            // 초기에 10개의 총알을 Queue에 추가
            for (int i = 0; i < 10; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
                bullet.SetActive(false); // 비활성화 상태로 시작
                bulletQueue.Enqueue(bullet);
            }
        }
    }
}