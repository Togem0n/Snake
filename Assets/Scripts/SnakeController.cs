using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    LList<Vector3> snake = new LList<Vector3>();
    LList<Vector3> oldsnake = new LList<Vector3>();

    private float interval;
    [SerializeField] float generalInterval;
    [SerializeField] float normalInterval;
    [SerializeField] float boostlInterval;

    private bool gameStart;
    private int horizontal;
    private int vertical;
    private Vector3 nextDirection;

    private float timeToSpeedUp;
    private float timeToSpeedUpCounter;

    private bool canBoost;
    private bool isBoosting;

    void Start()
    {
        // everytime snake moves, we change the value inside
        // everytime snake eats, we push(instantiate tails) at the end

        foreach (Transform child in transform)
        {
            snake.Push(new Vector3(child.position.x, child.position.y, 0));
            oldsnake.Push(new Vector3(child.position.x, child.position.y, 0));
        }

        generalInterval = normalInterval;
        interval = generalInterval;
        gameStart = false;
        canBoost = false;
        nextDirection = Vector3.zero;

        timeToSpeedUp = 0.1f;
        timeToSpeedUpCounter = timeToSpeedUp;
    }

    void Update()
    {
        GetDirection();

        if (gameStart)
        {
            if (interval > 0)
            {
                interval -= Time.deltaTime;
            }
            else
            {
                interval = generalInterval;
                //transform.Translate(nextDirection);
                ChangeSnakeLocation();
            }
        }
    }

    private void GetDirection()
    { 
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if(horizontal == 0 && vertical == 0)
        {
            canBoost = false;
            timeToSpeedUpCounter = timeToSpeedUp;
        }
        
        if(horizontal != 0 || vertical != 0)
        {
            if (!gameStart) gameStart = true;
            canBoost = true;
            isBoosting = true;
        }

        if(horizontal != 0 && vertical != 0)
        {
            // do nothing
        }else if(horizontal != 0)
        {
            nextDirection = new Vector3(horizontal, 0, 0);
        }else if(vertical != 0)
        {
            nextDirection = new Vector3(0, vertical, 0);
        }

        if (canBoost)
        {
            timeToSpeedUpCounter -= Time.deltaTime;
        }

        if(timeToSpeedUpCounter <= 0)
        {
            generalInterval = boostlInterval;
        }
        else
        {
            generalInterval = normalInterval;
        }

        //Debug.Log(nextDirection);
    }

    private void ChangeSnakeLocation()
    {
        snake.getCurr = snake.getHead;
        oldsnake.getCurr = oldsnake.getHead;

        snake.getCurr = snake.getCurr.next;
        oldsnake.getCurr = oldsnake.getCurr.next;

        foreach (Transform child in transform)
        {
            if (child.position == snake.getCurr.value)
            {
                child.Translate(nextDirection);
            }
        }
        snake.getCurr.value = snake.getCurr.value + nextDirection;

        while (snake.getCurr.next != null)
        {
            snake.getCurr = snake.getCurr.next;
            oldsnake.getCurr = oldsnake.getCurr.next;

            foreach (Transform child in transform)
            {
                Vector3 tmp = oldsnake.getCurr.prev.value - oldsnake.getCurr.value;
                if (child.position == snake.getCurr.value)
                {
                    child.Translate(tmp);
                }
            }
            snake.getCurr.value = oldsnake.getCurr.prev.value;
        }

        //reset
        snake.getCurr = snake.getHead;
        oldsnake.getCurr = oldsnake.getHead;
        while (snake.getCurr.next != null)
        {
            snake.getCurr = snake.getCurr.next;
            oldsnake.getCurr = oldsnake.getCurr.next;

            oldsnake.getCurr.value = snake.getCurr.value;
        }

        //snake.StepThrough();
    }
}