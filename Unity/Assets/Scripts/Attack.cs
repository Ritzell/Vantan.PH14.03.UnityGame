using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour
{
	private const float missileDelay = 0.15f;
	private const float gunDelay = 0.1f;
	private const float MMIDelay = 1f;//MultipleMissileInterceptの省略
	ReticleSystem Reticle;


	private static Queue<GameObject> missiles = new Queue<GameObject> ();
	public static Queue<GameObject> Missiles {
		get {
			return missiles;
		}
	}

	private static bool _mmiReady;
	public static bool MMIReady {
		set {
			_mmiReady = value;
		}get {
			return _mmiReady;
		}
	}

	void Awake(){
		missiles.Enqueue (GameObject.Find ("missileA"));
		missiles.Enqueue (GameObject.Find ("missileB"));
		missiles.Enqueue (GameObject.Find ("missileC"));
		missiles.Enqueue (GameObject.Find ("missileD"));
		Reticle = GameObject.Find ("ReticleImage").GetComponent<ReticleSystem> ();
	}

	void Start ()
	{
		StartCoroutine (MultipleMissileInterceptSystem ());
		StartCoroutine (MissileShoot ());
		StartCoroutine (GunShoot ());
	}

	//略名 : MMIS 複数ミサイル迎撃システム
	public IEnumerator MultipleMissileInterceptSystem(){
		float Reloading = 30.0f;

		while(!GameManager.GameOver){
			Reloading += Time.deltaTime;
			if (Reloading >= MMIDelay) {
				if (!_mmiReady) {
					if (MMISystemBoot (Reloading)) {
						Reticle.ChangeMode (true);
						yield return null;
					} else if (MMISystemEnd (Reloading)) {
						Reloading = LockOrReset (Reloading);
						yield return null;
					} 
				} else if (MMISystemCansel()) {
					Reticle.ChangeMode (false);
					yield return null;
				}
			}
			yield return null;
		}
		yield return null;
	}

	private bool MMISystemBoot(float Reloading){
		if((Input.GetKeyDown(KeyCode.JoystickButton18) || Input.GetKeyDown(KeyCode.Space))){
			return true;
		}else{
			return false;
		}
	}

	private bool MMISystemEnd(float Reloading){
		if ((Input.GetKeyUp (KeyCode.JoystickButton18) || Input.GetKeyUp (KeyCode.Space))) {
				return true;
			} else {
				return false;
			}
	}

	private float LockOrReset(float Reloading){
		if (ReticleSystem.MultiMissileLockOn.Count <= 0) {
			Reticle.ChangeMode (false);
			return 0;
		} else {
			Reticle.MMIReady ();
			return Reloading;
		}
	}

	private bool MMISystemCansel(){
		if ((Input.GetKeyUp (KeyCode.JoystickButton18) || Input.GetKeyUp (KeyCode.Escape))) {
			return true;
		} else {
			return false;
		}
	}

	private void MMISEnd(){

	}

	public IEnumerator MissileShoot ()
	{
		float Reloading = 0.0f;
		while (!GameManager.GameOver) {
			Reloading += Time.deltaTime;
			if (Reloading >= missileDelay) {
				if ((Input.GetAxis ("RTrigger") == 1 || Input.GetKeyDown (KeyCode.C)) && missiles.Count >= 1) {
					GameManager.MissileCounter = 1;
					StartCoroutine (missiles.Dequeue ().GetComponent<Missile> ().StraightToTgt (true));
					Reloading = 0f;
				} else if ((Input.GetAxis ("LTrigger") == 1 || Input.GetKeyDown (KeyCode.V)) && missiles.Count >= 1 && ReticleSystem.LockOnTgt != null) {
					StartCoroutine (missiles.Dequeue ().GetComponent<Missile> ().TrackingPlayer (ReticleSystem.LockOnTgt.transform));
					Reloading = 0f;
				}
			}
			yield return null;
		}
	}

	public IEnumerator GunShoot ()
	{
		float Reloading = 0.0f;
		while (!GameManager.GameOver) {
			Reloading += Time.deltaTime;
			if (Reloading >= gunDelay) {
				if ((Input.GetKey (KeyCode.JoystickButton12) || Input.GetKey (KeyCode.F))) {
					StartCoroutine (GameObject.Find ("guns").GetComponent<Gun> ().Shoot ());
					Reloading = 0f;
				}
			}
			yield return null;
		}
		yield return null;
	}
}