using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleReticle : MonoBehaviour
{
	private RectTransform Reticle;
	private static List<MultipleReticle> Reticles = new List<MultipleReticle>();

	private GameObject _lockOnTgt;
	public GameObject LockOn {
		set {
			if (value != null) {
				_lockOnTgt = value;
				StartCoroutine (ReticleMoveToTgt ());
				StartCoroutine (LockOnCanceler ());
			} else {
				
			}
		}
	}



	void Awake ()
	{
		Reticle = gameObject.GetComponent<RectTransform> ();
		transform.SetParent(GameObject.Find ("Canvas").transform,false);
		Reticles.Add (gameObject.GetComponent<MultipleReticle>());
	}

	public static IEnumerator AllReleaceReticle(){
		foreach (MultipleReticle reticle in Reticles) {
			reticle.StartCoroutine (reticle.ForciblyRelaseLock());
		}
		Reticles.Clear ();
		yield return null;
	}

	private IEnumerator ReticleMoveToTgt ()
	{
		while (!GameManager.GameOver) {
			Reticle.position = RectTransformUtility.WorldToScreenPoint (Camera.main, _lockOnTgt.transform.position);
			yield return null;
		}
	}

	private IEnumerator LockOnCanceler ()
	{
		while (_lockOnTgt != null && !GameManager.GameOver) {
			if (ReticleIsOutOfScreen ()) {
				StartCoroutine (ForciblyRelaseLock());
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator ForciblyRelaseLock ()
	{
		StopAllCoroutines ();
		_lockOnTgt = null;
		gameObject.GetComponent<AudioSource> ().Play ();
		StartCoroutine (Deth());
		yield return null;
	}

	private bool ReticleIsOutOfScreen ()
	{
		if (Reticle.position.x < 0 || Reticle.position.x > Screen.width || Reticle.position.y < 0 || Reticle.position.y > Screen.height) {
			return true;
		}
		return false;
	}

	private IEnumerator Deth(){
		yield return new WaitForSeconds (0.4f);
		DestroyImmediate (gameObject);
		yield return null;
	}
}
