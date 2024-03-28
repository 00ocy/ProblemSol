using System;

namespace DataStructure
{
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
    public class Stack<T>
    {
        private Queue<T> queue = new Queue<T>();
        private int count; // 스택의 요소 개수를 추적하는 카운트 변수

        public Stack()
        {
            count = 0; // 스택이 생성될 때 초기화
        }

        public void Push(T data)
        {
            // 새로운 요소를 추가하기 위해 임시 큐 생성
            Queue<T> tempQueue = new Queue<T>();

            // 기존 스택의 모든 요소를 임시 큐로 옮기기
            while (queue.Count() > 0)
            {
                tempQueue.Enqueue(queue.Dequeue());
            }

            // 새로운 요소를 맨 앞에 추가
            queue.Enqueue(data);

            // 임시 큐의 모든 요소를 다시 원래 스택으로 옮기기
            while (tempQueue.Count() > 0)
            {
                queue.Enqueue(tempQueue.Dequeue());
            }

            // 요소가 추가될 때마다 카운트 증가
            count++;
        }

        public T Pop()
        {
            if (count == 0)
                throw new InvalidOperationException("Stack is empty");

            // 요소가 제거될 때마다 카운트 감소
            count--;

            return queue.Dequeue();
        }

        // 현재 스택의 요소 개수를 반환하는 메서드
        public int Count()
        {
            return count;
        }
    }



}
