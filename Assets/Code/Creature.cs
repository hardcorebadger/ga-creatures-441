using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

	public GameObject foodPrefab;
	public float sensoryRadius = 5f;
	public float foodEnergy = 30f;
	public float attackEnergyHit = 30f;
	public float maxSpeed = 8f;
	public GameObject closestCreature;
	public GameObject closestFood;
	public float score = 0f;

	// genetics
	public Chromosome chromosome;

	// inputs
	public float creatureX;
	public float creatureZ;
	public float foodX;
	public float foodZ;
	public float energy = 120f;
	public float random;

	// outputs
	public float moveX;
	public float moveZ;
	public float mouth;
	public float attack;

	private Brain brain;

	// Use this for initialization
	void Start () {
		brain = new Brain (this);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateState ();
		brain.Think ();
		Act ();
	}

	void Act() {
		Move (Mathf.Min (maxSpeed, moveX), Mathf.Min (maxSpeed, moveZ));
		if (Random.Range (0f, 1f) < Mathf.Abs (mouth))
			Eat ();
		if (Random.Range (0f, 1f) < Mathf.Abs (attack))
			Attack ();
	}

	void UpdateState() {

		// update energy/life
		energy -= Time.deltaTime;
		score += Time.deltaTime;

		// dies
		if (energy <= 0f) {
			Instantiate (foodPrefab, transform.position, Quaternion.identity);
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
		if (closestFood != null) {
			Destroy (closestFood);
			energy += attackEnergyHit;
		}
	}

	void Attack() {
		if (closestCreature != null) {
			closestCreature.GetComponent<Creature>().energy -= attackEnergyHit;
		}
	}

	void Move(float x, float z) {
		transform.position += new Vector3 (x*Time.deltaTime, 0f, z*Time.deltaTime);
	}

}
