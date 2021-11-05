using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private Grid grid = new Grid();
    private int[,] graph;

    public int[,] getGraph { get { return graph; } set { graph = value; } }

    [SerializeField] private GameObject road;
    [SerializeField] private GameObject wall;

    void Awake()
    {
        grid.ReadFile();
        graph = grid.getGraph;

        for (int i = 0; i < graph.GetLength(0); i++)
        {
            for (int j = 0; j < graph.GetLength(1) - 1; j++)
            {
                if (graph[i, j] == 0) Instantiate(road, new Vector2(i, j), Quaternion.identity);
                if (graph[i, j] == 1) Instantiate(wall, new Vector2(i, j), Quaternion.identity);
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

}
