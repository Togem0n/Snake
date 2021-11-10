using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private string filename = @"Assets\Scripts\graph.txt";
    private int[,] graph;
    private int[,] AIgraph;


    public int[,] getGraph { get { return graph; } }
    public int[,] getAIGraph { get { return AIgraph; } }


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
        AIgraph = new int[height, width];


        for (int i = 0; i < width - 1; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(!lines[i][j].Equals('\n'))
                {
                    graph[j, i] = int.Parse(lines[i][j].ToString());
                    AIgraph[j, i] = int.Parse(lines[i][j].ToString());

                }
            }
        }

    }

}
