using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    private GameManager gameManager;

    private void Update()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        rb.AddForce(new Vector2(-1, 0) * speed * Time.deltaTime, ForceMode2D.Impulse);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            gameManager.EndGame();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Deleter")) Destroy(gameObject);
    }
}
