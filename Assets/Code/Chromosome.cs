using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome {

	// just to hold the weights for the NN and be bascially
	// just a place to put the data

	public float[] weights;
	public static int weightAmount = 40;
	public static int mutationLikelihood = 50;
	public static float initialWeightMin = -1f;
	public static float initialWeightMax = 1f;

	public Chromosome() {
		weights = new float[weightAmount];
	}

	public Chromosome Randomize() {
		for (int i = 0; i < weightAmount; i++) {
			weights [i] = Random.Range (initialWeightMin, initialWeightMax);
		}
		return this;
	}

	public static Chromosome Breed(Chromosome c1, Chromosome c2) {
		
		// crossover
		Chromosome c = new Chromosome();
		int crossover = Random.Range (0, weightAmount);

		for (int i = 0; i < weightAmount; i++) {
			if (i < crossover)
				c.weights [i] = c1.weights [i];
			else
				c.weights [i] = c2.weights [i];
		}

		// mutate
		int mutation = Random.Range (0, weightAmount);
		if (Random.Range (0, 100) < mutationLikelihood)
			c.weights [mutation] = Mutate (c.weights [mutation]);
	
		return c;
	}

	private static float Mutate(float f) {
		// idk
		return f - 1;
        //return Random.Range(initialWeightMin, initialWeightMax);

    }

}
