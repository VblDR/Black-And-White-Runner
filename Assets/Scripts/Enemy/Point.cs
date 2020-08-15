using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject particle;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(particle, transform);
        if (collision.CompareTag("Player"))
        {

            gameManager.IncreaseScore();

        }
    }
}
