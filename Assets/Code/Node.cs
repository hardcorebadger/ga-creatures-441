using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public float threshold = 0f;
	public Dictionary<Node, float> inputs;
	public float value = -1f;
	public bool isTerminal = false;

	public Node(float t) {
		threshold = t;
		inputs = new Dictionary<Node, float> ();
	}

	public Node SetTerminalValue(float v) {
		value = v;
		isTerminal = true;
		return this;
	}

	public Node AddInput(Node n, float weight) {
		inputs.Add (n, weight);
		isTerminal = false;
		return this;
	}

	public void ForwardProp() {
		if (isTerminal)
			return;
		
		value = threshold;
		foreach (Node n in inputs.Keys) {
			value += n.value * inputs [n];
		}
		value = (2 / (1 + Mathf.Exp (-1 * value))) - 1;
	}

}
