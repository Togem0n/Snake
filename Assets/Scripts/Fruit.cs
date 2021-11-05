using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private FruitGenerator fruitGenerator;
    void Start()
    {
        GameEvents.current.onFruitGotEaten += DestoryMyself;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            //fruitGenerator.GenerateFruit();
            Destroy(gameObject);
        }
    }

    private void DestoryMyself()
    {
        GameEvents.current.onFruitGotEaten -= DestoryMyself;
        Destroy(transform.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameEvents.current.FruitGotEaten();
        }
    }

}
