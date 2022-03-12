using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bullet;
    private GameObject target;

    public float fireRate = 5f;
    public float fireRange = 30f;
    private float nextFire;
    
    void Start()
    {
        nextFire = Time.time;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        checkFireTime();
    }

    void checkFireTime()
    {
        float targetRange = Vector2.Distance(target.transform.position, transform.position);
        if (Time.time > nextFire && targetRange <= fireRange)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    }
}
