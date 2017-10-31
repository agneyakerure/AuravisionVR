using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    [SerializeField] private float health = 20f;

	// Use this for initialization
	public void ApplyDamage(float dmg)
    {
        health = health - dmg;
        //if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
