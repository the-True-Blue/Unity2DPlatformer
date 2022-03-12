using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody2D _rigidBody;
    Vector2 startPosition;
    Collider2D _collider;
    SpriteRenderer _renderer;
    Color clear;
    Color startColor;
    
    /*
    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    } */

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigidBody.isKinematic = true;
        startPosition = transform.position;
        startColor = _renderer.color;
        clear = startColor;
        clear.a = 0;        
    }

    void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if (other.gameObject.CompareTag("Player")) {
            _rigidBody.isKinematic = false;
            //StopAllCoroutines();
            //StartCoroutine(Fall());
        } 
    }

    IEnumerator Fall() {
        yield return new WaitForSeconds(.1f);
        _rigidBody.isKinematic = false;
        
        yield return new WaitForSeconds(10f);
        _rigidBody.isKinematic = true;
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

