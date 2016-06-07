using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour
{
	private Material material;

	private const int TowerA = 0;
	private const int TowerB = 1;
	private const int TowerC = 2;
	private const int TowerD = 3;
	private const int TowerE = 4;
	private const int TowerF = 5;
	private static List<Transform> Towers = new List<Transform> ();
	private static List<EnemyAttack> Childs = new List<EnemyAttack>();
	private static int RestChildren = 0;

	public static int Rest {
		set {
			RestChildren = value;
		}get {
			return RestChildren;
		}
	}

	void Awake(){
		Towers.Add (GameObject.Find("TowerA").transform);
		Towers.Add (GameObject.Find("TowerB").transform);
		Towers.Add (GameObject.Find("TowerC").transform);
		Towers.Add (GameObject.Find("TowerD").transform);
		Towers.Add (GameObject.Find("TowerE").transform);
		Towers.Add (GameObject.Find("TowerF").transform);

		Childs.Add (GameObject.Find ("ChildEnemyA").GetComponent<EnemyAttack> ());
		Childs.Add (GameObject.Find ("ChildEnemyB").GetComponent<EnemyAttack> ());


	}

	void Start(){
//		StartCoroutine (ChangeTarget ());
		StartCoroutine (Breathing());
	}

	void OnTriggerEnter (Collider Col)
	{
        if (RestChildren <= 0)
        {
			GameObject.Find ("engine").GetComponent<AudioSource> ().Stop ();
			StopAllCoroutines ();
            StartCoroutine(GameManager.GameEnd(true));
            //Destroy(gameObject);
        }
	}

	private IEnumerator Breathing(){
		Material material;
		material = gameObject.GetComponent<Renderer> ().material;
		material.EnableKeyword("_EMISSION");
		Color MaterialMaxColor = material.GetColor("_EmissionColor");
		float Turning = (MaterialMaxColor.r + MaterialMaxColor.g + MaterialMaxColor.b)/3;
		while(true){
			
			while (0.05f < (material.GetColor("_EmissionColor").r + material.GetColor("_EmissionColor").g + material.GetColor("_EmissionColor").b) / 3) {
				Color mColor = material.GetColor ("_EmissionColor");
				material.SetColor("_EmissionColor", new Color (mColor.r - (MaterialMaxColor.r * (Time.deltaTime))
					, mColor.g - (MaterialMaxColor.g * (Time.deltaTime))
					, mColor.b - (MaterialMaxColor.b * (Time.deltaTime))));
				yield return null;
			}
			while (Turning > (material.GetColor("_EmissionColor").r + material.GetColor("_EmissionColor").g + material.GetColor("_EmissionColor").b) / 3) {
				Color mColor = material.GetColor ("_EmissionColor");
				material.SetColor("_EmissionColor", new Color (mColor.r + (MaterialMaxColor.r * (Time.deltaTime))
					, mColor.g + (MaterialMaxColor.g * (Time.deltaTime))
					, mColor.b + (MaterialMaxColor.b * (Time.deltaTime))));
				yield return null;
			}
		}
	}

	public static IEnumerator ChangeTarget(){
		while (true) {
			yield return new WaitForSeconds (3000);
			Childs [0].Target = Towers [TowerA];
			Childs [1].Target = Towers [TowerA];
			Debug.Log ("攻撃対象が変更されました");
		}

	}

}
