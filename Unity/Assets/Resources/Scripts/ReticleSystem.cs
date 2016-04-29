using UnityEngine;
using System.Collections;

public class ReticleSystem : MonoBehaviour {
	Camera camera;
	// Use this for initialization
	void Start () {
		StartCoroutine (lockOn ());
	}
	public IEnumerator lockOn(){
		RaycastHit hit;
		while (!GameManager.GameOver) {
			//GetComponent<RectTransform> ().localScale += new Vector3(0.01f,0.01f,0);
			//Vector3 a = camera.ViewportToWorldPoint(transform.position);
			if(Physics.Raycast(transform.position,transform.forward,out hit,30000)){
				//Debug.Log (hit.transform.name);
			}
			yield return null;
		}
	}
}
