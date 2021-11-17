using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private Grid grid = new Grid();
    private int[,] graph;

    // Create a graph for AI
    private int[,] AIgraph;

    public int[,] getGraph { get { return graph; } set { graph = value; } }
    public int[,] getAIGraph { get { return AIgraph; } set { AIgraph = value; } }


    [SerializeField] private GameObject road;
    [SerializeField] private GameObject wall;

    [SerializeField] private bool enableAI;

    void Awake()
    {
        grid.ReadFile();
        graph = grid.getGraph;
        AIgraph = grid.getAIGraph;

        for (int i = 0; i < graph.GetLength(0); i++)
        {
            for (int j = 0; j < graph.GetLength(1) - 1; j++)
            {
                if (graph[i, j] == 0) Instantiate(road, new Vector2(i, j), Quaternion.identity);
                if (graph[i, j] == 1) Instantiate(wall, new Vector2(i, j), Quaternion.identity);
            }
        }

        Debug.Log(graph.GetLength(0));
        Debug.Log(graph.GetLength(1));

        if (enableAI)
        {
            for (int i = 0; i < AIgraph.GetLength(0); i++)
            {
                for (int j = 0; j < AIgraph.GetLength(1) - 1; j++)
                {
                    if (AIgraph[i, j] == 0) Instantiate(road, new Vector2(i + AIgraph.GetLength(0) + 1, j), Quaternion.identity);
                    if (AIgraph[i, j] == 1) Instantiate(wall, new Vector2(i + AIgraph.GetLength(0) + 1, j), Quaternion.identity);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGraph(int i, int j, int value)
    {
        graph[i, j] = value;
    }

    public void setAIGraph(int i, int j, int value)
    {
        AIgraph[i, j] = value;
    }

}
