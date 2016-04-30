﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {

	private const float delay = 0.15f;

	public static Queue<GameObject> missiles = new Queue<GameObject> ();

	void Start () {
		missiles.Enqueue (GameObject.Find ("missileA"));
		missiles.Enqueue (GameObject.Find ("missileB"));
		missiles.Enqueue (GameObject.Find ("missileC"));
		missiles.Enqueue (GameObject.Find ("missileD"));
        StartCoroutine(shoot());
	}

    public IEnumerator shoot()
    {
       float reloading = 0.0f;
        while (!GameManager.GameOver)
        {
            reloading += Time.deltaTime;
            if(reloading >= delay)
            {
                if ((Input.GetAxis("RTrigger") == 1 || Input.GetKeyDown(KeyCode.C)) && missiles.Count >= 1)
                {
                    StartCoroutine(missiles.Dequeue().GetComponent<missile>().straight());
                    reloading = 0f;
                }
            }
            yield return null;
        }
    }
}