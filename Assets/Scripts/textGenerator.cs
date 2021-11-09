using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textGenerator : MonoBehaviour
{
    public AI ai;
    public GridGenerator gridg;

    int[,] aigraph;
    List<Vector3> aiSnake;

    public GameObject ttt;

    void Start()
    {
        aigraph = ai.getAIgraph;

        aiSnake = ai.getAIsnake;

    }

    // Update is called once per frame
    void Update()
    {
        aigraph = ai.getAIgraph;

        aiSnake = ai.getAIsnake;

        int cnt = 0;
        for (int i = 0; i < aigraph.GetLength(0); i++)
        {
            for (int j = 0; j < aigraph.GetLength(1) - 1; j++)
            {
                //Transform found = transform.Find(i.ToString() + j.ToString() + "(Clone)");

                if (aigraph[i, j] == 2)
                {
                    Debug.DrawRay(new Vector3(i, j), new Vector3(0.5f, 0, 0));
                }
                else
                {
                }

            }
        }
        //Debug.Log(cnt);
    }
}
