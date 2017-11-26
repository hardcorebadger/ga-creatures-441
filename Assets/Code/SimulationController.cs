using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour {

	public int chromosomes = 10;
	public int populationPerChromosome = 10;

	private Dictionary<Chromosome,Population> populations;

	public void OnEnable() {
		populations = new Dictionary<Chromosome,Population> ();
	}

	public void NewRandomPopulation() {
		for (int i = 0; i < chromosomes; i++) {
			populations.Add (new Chromosome (Random.Range (-10f, 10f)/*etc..*/),new Population(populationPerChromosome,0f));
		}
	}

	public void Run() {
		Generate ();
		// now that they exist they will tick so its running
		// they register on death their lifecycle
		// now that everyone is dead, the dictionary will hold all you need to evaluate fitness by chromosome
	}

	private void Generate() {
		// generate trees
		// generate dudes
	}

	public void Breed() {
		// using the current map, evaluate the fitness levels
		// -----> MAGIC GENETIC ALGORITHM ------>
		// map has new chromosomes in it with lifespan set to 0
	}

	public void RegisterDeath(Chromosome c, float lifespan) {
		Population p = populations [c];
		p.totalLifespan += lifespan;
		populations [c] = p;
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
