using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public float rotateSpeed = 3f;
    private bool isSpinning = false;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(0f, rotateSpeed, 0f);
            StartCoroutine(slowDown());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<SpeedPlayerController>().startPos = transform.position;
            isSpinning = true;
        }
    }

    IEnumerator slowDown()
    {
        yield return new WaitForSeconds(1);
        while (rotateSpeed > 0)
        {
            rotateSpeed -= 0.5f;
        }
        if (rotateSpeed == 0f)
        {
            isSpinning = false;
            transform.rotation = startRotation;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
