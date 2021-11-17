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

    public int[,] getAIgraph { get { return AIgraph; } }

    private List<Vector3> AIsnake = new List<Vector3>();
    private List<Vector3> OldAIsnake = new List<Vector3>();

    public List<Vector3> getAIsnake { get { return AIsnake; } }

    private List<Vector3> shortestPath = new List<Vector3>();
    private int shortestDis;
    private bool isNewFruit;

    public bool IsNewFruit { get { return isNewFruit; } set { isNewFruit = value; } }

    private float nextSearchTime = 0.05f;
    private float nextSearchCounter = 0.05f;

    private Vector3 nextAstarStartPos;

   // [SerializeField] private TMPro text;

    int count = 0;
    void Start()
    {
        AIgraph = gridGenerator.getAIGraph;
        offset = AIgraph.GetLength(0) + 1;
        interval = AIInterval;

        Debug.Log(offset);

        transform.SetPositionAndRotation(new Vector3(AIgraph.GetLength(0) / 2 + offset, (AIgraph.GetLength(1) - 1) / 2, 0), Quaternion.identity);

        foreach (Transform child in transform)
        {
            AIsnake.Add(new Vector3((int)child.position.x, (int)child.position.y, 0));
            OldAIsnake.Add(new Vector3Int((int)child.position.x, (int)child.position.y, 0));

            gridGenerator.setAIGraph((int)child.position.x - offset, (int)child.position.y, 2);
        }

        GameEvents.current.onFruitGotEatenByAI += GrowTail;

        Astar(transform.Find("Head").position, GameObject.FindWithTag("AIFruit").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (snakeController.GameStarted)
        {
            if (shortestPath.Count > 0)
            {
                if (interval > 0)
                {
                    interval -= Time.deltaTime;
                }
                else
                {
                    //Debug.Log(shortestPath.Count);
                    interval = AIInterval;
                    ChangePosition();
                }
            }else if(shortestPath.Count == 0)
            {
                foreach (Transform child in transform)
                {
                    gridGenerator.setAIGraph((int)child.position.x - offset, (int)child.position.y, 2);
                }
            }
        }
        //SyncPosition();
        if (isNewFruit)
        {

            if (nextSearchCounter > 0)
            {
                nextSearchCounter -= Time.deltaTime;
            }
            else
            {
                //Debug.Log("head position is:" + transform.Find("Head").position.ToString());
                Astar(transform.Find("Head").position, GameObject.FindWithTag("AIFruit").transform.position);

                isNewFruit = false;
                nextSearchCounter = nextSearchTime;
            }
        }
    }
    private void LateUpdate()
    {

    }

    private void SyncPosition()
    {
        //Debug.Log(GameObject.FindWithTag("AIFruit").transform.position);
        int cnt = 0;
        for (int i = 0; i < AIgraph.GetLength(0); i++)
        {
            for (int j = 0; j < AIgraph.GetLength(1) - 1; j++)
            {
                if (AIgraph[i, j] == 2)
                {
                    gridGenerator.setAIGraph(i, j, 0);
                    cnt++;
                }
            }
        }
        //Debug.Log(cnt);

        foreach (Transform child in transform)
        {
            gridGenerator.setAIGraph((int)child.position.x - offset, (int)child.position.y, 2);
        }
    }

    private void ChangePosition()
    {
        
        foreach (Transform child in transform)
        {
            //if((int)child.position.x == (int)OldAIsnake[0].x && (int)child.position.y == (int)OldAIsnake[0].y)
            if(Mathf.Abs(child.position.x - OldAIsnake[0].x) < 0.001 && Mathf.Abs(child.position.y - OldAIsnake[0].y) < 0.001)
            //if(child.position == OldAIsnake[0])
            {
                //if (shortestPath.Count == 1) nextAstarStartPos = shortestPath[0];

                Vector3 des = shortestPath[shortestPath.Count - 1];
                des.x = (int)des.x + offset;
                des.y = (int)des.y;
                shortestPath.RemoveAt(shortestPath.Count - 1);
                Vector3 nextDirection = des - AIsnake[0];

                gridGenerator.setAIGraph((int)(AIsnake[0].x - offset), (int)AIsnake[0].y, 0);


                nextDirection.x = Mathf.RoundToInt(nextDirection.x);
                nextDirection.y = Mathf.RoundToInt(nextDirection.y);
                nextDirection.z = Mathf.RoundToInt(nextDirection.z);

                child.Translate(nextDirection);

                des.x = Mathf.RoundToInt(des.x);
                des.y = Mathf.RoundToInt(des.y);
                des.z = Mathf.RoundToInt(des.z);

                AIsnake[0] = des;

                gridGenerator.setAIGraph((int)(AIsnake[0].x - offset), (int)AIsnake[0].y, 2);
            }

            for(int i = 1; i < AIsnake.Count; i++)
            {
                if (Mathf.Abs(child.position.x - OldAIsnake[i].x) < 0.001 && Mathf.Abs(child.position.y - OldAIsnake[i].y) < 0.001)
                //if(child.position == OldAIsnake[i])
                {
                    gridGenerator.setAIGraph((int)(OldAIsnake[i].x - offset), (int)OldAIsnake[i].y, 0);

                    child.position = OldAIsnake[i - 1];

                    AIsnake[i] = OldAIsnake[i - 1];

                    gridGenerator.setAIGraph((int)(OldAIsnake[i - 1].x - offset), (int)OldAIsnake[i - 1].y, 2);
                }
            }
        }

        for (int i = 0; i < AIsnake.Count; i++)
        {
            OldAIsnake[i] = AIsnake[i];
        }
        //SyncPosition();
    }

    private Dictionary<Vector3, Vector3> Astar(Vector3 start, Vector3 end)
    {
        shortestPath.Clear();

        AIgraph = gridGenerator.getAIGraph;

        start.x -= offset;
        end.x -= offset;

        start.x = Mathf.RoundToInt(start.x);
        start.y = Mathf.RoundToInt(start.y);
        start.z = Mathf.RoundToInt(start.z);

        end.x = Mathf.RoundToInt(end.x);
        end.y = Mathf.RoundToInt(end.y);
        end.z = Mathf.RoundToInt(end.z);

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
                shortestDis = 0;
                while (parent.ContainsKey(tmp))
                {
                    shortestPath.Add(tmp);
                    tmp = parent[tmp]; 
                    shortestDis++;
                }

                //Debug.Log("----------------------------------------------------------------");
                //Debug.Log("start:" + start.ToString() + "to " + end.ToString());
                foreach (var ele in shortestPath)
                {
                    //Debug.Log(ele);
                }
                nextAstarStartPos = shortestPath[0];
                nextAstarStartPos.x += offset;
                //Debug.Log("next start:" + nextAstarStartPos.ToString());
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

        return parent;
    }

    private void GrowTail()
    {

        Vector3 growDirection = AIsnake[AIsnake.Count - 1] - AIsnake[AIsnake.Count - 2];
        Vector3 growPosition = AIsnake[AIsnake.Count - 1] + growDirection;

        growPosition.x = (int)growPosition.x;
        growPosition.y = (int)growPosition.y;
        growPosition.z = (int)growPosition.z;


        AIsnake.Add(growPosition);
        OldAIsnake.Add(growPosition);
        gridGenerator.setAIGraph((int)growPosition.x - offset, (int)growPosition.y, 2);
        Instantiate(tail, growPosition, Quaternion.identity, transform);
    }
}
