using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 startPosition;
    Collider2D _collider;
    SpriteRenderer _renderer;
    Color clear;
    Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        rb.isKinematic = true;
        startPosition = transform.position;
        startColor = _renderer.color;
        clear = startColor;
        clear.a = 0;  
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            //rb.isKinematic = false;
            StartCoroutine(Fall());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fall() {
        yield return new WaitForSeconds(.1f);
        rb.isKinematic = false;
        
        yield return new WaitForSeconds(10f);
        rb.isKinematic = true;
        _collider.enabled = false;

        yield return StartCoroutine(ColorFade(startColor, clear));

        yield return new WaitForSeconds(1f);

        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        _collider.enabled = true;
        yield return StartCoroutine(ColorFade(clear, startColor)); 
    }

    IEnumerator ColorFade(Color a, Color b) {
        float t=0;
        while (t < 1) {
            _renderer.color = Color.Lerp(a, b, t);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
