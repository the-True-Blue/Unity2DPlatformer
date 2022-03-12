using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    public GameObject targetIcon;

    public bool targeted = false;
    private bool iAmBounce = false;

    void Start()
    {
        targetIcon.SetActive(false);
    }
    void Update()
    {
        if (targeted)
        {

            targetIcon.SetActive(true);
            targetIcon.transform.Rotate(0f, 0f, 0.5f);
            if (iAmBounce)
            {
                targetIcon.SetActive(false);
                StartCoroutine(nowTargetable());
            }
        }
        else { targetIcon.SetActive(false); }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && gameObject.name.Contains("Enemy"))
        {
            Debug.Log("enemy was hit!");
            GetComponent<AudioSource>().Play();
            targeted = false;
            collision.GetComponent<SpeedPlayerController>().lastHit = "Enemy";
            collision.GetComponent<SpeedPlayerController>().hit = true;
            GameControl.control.deadEnemies.Add(this.gameObject);
            this.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") && gameObject.name.Contains("BouncePad"))
        {
            iAmBounce = true;
            Debug.Log("bounce pad was hit!");
            targeted = false;
            collision.GetComponent<SpeedPlayerController>().lastHit = "BouncePad";
            collision.GetComponent<SpeedPlayerController>().hit = true;
        }
    }

    IEnumerator nowTargetable()
    {
        yield return new WaitForSeconds(1);
        iAmBounce = false;
    }


}
