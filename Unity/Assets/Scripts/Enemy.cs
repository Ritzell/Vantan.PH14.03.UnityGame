using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	private AudioSource CryBox;

	private Material Emission;
	private bool isLife = true;
	private int HP = 4;

	/// <summary>
	/// フレアによる回避はミサイルのスクリプトで行う
	/// </summary>
	private static GameObject tgt;
	public static GameObject Tgt {
		set {
			tgt = value;
		}
		get {
			return tgt;
		}
	}

	private static ReticleSystem PlayerReticle;
	private static Airframe Frame;

	void Awake(){
//		material.SetColor("_EmissionColor", new Color(1,0,0));
		PlayerReticle = GameObject.Find("ReticleImage").GetComponent<ReticleSystem>();
		Frame = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		Tgt = GameObject.Find ("eurofighter");
	}

	void Start ()
	{
		EnemyBase.Rest = EnemyBase.Rest + 1;
		StartCoroutine (Breathing ());
	}

	void OnTriggerStay (Collider Col)
	{
		if (!isLife) {
			return;
		}
		HP--;
		if (HP <= 0) {
			isLife = false;
			EnemyBase.Rest = EnemyBase.Rest - 1;
			PlayerReticle.DestoroyLockOnTgt (gameObject);
			StartCoroutine (Deth ());
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

	private IEnumerator Deth(){
		CryBox.pitch = Random.Range (0.65f,1.3f);
		CryBox.Play ();
		yield return null;
		while(true){
			if (CryBox.isPlaying == false) {
				Frame.StartCoroutine (NotificationSystem.UpdateNotification(gameObject.name + "を撃破しました！"));
				Destroy (gameObject);
			} else {
//				gameObject.
			}
			yield return null;
		}
	}
}
