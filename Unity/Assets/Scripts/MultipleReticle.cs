using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleReticle : MonoBehaviour
{
	private RectTransform Reticle;
//	private static List<MultipleReticle> Reticles = new List<MultipleReticle>();

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
	}

	public static IEnumerator AllReleaceReticle(){
		foreach (MultipleReticle reticle in FindObjectsOfType<MultipleReticle>()) {
			reticle.ForciblyRelaseLock(true);
		}
		ReticleSystem.MultiMissileLockOn.Clear ();
		yield return null;
	}

	private IEnumerator ReticleMoveToTgt ()
	{
		while (!GameManager.GameOver && _lockOnTgt != null) {
			Reticle.position = RectTransformUtility.WorldToScreenPoint (Camera.main, _lockOnTgt.transform.position);
			yield return null;
		}
		ForciblyRelaseLock (false);
	}

	private IEnumerator LockOnCanceler ()
	{
		while (_lockOnTgt != null && !GameManager.GameOver) {
			if (ReticleIsOutOfScreen ()) {
				ForciblyRelaseLock(false);
				yield return null;
			}
			yield return null;
		}
	}

	private void ForciblyRelaseLock (bool isSilent)
	{
		StopAllCoroutines ();
		if (!isSilent) {
			gameObject.GetComponent<AudioSource> ().Play ();
		}
		DestroyImmediate (gameObject);
	}

	private bool ReticleIsOutOfScreen ()
	{
		if (Reticle.position.x < 0 || Reticle.position.x > Screen.width || Reticle.position.y < 0 || Reticle.position.y > Screen.height) {
			return true;
		}
		return false;
	}
}
