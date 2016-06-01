using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileRader : MonoBehaviour {
	private static List<Transform> addOutRangeMissile = new List<Transform> ();
	public static List<Transform> AddOutRangeMissile{
		get{
			return addOutRangeMissile;
		}
	}
	private static List<Transform> addInRangeMissile = new List<Transform>();

	private static List<Transform> outRangeMissiles = new List<Transform> ();
	public static List<Transform> OutRangeMissiles{
		get{
			return outRangeMissiles;
		}
	}
	private static List<Transform> InRangeMissiles = new List<Transform>();

	[SerializeField]
	private Transform Player;
	[SerializeField]
	private GameObject PointOb;

	void Start () {
		StartCoroutine (OutRaderMissileDistance());
		StartCoroutine (RotateRader());
	}

	private IEnumerator RotateRader(){
		while(!GameManager.GameOver){
			transform.rotation = new Quaternion (0,0,Player.localRotation.y,transform.rotation.w);
			yield return null;
		}
	}

	private IEnumerator OutRaderMissileDistance(){
		while (!GameManager.GameOver) {
			yield return StartCoroutine(MissileAddList(true));
			for(int i = 0; i < outRangeMissiles.Count; i++){
				if (Mathf.Abs (Vector3.Distance (outRangeMissiles[i].position, Player.position)) <= 2000) {
					ToInRange (outRangeMissiles [i]);
					yield return null;
				}
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator InRaderMissileDistance(){
		while(!GameManager.GameOver){
			yield return StartCoroutine(MissileAddList(false));
			for(int i = 0; i < InRangeMissiles.Count; i++){
				if (Mathf.Abs (Vector3.Distance (InRangeMissiles[i].position, Player.position)) > 2000) {
					Debug.Log (outRangeMissiles[i]);
					ToOutRange(InRangeMissiles[i]);
					yield return null;
				}
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator MissileAddList(bool isOutRange){
		if (isOutRange) {
			addOutRangeMissile.ForEach (addMissile => outRangeMissiles.Add (addMissile));
			addOutRangeMissile.Clear ();
		} else {
			addInRangeMissile.ForEach (addMissile => InRangeMissiles.Add(addMissile));
			addInRangeMissile.Clear ();
		}
		yield return null;
	}

	private  void ToInRange(Transform Missile){
		addInRangeMissile.Add (Missile);
		NewMissilePointUI (Missile);
		outRangeMissiles.Remove (Missile);
	}

	private  void ToOutRange(Transform Missile){
		addOutRangeMissile.Add (Missile);
		//Debug.Log (GameObject.Find(Missile.name + "Point").gameObject);
		Destroy (GameObject.Find(Missile.name + "Point").gameObject);
		InRangeMissiles.Remove (Missile);
	}
		
	public static void DestroyMissile(Transform Missile){
		try{
		Destroy (GameObject.Find(Missile.name + "Point").gameObject);
		InRangeMissiles.Remove (Missile);
		}catch{
		}
	}

	private  void NewMissilePointUI(Transform Missile){
		GameObject MissileUI = Instantiate (PointOb);
		MissileUI.GetComponent<MapMissilePosition> ().StartCoroutine (MissileUI.GetComponent<MapMissilePosition> ().UpdatePosition (Missile));
	}
}
