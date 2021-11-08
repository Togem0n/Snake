using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    private int[,] AIgraph;
    private int offset;

    private List<Vector3> AIsnake = new List<Vector3>();
    private List<Vector3> shortestPath = new List<Vector3>();

    int count = 0;
    void Start()
    {
        AIgraph = gridGenerator.getAIGraph;
        offset = 51;

        foreach (Transform child in transform)
        {
            AIsnake.Add(new Vector3(child.position.x - offset, child.position.y, 0));

            gridGenerator.setAIGraph((int)child.position.x - offset, (int)child.position.y, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // run astar when game start
        // then everytime instantiate a fruit (so that is everytime a fruit got eaten), run Astar, 
        if(count == 0)
        {
            //Astar(transform.Find("Head").position, GameObject.FindWithTag("AIFruit").transform.position);
            count++;
        }
        //Astar(transform.Find("Head").position, GameObject.FindWithTag("AIFruit").transform.position);
    }

    private void SyncPosition()
    {
        for (int i = 0; i < AIgraph.GetLength(0); i++)
        {
            for (int j = 0; j < AIgraph.GetLength(1) - 1; j++)
            {
                if (AIgraph[i, j] == 2) gridGenerator.setAIGraph(i, j, 0);
            }
        }

        foreach (Transform child in transform)
        {
            gridGenerator.setAIGraph((int)child.position.x - offset, (int)child.position.y, 2);
        }
    }

    private Dictionary<Vector3, Vector3> Astar(Vector3 start, Vector3 end)
    {
        shortestPath.Clear();
        AIgraph = gridGenerator.getAIGraph;

        start.x -= offset;
        end.x -= offset;

        List<Vector3> openList = new List<Vector3>();
        List<Vector3> closeList = new List<Vector3>();

        Dictionary<Vector3, Vector3> parent = new Dictionary<Vector3, Vector3>();
        Dictionary<Vector3, float> g = new Dictionary<Vector3, float>();
        Dictionary<Vector3, float> h = new Dictionary<Vector3, float>();
        Dictionary<Vector3, float> f = new Dictionary<Vector3, float>();

        for (int i = 0; i < AIgraph.GetLength(0); i++)
        {
            for (int j = 0; j < AIgraph.GetLength(1) - 1; j++)
            {   
                h[new Vector3(i, j)] = Mathf.Abs(end.x - i) + Mathf.Abs(end.y - j);
                g[new Vector3(i, j)] = Mathf.Infinity;
                f[new Vector3(i, j)] = Mathf.Infinity;
            }
        }

        g[start] = 0;
        f[start] = h[start];

        openList.Add(start);


        while (openList.Count != 0)
        {
            float min = Mathf.Infinity;
            Vector3 current = new Vector3();
            foreach(var ele in openList)
            {
                if (min < f[ele])
                {
                    min = f[ele];
                    current = ele;
                }
               
            }

            if (current == end)
            {
                Vector3 tmp = current;
                while (parent.ContainsKey(tmp))
                {
                    Debug.Log(tmp);
                    tmp = parent[tmp];
                }
                return parent;
            }

            openList.Remove(current);
            closeList.Add(current);

            //up
            Vector3 currentUp = current;
            currentUp.y += 1;
            
            if(currentUp.x >=0 && currentUp.x <= 51 && currentUp.y >= 0 && currentUp.y <= 51)
            {
                if (!closeList.Contains(currentUp))
                {
                    float tmp = g[current] + 1;
                    if (!openList.Contains(currentUp))
                    {
                        openList.Add(currentUp);
                        parent[currentUp] = current;
                        g[currentUp] = tmp;
                        f[currentUp] = g[currentUp] + h[currentUp];
                    }
                    else if (tmp < g[currentUp])
                    {
                        parent[currentUp] = current;
                        g[currentUp] = tmp;
                        f[currentUp] = g[currentUp] + h[currentUp];
                    }
                }
            }

            //Down
            Vector3 currentDown = current;
            currentDown.y -= 1;

            if (currentDown.x >= 0 && currentDown.x <= 51 && currentDown.y >= 0 && currentDown.y <= 51)
            {
                if (!closeList.Contains(currentDown))
                {
                    float tmp = g[current] + 1;
                    if (!openList.Contains(currentDown))
                    {
                        openList.Add(currentDown);
                        parent[currentDown] = current;
                        g[currentDown] = tmp;
                        f[currentDown] = g[currentDown] + h[currentDown];
                    }
                    else if (tmp < g[currentDown])
                    {
                        parent[currentDown] = current;
                        g[currentDown] = tmp;
                        f[currentDown] = g[currentDown] + h[currentDown];
                    }
                }
            }

            //Left
            Vector3 currentLeft = current;
            currentLeft.x -= 1;

            if (currentLeft.x >= 0 && currentLeft.x <= 51 && currentLeft.y >= 0 && currentLeft.y <= 51)
            {
                if (!closeList.Contains(currentLeft))
                {
                    float tmp = g[current] + 1;
                    if (!openList.Contains(currentLeft))
                    {
                        openList.Add(currentLeft);
                        parent[currentLeft] = current;
                        g[currentLeft] = tmp;
                        f[currentLeft] = g[currentLeft] + h[currentLeft];
                    }
                    else if (tmp < g[currentLeft])
                    {
                        parent[currentLeft] = current;
                        g[currentLeft] = tmp;
                        f[currentLeft] = g[currentLeft] + h[currentLeft];
                    }
                }
            }

            //Right
            Vector3 currentRight = current;
            currentRight.x += 1;

            if (currentRight.x >= 0 && currentRight.x <= 51 && currentRight.y >= 0 && currentRight.y <= 51)
            {
                if (!closeList.Contains(currentRight))
                {
                    float tmp = g[current] + 1;
                    if (!openList.Contains(currentRight))
                    {
                        openList.Add(currentRight);
                        parent[currentRight] = current;
                        g[currentRight] = tmp;
                        f[currentRight] = g[currentRight] + h[currentRight];
                    }
                    else if (tmp < g[currentRight])
                    {
                        parent[currentRight] = current;
                        g[currentRight] = tmp;
                        f[currentRight] = g[currentRight] + h[currentRight];
                    }
                }
            }
        }

        Debug.Log(start.ToString() + ";" + end.ToString());
        return parent;
    }
}
