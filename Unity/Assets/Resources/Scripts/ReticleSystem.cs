using UnityEngine;
using System.Collections;



public class ReticleSystem : MonoBehaviour {
	////Corgi(8)レイヤーとだけ衝突する
	//int layerMask = 1 << 8;
	////Corgi(8)、Shiba(9)レイヤーとだけ衝突する
	//int layerMask = 1 << 8 | 1 << 9;
	////Corgi(8)レイヤーとだけ衝突しない
	//int layerMask = ~(1 << 8);
	public Camera MainCamera, UICamera;
	RectTransform rt = null;
	public GameObject reticleUI;
	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
		StartCoroutine (lockOn ());
	}
	public IEnumerator lockOn(){
		RaycastHit hit;
		int layerMask = 1 << 11 | 1 << 12;
		while (!GameManager.GameOver) {
			var ray = RectTransformUtility.ScreenPointToRay (Camera.main,new Vector3(294.0f, 157.5f, 0.0f));
			if(Physics.Raycast(ray,out hit,30000,layerMask)){
				Debug.Log (hit.transform.name);
			}
			yield return null;
		}
	}
}

