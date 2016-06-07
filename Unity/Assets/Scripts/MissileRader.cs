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
	public static List<Transform> OutRangeMissiles {
		get {
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
		StartCoroutine (InRaderMissileDistance ());
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
		Vector2 RaderPos = new Vector2 (transform.position.x, transform.position.y);
		while(!GameManager.GameOver){
			yield return StartCoroutine(MissileAddList(false));
			for(int i = 0; i < InRangeMissiles.Count; i++){
				float distance;
				try{
					distance = Mathf.Abs (Vector2.Distance (new Vector2 (InRangeMissiles [i].position.x, InRangeMissiles [i].position.y), RaderPos));
				}catch{
					InRangeMissiles.Remove (InRangeMissiles[i]);
					continue;
				}
				if (distance > RaderPos.x) {
					ToOutRange (InRangeMissiles [i]);
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
		addInRangeMissile.Add (NewMissilePointUI (Missile).transform);
		outRangeMissiles.Remove (Missile);
	}

	private  void ToOutRange(Transform MissilePoint){
		addOutRangeMissile.Add (MissilePoint.GetComponent<MapMissilePosition>().Tgt);
		Destroy (GameObject.Find(MissilePoint.name));
		InRangeMissiles.Remove (MissilePoint);
	}
		
	public static void DestroyMissile(Transform Missile){
		GameObject MissilePoint = GameObject.Find (Missile.name + "Point");
		if(OutRangeMissiles.Contains(Missile)){
			OutRangeMissiles.Remove (Missile);
		}else{
			try{
				InRangeMissiles.Remove (MissilePoint.transform);
			}catch{
				Debug.Log ("error");
			}
		}
		Destroy (MissilePoint);
	}

	private  GameObject NewMissilePointUI(Transform Missile){
		GameObject MissileUI = Instantiate (PointOb);
		MissileUI.GetComponent<MapMissilePosition> ().StartCoroutine (MissileUI.GetComponent<MapMissilePosition> ().UpdatePosition (Missile));
		return MissileUI;
	}
}
