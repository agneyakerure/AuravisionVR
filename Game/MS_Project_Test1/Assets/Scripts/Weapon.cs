using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public int bulletsPerMag = 30;
	public int bulletsLeft = 200;

	public int currentBullets;

	public float range = 100;
	public float fireRate = 0.1f;
	public Transform shootPoint;

	public GameObject bulletImpact;
	public float damage = 100f;

	float fireTimer;
	// Use this for initialization
	void Start () {
		currentBullets = bulletsPerMag;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1")) {
			Fire();

		}
		if(fireTimer < fireRate) {
			fireTimer += Time.deltaTime;
		}
	}

	private void Fire() {
		if(fireTimer < fireRate){
			return;
		}

		RaycastHit hit;
		//Debug.Log(shootPoint.position);

		if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)) {
			Debug.Log("Fired! : " + hit.transform.name);
			GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
			Debug.Log("THIS : " + bulletHole);
			if(hit.transform.GetComponent<HealthController>()) {
				hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
			}
			Destroy(bulletHole, 1.5f);
		}

		currentBullets --;
		fireTimer = 0.0f; //Reset

	}
}
