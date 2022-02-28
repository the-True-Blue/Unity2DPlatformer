using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformBackForth : MonoBehaviour
{
    public int speed = 10;
    private float startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = transform.position;
        newPosition.x = Mathf.SmoothStep(startPosition, startPosition + 5*speed, Mathf.PingPong(Time.time * 0.75f * speed, 1));
        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.SetParent(null);
        }
    }

}
