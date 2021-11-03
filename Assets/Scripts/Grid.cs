using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    string filename = @"Assets\Scripts\graph.txt";
    int[,] graph;
    private void Awake()
    {
        ReadFile();
    }

    private void ReadFile()
    {
        string text = File.ReadAllText(filename);
        string[] lines = text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Replace(" ", "");
        }

        int height = lines.Length;
        int width = lines[0].Length;

        graph = new int[width, height];

        Debug.Log(width);
        Debug.Log(height);
        Debug.Log(lines[0][lines[0].Length - 1]);

        for (int i = 0; i < graph.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                if(!lines[i][j].Equals('\n'))
                {
                    graph[i, j] = int.Parse(lines[i][j].ToString());
                    Debug.Log(graph[i, j]);
                }
            }
        }

    }

}
