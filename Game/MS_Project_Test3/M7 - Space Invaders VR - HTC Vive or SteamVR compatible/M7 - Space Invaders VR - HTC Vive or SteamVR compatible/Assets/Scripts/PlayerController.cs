using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZenvaVR;

public class PlayerController : MonoBehaviour {

    // Bullet velocity
    public float bulletSpeed = 10;

    // Frequency
    public float timeBetweenShots = 1;

    // Gun
    public GameObject leftGun;
    public GameObject rightGun;


    // Game Manager
    GameManager gm;

    // Object pool
    ObjectPool bulletPool;

    // Time until you can shoot again
    float remainingTimeLeft = 0;
    float remainingTimeRight = 0;

    void Awake()
    {
        gm = GameObject.FindObjectOfType<GameManager>();

        // grab the pool component
        bulletPool = GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update () {
        // Get user input
        HandleInput();
	}

    void HandleInput()
    {
        if(Input.GetAxis("Fire1") > 0 && remainingTimeLeft == 0)
        {
            Shoot(leftGun);

            //reset time
            remainingTimeLeft = timeBetweenShots;
        }
        if(Input.GetAxis("Fire2") > 0 && remainingTimeRight == 0)
        {
            Shoot(rightGun);

            //reset time
            remainingTimeRight = timeBetweenShots;
        }

        //update remaining time
        remainingTimeLeft = Mathf.Max(0, remainingTimeLeft - Time.deltaTime);
        remainingTimeRight = Mathf.Max(0, remainingTimeRight - Time.deltaTime);
    }

    void Shoot(GameObject gun)
    {
        // spawn a new bullet
        GameObject newBullet = bulletPool.GetObj();

        // pass the game manager
        newBullet.GetComponent<BulletController>().gm = gm;

        // position will be that of the gun
        newBullet.transform.position = gun.transform.position;

        // get rigid body
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();

        // give the bullet velocity
        bulletRb.velocity = gun.transform.forward * bulletSpeed;
    }
}
