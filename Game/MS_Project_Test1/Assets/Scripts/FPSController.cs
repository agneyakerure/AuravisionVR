using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour {

	public float speed = 2f;

	float moveFB;
	float moveLR;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		moveFB = Input.GetAxis("Vertical") * speed;
		moveLR = Input.GetAxis("horizontal") * speed;

		
	}
}
