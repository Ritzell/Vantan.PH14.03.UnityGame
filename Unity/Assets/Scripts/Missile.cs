using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missile : MonoBehaviour {
	[SerializeField]
	private AudioClip AudioClip1;
	[SerializeField]
	private AudioClip AudioClip2;

	private AudioSource AudioS;

	[SerializeField]
	private float Speed = 850f;//時速3000km
	private static Airframe AirFrame;

	private Vector3 StartPos;
	private Quaternion StartRot;

	void Start ()
	{
		AirFrame = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		AudioS = gameObject.GetComponent<AudioSource>();
		AudioS.clip = AudioClip1;
		StartPos = transform.localPosition;
		StartRot = transform.localRotation;
	}

	private IEnumerator ShootReady()
	{
		AirFrame.Reload (StartPos,StartRot); //StartCoroutine (GameObject.Find("GameManager").GetComponent<GameManager>().reloadMissile(startPos,startRot));
		transform.parent = null;
		AudioS.Play();
		StartCoroutine (SelfBreak ());
		yield return null;
	}

	private IEnumerator shootReady_E()
	{
		//audioS.Play();
		StartCoroutine (SelfBreak ());
		yield return null;
	}

	public IEnumerator Straight()
	{
		StartCoroutine (ShootReady ());
		while (!GameManager.GameOver)
		{
            try
            {
				StartCoroutine(MoveForward());
			}
			catch
			{
				break;
			}
			yield return null;
		}
	}

	private IEnumerator GetAiming(Transform tgt,bool player)
	{
		Vector3 TgtPos = new Vector3 (tgt.transform.position.x + Random.Range(-3,3),tgt.transform.position.y + Random.Range(-3,3),tgt.transform.position.z + Random.Range(-3,3));
		transform.LookAt (TgtPos);
		yield return null;
	}

	public IEnumerator Straight(Transform tgt)
	{
		StartCoroutine (shootReady_E ());
		transform.LookAt (tgt);
		while (!GameManager.GameOver)
		{
			try
			{
				StartCoroutine(MoveForward());
			}
			catch{}
			yield return null;
		}
	}

	public IEnumerator Tracking(Transform tgt)
	{
		StartCoroutine (ShootReady ());
		float delay = 0f;
		while (!GameManager.GameOver)
		{
			delay += Time.deltaTime;
			try
			{
				//if(delay >= 0.5f){
				StartCoroutine(GetAiming(tgt,true));
					delay = 0f;
				//}
				StartCoroutine(MoveForward());
			}catch{}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator Tracking_E(Transform tgt)
	{
		StartCoroutine (shootReady_E ());
		transform.LookAt (tgt);
		float delay = 0f;
		while (!GameManager.GameOver)
		{
			delay += Time.deltaTime;
			try
			{
				if(delay >= 0.5f){
				StartCoroutine(GetAiming(tgt,false));
					delay = 0f;
				}
				StartCoroutine(MoveForward());
			}catch{}
			yield return null;
		}
		yield return null;
	}

	private IEnumerator MoveForward()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * Speed);
		yield return null;
	}

	void OnTriggerEnter(Collider col)
	{
		if (transform.parent != null) 
		{
			return;
		}
		StartCoroutine (BreakMissile());
	}

	private IEnumerator SelfBreak()
	{
		float time = 0f;
		while (!GameManager.GameOver && time < 20f) 
		{
			time += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (BreakMissile ());
		yield return null;
	}

	private IEnumerator BreakMissile()
	{
		Instantiate(Resources.Load("prefabs/Explosion"),transform.position,Quaternion.identity);
		Destroy (gameObject);
		yield return null;
	}
}

