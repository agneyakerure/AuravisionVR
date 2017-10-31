using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float maxDistance = 30;

    public GameManager gm;

    Vector3 initPos;

	// Use this for initialization
	void Start () {
        initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // difference in all coordinate
        float diffX = Math.Abs(initPos.x - transform.position.x);
        float diffY = Math.Abs(initPos.y - transform.position.y);
        float diffZ = Math.Abs(initPos.z - transform.position.z);

        // destroy if it's too far away
        if(diffX >= maxDistance || diffY >= maxDistance || diffZ >= maxDistance)
        {
            gameObject.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        // check if we hit an enemy
        if(other.CompareTag("Enemy"))
        {
            //other.gameObject.GetComponent<EnemyController>().KillEnemy();
            //Debug.Log("Enemy");
            other.gameObject.GetComponent<HealthController>().ApplyDamage(100f);
        }
        // check if we hit the graffiti
        else if(other.CompareTag("Graffiti")) {            
            gm.InitGame();
        }

        gameObject.SetActive(false);
    }
}
