using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private string filename = @"Assets\Scripts\graph.txt";
    private int[,] graph;

    public int[,] getGraph { get { return graph; } }

    public void ReadFile()
    {
        string text = File.ReadAllText(filename);
        string[] lines = text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Replace(" ", "");
        }

        int height = lines.Length;
        int width = lines[0].Length;

        graph = new int[height, width];

        Debug.Log(width);
        Debug.Log(height);
        Debug.Log(lines[0][lines[0].Length - 1]);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width - 1; j++)
            {
                if(!lines[i][j].Equals('\n'))
                {
                    graph[i, j] = int.Parse(lines[i][j].ToString());
                }
            }
        }

    }

}
