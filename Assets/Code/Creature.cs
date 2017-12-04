using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

	public GameObject foodPrefab;
    public float sensoryRadius = 100f;

    public float eatRadius = 1f;
    public float attackRadius = 3f;
	public float foodEnergy = 30f;
	public float attackEnergyHit = 30f;
	public float maxSpeed = 8f;
	public GameObject closestCreature;
	public GameObject closestFood;
	public float score = 0f;
	public float timeout = 120f;

	// genetics
	public Chromosome chromosome;

	// inputs
	public float creatureX;
	public float creatureZ;
	public float foodX;
	public float foodZ;
	public float energy = 120f;
    public float creatureEnergy;
	public float random;

	// outputs
	public float moveX;
	public float moveZ;
    public float mouth;
    public float attack;

	private Brain brain;
	private bool isInitialized = false;

	// Use this for initialization
	public void Initialize (Chromosome c) {
		chromosome = c;
		brain = new Brain (this);
		isInitialized = true;
        attack = 0.5f;
		StartCoroutine (Timeout ());
	}

	private IEnumerator Timeout() {
		yield return new WaitForSeconds (timeout);
		energy = -1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isInitialized)
			return;
		UpdateState ();
		brain.Think ();
		Act ();
	}

	void Act() {
        //normalize vector to preserve direction
        Vector3 a = new Vector3 (moveX, 0f, moveZ).normalized;
        Move(a.x, a.z);

        //if (Random.Range (0f, 1f) < mouth)
        Eat();
		//if (Random.Range (0f, 1f) < attack)
		Attack ();
	}

	void UpdateState() {

		// update energy/life
		energy -= Time.deltaTime;
		score += Time.deltaTime;

		// dies
		if (energy <= 0f) {
			Instantiate (foodPrefab, transform.position, Quaternion.identity);
			FindObjectOfType<SimulationController> ().RegisterDeath (chromosome, score);
			Destroy (gameObject);
		}

		// update closest objects
		closestCreature = null;
		closestFood = null;
		float closestCreatureDistance = sensoryRadius + 1;
		float closestFoodDistance = sensoryRadius + 1;

		Collider[] cols = Physics.OverlapSphere (transform.position, sensoryRadius);

		foreach (Collider c in cols) {
			if (c.tag == "creature" && c.gameObject != gameObject) {
				float d = Vector3.Distance (transform.position, c.transform.position);
				if (d < closestCreatureDistance) {
					closestCreature = c.gameObject;
					closestCreatureDistance = d;
				}
			}
			if (c.tag == "food") {
				float d = Vector3.Distance (transform.position, c.transform.position);
				if (d < closestFoodDistance) {
					closestFood = c.gameObject;
					closestFoodDistance = d;
				}
			}
		}

		if (closestCreature == null) {
			creatureX = sensoryRadius + 1;
			creatureZ = sensoryRadius + 1;
		} else {
			creatureX = closestCreature.transform.position.x - transform.position.x;
			creatureZ = closestCreature.transform.position.z - transform.position.z;
		}

		if (closestFood == null) {
			foodX = sensoryRadius + 1;
			foodZ = sensoryRadius + 1;
		} else {
			foodX = closestFood.transform.position.x - transform.position.x;
			foodZ = closestFood.transform.position.z - transform.position.z;
		}

	}

	void Eat() {
		if (closestFood != null && Vector3.Distance(transform.position, closestFood.transform.position) < eatRadius) {
			Destroy (closestFood);
			energy += attackEnergyHit;
		}
	}

	void Attack() {
        if (closestCreature != null && Vector3.Distance(transform.position, closestCreature.transform.position) < attackRadius)
        {
            closestCreature.GetComponent<Creature>().energy -= attackEnergyHit * Time.deltaTime;
        }


	}

	void Move(float x, float z) {
        //vector scaled by deltaTime to work at same speed for all cpu speeds
		transform.position += new Vector3 ((x*Time.deltaTime), 0f, (z*Time.deltaTime));
    }

	public float GetWeight(int i) {
		return chromosome.weights[i];
	}

}
