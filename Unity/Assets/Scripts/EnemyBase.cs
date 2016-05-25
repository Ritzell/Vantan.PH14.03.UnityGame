using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
	private static int RestChildren = 0;

	public static int Rest {
		set {
			RestChildren = value;
		}get {
			return RestChildren;
		}
	}

	void OnTriggerEnter (Collider Col)
	{
        if (RestChildren <= 0)
        {
            StartCoroutine(GameManager.GameEnd(true));
            Destroy(gameObject);
        }
	}
}
