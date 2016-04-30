using UnityEngine;
using System.Collections;



public class ReticleSystem : MonoBehaviour {
	////Corgi(8)レイヤーとだけ衝突する
	//int layerMask = 1 << 8;
	////Corgi(8)、Shiba(9)レイヤーとだけ衝突する
	//int layerMask = 1 << 8 | 1 << 9;
	////Corgi(8)レイヤーとだけ衝突しない
	//int layerMask = ~(1 << 8);
	public static GameObject lockOnTgt;
	private float lockNow = 0f;
	public Camera MainCamera, UICamera;
	public GameObject reticleUI;
	// Use this for initialization
	void Start () {
		StartCoroutine (serchEnemy ());
		StartCoroutine (releaseLock ());
	}

	public IEnumerator serchEnemy(){
		RaycastHit hit;
		int layerMask = 1 << 11 | 1 << 12;
		while (!GameManager.GameOver) {
			var ray = RectTransformUtility.ScreenPointToRay (Camera.main,new Vector3(294.0f, 157.5f, 0.0f));
			if(Physics.Raycast(ray,out hit,30000,layerMask) && lockOnTgt == null){
				LockNow = hit.transform.gameObject;
			}
			yield return null;
		}
	}

	public GameObject LockNow{
		set{
			if (Time.deltaTime >= 0.4f) {
				Debug.Log (lockNow);
				lockNow = 0f;
			} else {
				lockNow += Time.deltaTime;
				LockOn (value);
			}
		}get{
			return null;
		}
	}
	public void LockOn(GameObject tgt){
		if (lockNow >= 1.5f) {
			lockOnTgt = tgt;
			lockNow = 0;
		}
	}

	public IEnumerator releaseLock(){
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.Alpha1) && lockOnTgt != null) {
				lockOnTgt = null;
			}
			yield return null;
		}
	}
}

