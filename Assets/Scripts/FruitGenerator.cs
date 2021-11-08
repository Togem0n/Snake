using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGenerator : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private GameObject fruit;
    [SerializeField] private GameObject AIfruit;

    private int[,] graph;
    private int[,] AIgraph;
    private List<Vector3> availablePosition = new List<Vector3>();

    // once game start -> generate first fruit
    // once fruit was eaten -> generate next fruit
    void Start()
    {
        graph = gridGenerator.getGraph;

        AIgraph = gridGenerator.getAIGraph;

        GameEvents.current.onFruitGotEaten += GenerateFruit;

        GenerateFruit();
    }

    void Update()
    {
        
    }

    public void GenerateFruit()
    {
        graph = gridGenerator.getGraph;

        AIgraph = gridGenerator.getAIGraph;

        int count = -1;
        for (int i = 0; i < graph.GetLength(0); i++)
        {
            for (int j = 0; j < graph.GetLength(1) - 1; j++)
            {
                if (graph[i, j] == 0 && AIgraph[i, j] == 0)
                {
                    //Debug.DrawLine(new Vector3(i, j, 0), new Vector3(0, 0, 0));
                    count++;
                    availablePosition.Add(new Vector3(i, j, 0));
                }
            }
        }
        int num = Random.Range(0, count);
        Instantiate(fruit, availablePosition[num], Quaternion.identity);
        Instantiate(AIfruit, new Vector3(availablePosition[num].x + 51, availablePosition[num].y, 0), Quaternion.identity);
    }
}
