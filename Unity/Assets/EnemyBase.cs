﻿using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TriggerEnter(Collider col){
		Destroy (gameObject);
	}
}
