using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour
{
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

		Childs.Add (GameObject.Find ("childEnemyA").GetComponent<EnemyAttack> ());
		Childs.Add (GameObject.Find ("childEnemyB").GetComponent<EnemyAttack> ());


	}

	void Start(){
		StartCoroutine (ChangeTarget ());
	}

	void OnTriggerEnter (Collider Col)
	{
        if (RestChildren <= 0)
        {
            StartCoroutine(GameManager.GameEnd(true));
            Destroy(gameObject);
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
