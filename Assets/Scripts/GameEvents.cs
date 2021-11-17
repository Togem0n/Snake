using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onFruitGotEaten;

    public event Action onFruitGotEatenByAI;
    public void FruitGotEaten()
    {
        if(onFruitGotEaten != null)
        {
            onFruitGotEaten();
        }
    }

    public void FruitGotEatenByAI()
    {
        if (onFruitGotEatenByAI != null)
        {
            onFruitGotEatenByAI();
        }
    }
}
