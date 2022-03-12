using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathInteract : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if(GameControl.control.deadEnemies.Count == 0) {
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.transform.position = collision.GetComponent<SpeedPlayerController>().startPos;
                GameControl.control.livesCounter -= 1;
                GameControl.control.hp = 100;
            }
            else {
                for (int i = 0; i < GameControl.control.deadEnemies.Count; i++)
                {
                    GameControl.control.deadEnemies[i].SetActive(true);
                }
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.transform.position = collision.GetComponent<SpeedPlayerController>().startPos;
                GameControl.control.deadEnemies.Clear();
                GameControl.control.livesCounter -= 1;
                GameControl.control.hp = 100;
            }
        }
    }
}
