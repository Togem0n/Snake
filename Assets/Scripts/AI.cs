using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private float AIInterval;
    [SerializeField] private GameObject tail;

    [SerializeField] private SnakeController snakeController;
    private float interval;
    private int[,] AIgraph;
    private int offset;

    private List<Vector3> AIsnake = new List<Vector3>();
    private List<Vector3> shortestPath = new List<Vector3>();

    int count = 0;
    void Start()
    {
        AIgraph = gridGenerator.getAIGraph;
        offset = 51;
        interval = AIInterval;
        foreach (Transform child in transform)
        {
            AIsnake.Add(new Vector3(child.position.x, child.position.y, 0));

            gridGenerator.setAIGraph((int)child.position.x - offset, (int)child.position.y, 2);
        }

        GameEvents.current.onFruitGotEaten += GrowTail;
        GameEvents.current.onFruitGotEaten += combineTest;

        Astar(transform.Find("Head").position, GameObject.FindWithTag("AIFruit").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // run astar when game start
        // then everytime instantiate a fruit (so that is everytime a fruit got eaten), run Astar, 
/*        if(count == 0)
        {
            Astar(transform.Find("Head").position, GameObject.FindWithTag("AIFruit").transform.position);
            count++;
        }*/

        if (snakeController.GameStarted)
        {
            if (shortestPath.Count != 0)
            {
                if (interval > 0)
                {
                    interval -= Time.deltaTime;
                }
                else
                {
                    interval = AIInterval;
                    ChangePosition();
                }
            }
           
        }
    }

    private void combineTest()
    {
        Debug.Log("???");
        Astar(transform.Find("Head").position, GameObject.FindWithTag("AIFruit").transform.position);
    }

    private void SyncPosition()
    {
        Debug.Log(GameObject.FindWithTag("AIFruit").transform.position);

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

    private void ChangePosition()
    {
        /*Vector3 des = shortestPath[shortestPath.Count - 1];
                    des.x += offset;
                    shortestPath.RemoveAt(shortestPath.Count - 1);
                    Vector3 nextDirection = des - transform.position;*/
        for (int i = AIsnake.Count - 1; i >= 0; i--)
        {
            foreach (Transform child in transform)
            {

                if (child.position == AIsnake[i])
                {

                    if (i == 0)
                    {
                        Vector3 des = shortestPath[shortestPath.Count - 1];
                        des.x += offset;
                        shortestPath.RemoveAt(shortestPath.Count - 1);
                        Vector3 nextDirection = des - AIsnake[i];
                        AIsnake[i] += nextDirection;
                        child.Translate(nextDirection);
                    }
                    else
                    {
                        child.Translate(AIsnake[i - 1] - AIsnake[i]);
                        AIsnake[i] = AIsnake[i - 1];
                    }


                }
            }

        }
    }

    private Dictionary<Vector3, Vector3> Astar(Vector3 start, Vector3 end)
    {
        SyncPosition();
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
                if (min >= f[ele])
                {
                    min = f[ele];
                    current = ele;
                }
               
            }

            if (current == end)
            {
                Vector3 tmp = current;
                int shortestDis = 0;
                while (parent.ContainsKey(tmp))
                {
                    shortestPath.Add(tmp);
                    tmp = parent[tmp]; 
                    shortestDis++;
                }
                return parent;
            }

            openList.Remove(current);
            closeList.Add(current);

            //up
            Vector3 currentUp = new Vector3();
            currentUp.x = current.x;
            currentUp.y = current.y + 1;
            currentUp.z = current.z;
            
            if(currentUp.x >=0 && currentUp.x < AIgraph.GetLength(0) && currentUp.y >= 0 && currentUp.y < AIgraph.GetLength(1) - 1 && AIgraph[(int)currentUp.x, (int)currentUp.y] == 0)
            {
                if (!closeList.Contains(currentUp)){
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
            Vector3 currentDown = new Vector3();
            currentDown.x = current.x;
            currentDown.y = current.y - 1;
            currentDown.z = current.z;

            if (currentDown.x >= 0 && currentDown.x < AIgraph.GetLength(0) && currentDown.y >= 0 && currentDown.y < AIgraph.GetLength(1) - 1 && AIgraph[(int)currentDown.x, (int)currentDown.y] == 0)
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
            Vector3 currentLeft = new Vector3();
            currentLeft.x = current.x - 1;
            currentLeft.y = current.y;
            currentLeft.z = current.z;

            if (currentLeft.x >= 0 && currentLeft.x < AIgraph.GetLength(0) && currentLeft.y >= 0 && currentLeft.y < AIgraph.GetLength(1) - 1 && AIgraph[(int)currentLeft.x, (int)currentLeft.y] == 0)
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
            Vector3 currentRight = new Vector3();
            currentRight.x = current.x + 1;
            currentRight.y = current.y;
            currentRight.z = current.z;

            if (currentRight.x >= 0 && currentRight.x < AIgraph.GetLength(0) && currentRight.y >= 0 && currentRight.y < AIgraph.GetLength(1) - 1 && AIgraph[(int)currentRight.x, (int)currentRight.y] == 0)
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

        //Debug.Log(start.ToString() + ";" + end.ToString());
        return parent;
    }

    private void GrowTail()
    {

        Vector3 growDirection = AIsnake[AIsnake.Count - 1] - AIsnake[AIsnake.Count - 2];
        Vector3 growPosition = AIsnake[AIsnake.Count - 1] + growDirection;
        AIsnake.Add(growPosition);
        gridGenerator.setAIGraph((int)growPosition.x - offset, (int)growPosition.y, 2);
        Instantiate(tail, growPosition, Quaternion.identity, transform);
    }
}
