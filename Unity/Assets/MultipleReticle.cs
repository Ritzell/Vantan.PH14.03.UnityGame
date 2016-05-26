using UnityEngine;
using System.Collections;

public class MultipleReticle : MonoBehaviour {
	private RectTransform Reticle;

	private GameObject _lockOnTgt;
	public GameObject LockOn {
		set {
			_lockOnTgt = value;
			StartCoroutine (ReticleMoveToTgt ());
			StartCoroutine (ForciblyRelaseLock ());
		}
	}

	void Awake(){
		Reticle = gameObject.GetComponent<RectTransform> ();
		transform.parent = GameObject.Find ("Canvas").transform;
	}
	
	private IEnumerator ReticleMoveToTgt(){
		while (!GameManager.GameOver) {
			Reticle.position = RectTransformUtility.WorldToScreenPoint (Camera.main, _lockOnTgt.transform.position);
			yield return null;
		}
	}
	private IEnumerator ForciblyRelaseLock ()
	{
		while (_lockOnTgt != null && !GameManager.GameOver) {
			if (ReticleIsOutOfScreen ()) {
				gameObject.GetComponent<AudioSource> ().Play ();
				yield return new WaitForSeconds (0.5f);
				_lockOnTgt = null;
				Destroy (gameObject);
				yield return null;
			}
			yield return null;
		}
		yield return null;
	}

	private bool ReticleIsOutOfScreen ()
	{
		if (Reticle.position.x < 0 || Reticle.position.x > Screen.width || Reticle.position.y < 0 || Reticle.position.y > Screen.height) {
			return true;
		}
		return false;
	}

}
