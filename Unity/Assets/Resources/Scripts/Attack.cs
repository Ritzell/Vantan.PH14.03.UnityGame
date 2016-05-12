using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {

	struct Delay{
		public const float missileDelay = 0.15f;
		public const float gunDelay = 0.1f;
	}

	public static Queue<GameObject> missiles = new Queue<GameObject> ();

	void Start () {
		missiles.Enqueue (GameObject.Find ("missileA"));
		missiles.Enqueue (GameObject.Find ("missileB"));
		missiles.Enqueue (GameObject.Find ("missileC"));
		missiles.Enqueue (GameObject.Find ("missileD"));
        StartCoroutine(MissileShoot());
		StartCoroutine(GunShoot());
	}

    public IEnumerator MissileShoot()
    {
       float reloading = 0.0f;
        while (!GameManager.GameOver)
        {
            reloading += Time.deltaTime;
			if(reloading >= Delay.missileDelay)
            {
                if ((Input.GetAxis("RTrigger") == 1 || Input.GetKeyDown(KeyCode.C)) && missiles.Count >= 1)
                {
                    StartCoroutine(missiles.Dequeue().GetComponent<Missile>().Straight());
                    reloading = 0f;
				}else if ((Input.GetAxis("LTrigger") == 1 || Input.GetKeyDown(KeyCode.V)) && missiles.Count >= 1 && ReticleSystem.LockOnTgt != null)
				{
					StartCoroutine(missiles.Dequeue().GetComponent<Missile>().Tracking(ReticleSystem.LockOnTgt.transform));
					reloading = 0f;
				}
            }
            yield return null;
        }
    }
	public IEnumerator GunShoot(){
		float reloading = 0.0f;
		while (!GameManager.GameOver)
		{
			reloading += Time.deltaTime;
			if(reloading >= Delay.gunDelay)
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