using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	[SerializeField] private float health = 20f;
	// Use this for initialization
	public void ApplyDamage(float damage) {
		Debug.Log("HIT!!! Damage = " + damage);
		health = health - damage;
		{
			Destroy(gameObject);
		}
	}
}
