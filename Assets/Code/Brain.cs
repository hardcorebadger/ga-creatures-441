using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain {

	Creature creature;

	// inputs
	Node creatureX, creatureZ, foodX, foodZ, energy, random;

	// hidden
	Node h1, h2, h3, h4;

	// outputs
	Node moveX, moveZ, mouth, attack;

	public Brain(Creature c) {
		creature = c;

        // initialize input values
        creatureX = new Node (0f).SetTerminalValue(creature.creatureX);
        creatureZ = new Node (0f).SetTerminalValue(creature.creatureZ);
        foodX = new Node (0f).SetTerminalValue(creature.foodX);
		foodZ = new Node (0f).SetTerminalValue(creature.foodZ);

        // initialize and link hidden layer
        h1 = new Node (creature.GetWeight(0)).AddInput (creatureX, creature.GetWeight(1)).AddInput (creatureZ, creature.GetWeight(2)).AddInput (foodX, creature.GetWeight(3)).AddInput (foodZ, creature.GetWeight(4));
        h2 = new Node (creature.GetWeight(5)).AddInput (creatureX, creature.GetWeight(6)).AddInput (creatureZ, creature.GetWeight(7)).AddInput (foodX, creature.GetWeight(8)).AddInput (foodZ, creature.GetWeight(9));
        h3 = new Node (creature.GetWeight(10)).AddInput (creatureX, creature.GetWeight(11)).AddInput (creatureZ, creature.GetWeight(12)).AddInput (foodX, creature.GetWeight(13)).AddInput (foodZ, creature.GetWeight(14));
        h4 = new Node (creature.GetWeight(15)).AddInput (creatureX, creature.GetWeight(16)).AddInput (creatureZ, creature.GetWeight(17)).AddInput (foodX, creature.GetWeight(18)).AddInput (foodZ, creature.GetWeight(19));
        //h1 = new Node(creature.GetWeight(0)).AddInput(foodX, creature.GetWeight(1)).AddInput(foodZ, creature.GetWeight(2));
        //h2 = new Node(creature.GetWeight(3)).AddInput(foodX, creature.GetWeight(4)).AddInput(foodZ, creature.GetWeight(5));

        // initialize and link output layer
        moveX = new Node (creature.GetWeight(20)).AddInput (h1, creature.GetWeight(21)).AddInput (h2, creature.GetWeight(22)).AddInput (h3, creature.GetWeight(23)).AddInput (h4, creature.GetWeight(24));
        moveZ = new Node (creature.GetWeight(25)).AddInput (h1, creature.GetWeight(26)).AddInput (h2, creature.GetWeight(27)).AddInput (h3, creature.GetWeight(28)).AddInput (h4, creature.GetWeight(29));
        //moveX = new Node(creature.GetWeight(6)).AddInput(h1, creature.GetWeight(7)).AddInput(h2, creature.GetWeight(8));
        //moveZ = new Node(creature.GetWeight(9)).AddInput(h1, creature.GetWeight(10)).AddInput(h2, creature.GetWeight(11));
        mouth = new Node (creature.GetWeight(30)).AddInput (h1, creature.GetWeight(31)).AddInput (h2, creature.GetWeight(32)).AddInput (h3, creature.GetWeight(33)).AddInput (h4, creature.GetWeight(34));
        attack = new Node (creature.GetWeight(35)).AddInput (h1, creature.GetWeight(36)).AddInput (h2, creature.GetWeight(37)).AddInput (h3, creature.GetWeight(38)).AddInput (h4, creature.GetWeight(39));

    }

    public void Think() {
		
		// set input values
		creatureX.SetTerminalValue (creature.creatureX);
		creatureZ.SetTerminalValue (creature.creatureZ);
        creatureX.SetTerminalValue(creature.foodX); // just make it food
        creatureZ.SetTerminalValue(creature.foodZ);

        foodX.SetTerminalValue (creature.foodX);
		foodZ.SetTerminalValue (creature.foodZ);

		// forward prop
		h1.ForwardProp();h2.ForwardProp();h3.ForwardProp();h4.ForwardProp();
		moveX.ForwardProp();moveZ.ForwardProp();mouth.ForwardProp();attack.ForwardProp();
        //h1.ForwardProp(); h2.ForwardProp();
        //moveX.ForwardProp(); moveZ.ForwardProp();

        creature.moveX = creature.maxSpeed * moveX.value;
		creature.moveZ = creature.maxSpeed * moveZ.value;
		creature.mouth = (mouth.value + 1f)/2f;
		creature.attack = (attack.value + 1f)/2f;
	}

	public void Stochastic() {
		creature.moveX = Random.Range(-8f,8f);
		creature.moveZ = Random.Range(-8f,8f);
		creature.mouth = Random.Range(0f,1f);
		creature.attack = Random.Range(0f,1f);
	}

}
