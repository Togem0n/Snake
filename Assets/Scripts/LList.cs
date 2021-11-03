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
    }

    public void Push(T value)
    {
        Node<T> newNode = new Node<T>();
        newNode.value = value;
        curr.next = newNode;
        newNode.prev = curr;
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
    }

    public void Clear()
    {
        head.next = null;
        tail = head;
        curr = head;
    }

    public void Print()
    {
        //Debug.Log("Head->Tail");
        Node<T> curr = head;
        while(curr.next != null)
        {
            curr = curr.next;
            //Debug.Log(curr.value);
        }

        //Debug.Log("Tail->Head");
        Node<T> tmp = tail;
        while (tmp.prev != null)
        {
            //Debug.Log(tmp.value);
            tmp = tmp.prev;
        }
    }
}
