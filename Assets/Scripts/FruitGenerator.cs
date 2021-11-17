using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGenerator : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private GameObject fruit;
    [SerializeField] private GameObject AIfruit;

    [SerializeField] private AI ai;

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
        GameEvents.current.onFruitGotEatenByAI += GenerateFruit;


        GenerateFruit();
    }

    void Update()
    {
        graph = gridGenerator.getGraph;

        AIgraph = gridGenerator.getAIGraph;
    }

    public void GenerateFruit()
    {

        int count = -1;
        for (int i = 0; i < graph.GetLength(0); i++)
        {
            for (int j = 0; j < graph.GetLength(1) - 1; j++)
            {
                if (graph[i, j] == 0 && AIgraph[i, j] == 0)
                {
                    count++;
                    availablePosition.Add(new Vector3(i, j, 0));
                }
            }
        }
        int num = Random.Range(0, count);
        Instantiate(fruit, availablePosition[num], Quaternion.identity);
        Instantiate(AIfruit, new Vector3(availablePosition[num].x + AIgraph.GetLength(0) + 1, availablePosition[num].y, 0), Quaternion.identity);

        ai.IsNewFruit = true;
    }
}
