using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    // movement range
    public float rangeH = 5;
    public float rangeV = 1;

    // speed
    public float speed = 2;

    // material for dead enemies
    public Material deadMaterial;

    // direction
    int direction = 1;

    // accumulated movement
    float accMovement = 0;

    // available states
    enum State { MovingHorizontally, MovingVertically, Dead};

    // keep track of the current state
    State currState;

    // Game Manager
    GameManager gm;

    // Enemy Manager
    EnemyManager em;

	// Use this for initialization
	void Start () {
        // initial state
        currState = State.MovingHorizontally;

        // game manager
        gm = GameObject.FindObjectOfType<GameManager>();

        // log error if it wasn't found
        if (gm == null)
        {
            Debug.LogError("there needs to be an GameManager in the scene");
        }

        // enemy manager
        em = GameObject.FindObjectOfType<EnemyManager>();

        // log error if it wasn't found
        if (em == null)
        {
            Debug.LogError("there needs to be an EnemyManager in the scene");
        }
    }
	
	// Update is called once per frame
	void Update () {
        // nothing happens if the enemy is dead
        if (currState == State.Dead) return;

        // calculate movement  v = d / t --> d = v * t
        float movement = speed * Time.deltaTime;

        // update accumulate movement
        accMovement += movement;

        // are we moving horizontally?
        if (currState == State.MovingHorizontally)
        {
            // if yes, then transition to moving vertically
            if(accMovement >= rangeH)
            {
                // transition to moving vertically
                currState = State.MovingVertically;

                // reverse direction (for horizontal movement)
                direction *= -1;

                // reset acc movement
                accMovement = 0;
            }
            // if not, move the invader horizontally
            else
            {
                transform.position += transform.forward * movement * direction;
            }
        }
        // this is, if we are moving vertically
        else
        {
            // if yes, then transition to moving horizontally
            if (accMovement >= rangeV)
            {
                // transition to moving horiz
                currState = State.MovingHorizontally;

                // reset acc movement
                accMovement = 0;
            }
            // if not, move the invader vertically
            else
            {
                transform.position += Vector3.down * movement;
            }
        }
	}

    // is called when the enemy is shot
    public void KillEnemy()
    {
        // nothing will happen if already dead
        if (currState == State.Dead) return;

        // set the state to dead
        currState = State.Dead;

        // remove kinematic
        GetComponent<Rigidbody>().isKinematic = false;

        // remove trigger
        GetComponent<Collider>().isTrigger = false;

        // change material
        gameObject.GetComponentInChildren<Renderer>().sharedMaterial = deadMaterial;

        // decrease number of enemies
        em.numEnemies--;
        
        // check winning condition
        gm.HandleEnemyDead();
    }

    void OnTriggerEnter(Collider other)
    {
        // nothing will happen if already dead
        if (currState == State.Dead) return;

        //check if the enemy hit the player
        if (other.CompareTag("Player Body"))
        {
            gm.GameOver();
        }

        //check if the enemy reached the floor
        else if (other.CompareTag("Ground"))
        {
            gm.GameOver();
        }
    }
}
