using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {

	struct delay{
		public const float missileDelay = 0.15f;
		public const float gunDelay = 0.1f;
	}

	public static Queue<GameObject> missiles = new Queue<GameObject> ();

	void Start () {
		missiles.Enqueue (GameObject.Find ("missileA"));
		missiles.Enqueue (GameObject.Find ("missileB"));
		missiles.Enqueue (GameObject.Find ("missileC"));
		missiles.Enqueue (GameObject.Find ("missileD"));
        StartCoroutine(missileShoot());
		StartCoroutine(gunShoot());
	}

    public IEnumerator missileShoot()
    {
       float reloading = 0.0f;
        while (!GameManager.GameOver)
        {
            reloading += Time.deltaTime;
			if(reloading >= delay.missileDelay)
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
	public IEnumerator gunShoot(){
		float reloading = 0.0f;
		while (!GameManager.GameOver)
		{
			reloading += Time.deltaTime;
			if(reloading >= delay.gunDelay)
			{
				if ((Input.GetKey(KeyCode.JoystickButton12) || Input.GetKey(KeyCode.F)))
				{
					StartCoroutine (GameObject.Find ("guns").GetComponent<gun> ().shoot ());
					reloading = 0f;
				}
			}
			yield return null;
		}
		yield return null;
	}
}