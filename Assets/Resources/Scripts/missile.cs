using UnityEngine;
using System.Collections;

public class missile : MonoBehaviour {
	private float speed = 500f;
	private Vector3 startPos;
	private Quaternion startRot;
	private static Transform root;

	void Start () {
		startPos = transform.localPosition;
		startRot = transform.localRotation;
		root = GameObject.Find ("missiles").transform;
	}
	
	// Update is called once per frame

	public IEnumerator straight(){
		
		StartCoroutine (reloadMissile());
		float timer = 0f;
		transform.parent = null;
		while(timer < 0.5f){
			timer += Time.deltaTime;
			yield return null;
		}
		while(!GameOver){
			transform.Translate (Vector3.back * Time.deltaTime * speed);
			yield return null;
		}
	}
	private bool GameOver = false;
	private IEnumerator reloadMissile(){
		float timer = 0f;
		while (timer < 1.8f) {
			timer += Time.deltaTime;
			yield return null;
		}
		GameObject newMissile = (GameObject)Instantiate (Resources.Load("prefabs/"+gameObject.name),Vector3.zero,Quaternion.identity);
		newMissile.transform.parent = root;
		newMissile.name = name.Substring (0,8);
		newMissile.transform.localPosition = startPos;
		newMissile.transform.localRotation = startRot;
		Attack.missiles.Enqueue (newMissile);
		while (timer < 11) {
			timer += Time.deltaTime;
			if (timer >= 10) {
				GameOver = true;
				Destroy (gameObject);
			}
			yield return null;
		}
		yield return null;
	}
}
