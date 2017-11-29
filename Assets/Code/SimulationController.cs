using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour {

	public GameObject plantPrefab;
	public GameObject creaturePrefab;

	public int chromosomes = 10;
	public int populationPerChromosome = 10;
	public int treeCount = 100;
	public int size = 100;

	private Dictionary<Chromosome,Population> populations;
	private int creaturesLiving = 0;

	public void OnEnable() {
		populations = new Dictionary<Chromosome,Population> ();
		NewRandomPopulation ();
	}

	public void NewRandomPopulation() {
		Debug.Log ("Ranomd Population Generated");
		for (int i = 0; i < chromosomes; i++) {
			populations.Add (new Chromosome ().Randomize(),new Population(populationPerChromosome,0f));
		}
	}

	public void Run() {
		Generate ();
		// now that they exist they will tick so its running
		// they register on death their lifecycle
		// now that everyone is dead, the dictionary will hold all you need to evaluate fitness by chromosome
	}

	private void Generate() {
		Debug.Log ("Generation Started");
		// generate trees
		// generate dudes
		for (int i = 0; i < treeCount; i++) {
			Instantiate(plantPrefab, new Vector3(Random.Range(size*-1, size), 0f, Random.Range(size*-1, size)), Quaternion.identity);
		}
		foreach (Chromosome c in populations.Keys) {
			Population p = populations [c];
			for (int i = 0; i < p.amount; i++) {
				GameObject creature = Instantiate(creaturePrefab, new Vector3(Random.Range(size*-1, size), 0f, Random.Range(size*-1, size)), Quaternion.identity);
				creature.GetComponent<Creature> ().Initialize (c);
				creaturesLiving++;
			}
		}
	}

	public void Breed() {

		// get best 2 and worst 2

		// using the current map, evaluate the fitness levels
		// -----> MAGIC GENETIC ALGORITHM ------>
		// map has new chromosomes in it with lifespan set to 0
	}

	public void RegisterDeath(Chromosome c, float lifespan) {
		Population p = populations [c];
		p.totalLifespan += lifespan;
		populations [c] = p;
		creaturesLiving--;
		if (creaturesLiving <= 0)
			OnGenerationComplete ();
	}

	private void OnGenerationComplete() {
		Debug.Log ("Generation Complete");
	}

	public struct Population {
		public int amount;
		public float totalLifespan;
		public Population(int a, float l) {
			amount = a;
			totalLifespan = l;
		}
	}

}
