using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float initSpeed = 1, maxSpeed = 3;
    private float speed;

    public float timeBetweenObstacles, minTimeBetweentObstacles;
    private float coolDown;

    public GameObject whiteBlock, blackBlock;

    public Transform border;
    private Vector2 spawnPointUp, spawnPointDown;

    private void Start()
    {
        speed = initSpeed;
        spawnPointUp = new Vector2(transform.position.x, transform.position.y + border.localScale.y / 2 + 0.01f);
        spawnPointDown = new Vector2(transform.position.x, transform.position.y - border.localScale.y / 2 - 0.01f);
        coolDown = timeBetweenObstacles;
    }

    private void Update()
    {
        if (coolDown <= 0)
        {
            SpawnObstacle();
            coolDown = timeBetweenObstacles;
        }
        else coolDown -= Time.deltaTime;
    }


    private void SpawnObstacle()
    {
        GameObject obstacle;
        if(Random.Range(0, 2) == 0)
        {
           obstacle = Instantiate(whiteBlock, spawnPointUp, Quaternion.identity);
            
        }
        else
        {
            obstacle = Instantiate(blackBlock, spawnPointDown, Quaternion.identity);
        }
        obstacle.GetComponent<Block>().speed = speed;
        if(speed != maxSpeed) speed += 0.01f;
        if (timeBetweenObstacles > minTimeBetweentObstacles) timeBetweenObstacles -= 0.05f;

    }
}
