using UnityEngine;
using System.Collections;

public class missile : MonoBehaviour {
	private float speed = 60f;
	private Vector3 move;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public IEnumerator straight(){
		float times = 0f;
		while(times < 0.5f){
			times += Time.deltaTime;
			yield return null;
		}
		times = 0f;
		while(true){
			times+=Time.deltaTime;
			if (times >= 10) {
				Destroy (gameObject);
				Destroy (this);
			}
			transform.Translate (Vector3.back * Time.deltaTime * speed);
			yield return null;
		}
	}

}
