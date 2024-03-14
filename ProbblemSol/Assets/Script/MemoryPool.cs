using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MemoryPool : MonoBehaviour
{
    public Transform spawnPoint; // �Ѿ��� ������ ��ġ�� ������ Transform
    public Queue<GameObject> bulletQueue;
    public GameObject bulletPrefab;

    void Start()
    {
        bulletQueue = new Queue<GameObject>();

        // �ʱ⿡ 10���� �Ѿ��� Queue�� �߰�
        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            bullet.SetActive(false); // ��Ȱ��ȭ ���·� ����
            bulletQueue.Enqueue(bullet);
        }
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


    public class Queue<T>
    {
        public class Node
        {
            public T data;
            public Node next;

            public Node(T data)
            {
                this.data = data;
                this.next = null;
            }
        }

        public Node head;
        private Node tail;
        private int count;

        public Queue()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void Enqueue(T data)
        {
            Node newNode = new Node(data);

            if (tail == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.next = newNode;
                tail = newNode;
            }

            count++;
        }

        public T Peek()
        {
            if (head == null)
                throw new InvalidOperationException("Queue is empty");

            return head.data;
        }

        public T Dequeue()
        {
            if (head == null)
                throw new InvalidOperationException("Queue is empty");

            T data = head.data;
            head = head.next;
            count--;

            if (head == null)
                tail = null;

            return data;
        }

        public int Count()
        {
            return count;
        }
    }
}