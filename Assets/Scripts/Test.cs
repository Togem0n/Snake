using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    LList<Vector2> test = new LList<Vector2>();

    void Start()
    {
        test.Push(new Vector2(0, 0));
        test.Push(new Vector2(1, 0));
        test.Push(new Vector2(2, 0));
        test.Clear();
        test.Push(new Vector2(0, 0));
        test.Print();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
