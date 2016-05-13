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
		Destroy (RestChildren <= 0 ? gameObject : null);
	}

	void OnDestroy ()
	{
		GameManager.loadScene ();
	}
}
