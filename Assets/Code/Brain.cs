using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain {

	Creature creature;

	public Brain(Creature c) {
		creature = c;
	}

	public void Think() {
		// use the public input values in creature
		// smash those together with the public creature chromosome
		// -----> (MAGIC FORWARD PROP NN) ---->
		// set the public output variables in creature

		// This is just a plug
		Stochastic();
	}

	public void Stochastic() {
		creature.moveX = Random.Range(-8f,8f);
		creature.moveZ = Random.Range(-8f,8f);
		creature.mouth = Random.Range(0f,1f);
	}

}
