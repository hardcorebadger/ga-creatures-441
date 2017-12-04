using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimulationController : MonoBehaviour {

	public GameObject plantPrefab;
	public GameObject creaturePrefab;


	public int chromosomes = 10;
	public int populationPerChromosome = 10;
	public int treeCount = 100;
	public int size = 100;
	public float timescale = 1f;

	private Dictionary<Chromosome,Population> populations;
	public int creaturesLiving = 0;
	public int runningLoop = 0;

	public void OnEnable() {
		populations = new Dictionary<Chromosome,Population> ();
		NewRandomPopulation ();
		Time.timeScale = timescale;
	}

	public void NewRandomPopulation() {
		Debug.Log ("Random Population Generated");
		for (int i = 0; i < chromosomes; i++) {
			populations.Add (new Chromosome ().Randomize(),new Population(populationPerChromosome,0f));
		}
	}

	public void RunFast100() {
		RunGenerations (100);
	}

	public void RunGenerations(int i) {
		Time.timeScale = 100f;
		runningLoop = i;
		Generate ();
	}

	public void Run() {
		Time.timeScale = timescale;
		runningLoop = 0;
		Generate ();
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

		Chromosome firstPlace = populations.Keys.First();
		Chromosome secondPlace = populations.Keys.First();
		Chromosome lastPlace = populations.Keys.First();
		Chromosome secondLastPlace = populations.Keys.First();

        // get best 2 and worst 2a
        /*
		foreach (Chromosome c in populations.Keys) {
			if (populations [c].totalLifespan > populations [firstPlace].totalLifespan) {
				secondPlace = firstPlace;
				firstPlace = c;
			} else if (populations [c].totalLifespan > populations [secondPlace].totalLifespan)
				secondPlace = c;
			else if (populations [c].totalLifespan < populations [lastPlace].totalLifespan) {
				secondLastPlace = lastPlace;
				lastPlace = c;
			} else if (populations [c].totalLifespan < populations [secondLastPlace].totalLifespan)
				secondLastPlace = c;
		}
        */
        foreach (Chromosome c in populations.Keys)
        {
            if (populations[c].totalLifespan < populations[lastPlace].totalLifespan)
            {
                lastPlace = c;
            }
        } 
        populations.Remove(lastPlace);
        lastPlace = populations.Keys.First();
        foreach (Chromosome c in populations.Keys)
        {
            if (populations[c].totalLifespan < populations[lastPlace].totalLifespan)
            {
                lastPlace = c;
            }
        }
        populations.Remove(lastPlace); //last place done

        float fp = 0;
        float sp = 0;
        firstPlace = null;
        secondPlace = null; 

        foreach (Chromosome c in populations.Keys)
        {
            if (populations[c].totalLifespan > fp)
            {
                secondPlace = firstPlace;
                if (secondPlace != null) {
                    sp = populations[secondPlace].totalLifespan;
                }
                firstPlace = c;
                fp = populations[firstPlace].totalLifespan;
            }
            else if (populations[c].totalLifespan > sp)
            {
                secondPlace = c;
                if (secondPlace != null)
                {
                    sp = populations[secondPlace].totalLifespan;
                }
            }
        }

        Debug.Log("Best Chromosome: " + (populations[firstPlace].totalLifespan / populationPerChromosome));
        /*
        Debug.Log(firstPlace == secondPlace);
        Debug.Log(firstPlace == null);
        Debug.Log(secondPlace == null);
        Debug.Log(fp < sp);
        */
        
        Chromosome child1 = Chromosome.Breed (firstPlace, secondPlace);
		Chromosome child2 = Chromosome.Breed (secondPlace, firstPlace);

		// add the children
		populations.Add(child1,new Population(populationPerChromosome, 0));
		populations.Add(child2,new Population(populationPerChromosome, 0));

		Dictionary<Chromosome,Population> p = new Dictionary<Chromosome,Population> ();
		foreach (Chromosome c in populations.Keys) {
			p.Add (c, new Population (populationPerChromosome, 0));
		}
		populations = p;
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
		foreach (GameObject o in GameObject.FindGameObjectsWithTag("food")) {
			Destroy (o);
		}
		if (runningLoop > 0) {
			runningLoop--;
			Breed ();
			Generate ();
		}
	}

	public struct Population {
		public int amount;
		public float totalLifespan;
		public Population(int a, float l) {
			amount = a;
			totalLifespan = l;
		}
		public void Reset() {
			totalLifespan = 0;
		}
	}

}
