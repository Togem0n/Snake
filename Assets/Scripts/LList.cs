using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    public T value;
    public Node<T> prev;
    public Node<T> next;
}

public class LList<T>
{
    private Node<T> head;
    private Node<T> tail;
    private Node<T> curr;
    public int count;

    public LList()
    {
        head = new Node<T>();
        curr = head;
        tail = head;
    }

    public Node<T> getHead { get { return head; } }
    public Node<T> getTail { get { return tail; } }
    public Node<T> getCurr { get { return curr; } set { curr = value; } }



    public void Push(T value)
    {
        Node<T> newNode = new Node<T>();
        newNode.value = value;
        tail.next = newNode;
        newNode.prev = tail;
        newNode.next = null;
        curr = newNode;
        tail = newNode;
        count++;
    }

    public void Pop()
    {
        Node<T> last = tail.prev;
        last.next = null;
        tail = last;
        count--;
    }

    public void Clear()
    {
        head.next = null;
        tail = head;
        curr = head;
        count = 0;
    }

    public void StepThrough()
    {
        //Debug.Log("Head->Tail");
        Node<T> curr = head;
        while(curr.next != null)
        {
            curr = curr.next;
            Debug.Log(curr.value);
        }
    }

    public Node<T> Find(T value)
    {
        //Debug.Log("Head->Tail");
        Node<T> curr = head;
        while (curr.next != null)
        {
            if (object.Equals(curr.value, value)) return curr;
            curr = curr.next;
            //Debug.Log(curr.value);
        }
        if (object.Equals(curr.value, value)) return curr;
        return null;
    }
}
