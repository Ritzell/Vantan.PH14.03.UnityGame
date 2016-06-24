using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour
{
	[SerializeField]
	private Material material;

	private const int TowerA = 0;
	private const int TowerB = 1;
	private const int TowerC = 2;
	private const int TowerD = 3;
	private const int TowerE = 4;
	private const int TowerF = 5;
	private static List<Transform> Towers = new List<Transform> ();
	private static List<EnemyAttack> Childs = new List<EnemyAttack> ();
	private static int RestChildren = 0;
	private static int HP = 600;
	private static IEnumerator StateNotice;

	public static int Rest {
		set {
			RestChildren = value;
		}get {
			return RestChildren;
		}
	}

	void Awake ()
	{
		Towers.Add (GameObject.Find ("TowerA").transform);
		Towers.Add (GameObject.Find ("TowerB").transform);
		Towers.Add (GameObject.Find ("TowerC").transform);
		Towers.Add (GameObject.Find ("TowerD").transform);
		Towers.Add (GameObject.Find ("TowerE").transform);
		Towers.Add (GameObject.Find ("TowerF").transform);

		Childs.Add (GameObject.Find ("ChildEnemyA").GetComponent<EnemyAttack> ());
		Childs.Add (GameObject.Find ("ChildEnemyB").GetComponent<EnemyAttack> ());


	}

	void Start ()
	{
//		StartCoroutine (ChangeTarget ());
		//material.color = Color.gray;
		StartCoroutine (Respiration ());
		StateNotice = FindObjectOfType<EnemyBase> ().StateNotification ();
		material.color = new Color (0.3f, 0.3f, 0.3f, 1);
	}


	public static IEnumerator PlayerInArea(){
		yield return new WaitForSeconds (5f);
		foreach(EnemyAttack Enemy in GameObject.FindObjectsOfType<EnemyAttack>()){
			Enemy.StartCoroutine (Enemy.Attack ());
		}
//		NotificationSystem.Announce = "敵の攻撃が開始しました";
		GameObject.FindObjectOfType<NotificationSystem>().StartCoroutine(NotificationSystem.UpdateNotification("敵の攻撃が開始しました"));
		yield return null;
	}

	void OnTriggerEnter (Collider Col)
	{
		if (Col.gameObject.layer == (int)PlayerAttackPower.bulletLayer) {
			HP -= (int)PlayerAttackPower.bullet;
		} else if (Col.gameObject.layer == (int)PlayerAttackPower.missileLayer) {
			HP -= (int)PlayerAttackPower.missile;
		}
		StateNotice.MoveNext ();
		if (RestChildren <= 0 && HP <= 0) {
			GameObject.Find ("engine").GetComponent<AudioSource> ().Stop ();
			StopAllCoroutines ();
			StartCoroutine (GameManager.GameEnd (true));
			//Destroy(gameObject);
		}
	}

	private IEnumerator StateNotification(){
		bool isPassing = false;
		while (!isPassing) {
			if (HP <= 300) {
//				NotificationSystem.Announce = gameObject.name + "の体力が著しく消耗しています。";
				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "の体力が著しく消耗しています。"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
		isPassing = false;
		while (!isPassing) {
			if (HP <= 150) {
//				NotificationSystem.Announce = gameObject.name + "が非常に弱っています";

				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "が非常に弱っています"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
		isPassing = false;
		while (!isPassing) {
			if (HP <= 75) {
//				NotificationSystem.Announce = gameObject.name +"敵がもう少しで撃破できます。";

				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "敵がもう少しで撃破できます。"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator Respiration ()
	{
		Material material;
		material = gameObject.GetComponent<Renderer> ().material;
		material.EnableKeyword ("_EMISSION");
		Color MaterialMaxColor = material.GetColor ("_EmissionColor");
		float Turning = (MaterialMaxColor.r + MaterialMaxColor.g + MaterialMaxColor.b) / 3;
		while (true) {
			
			while (0.05f < (material.GetColor ("_EmissionColor").r + material.GetColor ("_EmissionColor").g + material.GetColor ("_EmissionColor").b) / 3) {
				Brethes (material, MaterialMaxColor);
				yield return null;
			}
			while (Turning > (material.GetColor ("_EmissionColor").r + material.GetColor ("_EmissionColor").g + material.GetColor ("_EmissionColor").b) / 3) {
				Suck (material, MaterialMaxColor);
				yield return null;
			}
		}
	}

	private void Brethes(Material material,Color MaxColor){
		Color mColor = material.GetColor ("_EmissionColor");
		material.SetColor ("_EmissionColor", new Color (mColor.r - (MaxColor.r * (Time.deltaTime))
			, mColor.g - (MaxColor.g * (Time.deltaTime))
			, mColor.b - (MaxColor.b * (Time.deltaTime))));
	}

	private void Suck(Material material,Color MaxColor){
		Color mColor = material.GetColor ("_EmissionColor");
		material.SetColor ("_EmissionColor", new Color (mColor.r + (MaxColor.r * (Time.deltaTime))
			, mColor.g + (MaxColor.g * (Time.deltaTime))
			, mColor.b + (MaxColor.b * (Time.deltaTime))));
	}

	public static IEnumerator ChangeTarget ()
	{
		while (true) {
			yield return new WaitForSeconds (3000);
			Childs [0].Target = Towers [TowerA];
			Childs [1].Target = Towers [TowerA];
			Debug.Log ("攻撃対象が変更されました");
		}

	}

}
