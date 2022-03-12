using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float moveSpeed = 7f;
    private GameObject target;

    private Vector2 direction;
    private Rigidbody2D body;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        body = GetComponent<Rigidbody2D>();
        direction = (target.transform.position - transform.position).normalized * moveSpeed;
        body.velocity = new Vector2(direction.x, direction.y);
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (collision.GetComponent<SpeedPlayerController>().canHit) 
            { 
                GameControl.control.hp -= 25;
                collision.transform.GetChild(1).GetComponent<AudioSource>().Play(); //playerhit sound
            }
            Destroy(gameObject);
        } 
    }
}
