using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OCY_ProblemSol
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
}
