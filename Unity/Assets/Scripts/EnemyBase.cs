using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour
{
	[SerializeField]
	private Material material;
    [SerializeField]
    private GameObject barrier;

	private static List<EnemyAttack> Childs = new List<EnemyAttack> ();
	private static int RestChildren = 0;
	private static float HP = 50;
    private static float MaxHP;
	private static IEnumerator StateNotice;

	public static int Rest {
		set {
			RestChildren = value;
            if(RestChildren <= 0)
            {
                FindObjectOfType<EnemyBase>().DisableBarrier();
            }
		}get {
			return RestChildren;
		}
	}

	void Awake ()
	{
		Childs.Add (GameObject.Find ("ChildEnemyA").GetComponent<EnemyAttack> ());
		Childs.Add (GameObject.Find ("ChildEnemyB").GetComponent<EnemyAttack> ());
	}

    void DisableBarrier()
    {
        barrier.SetActive(false);
    }

	public static IEnumerator PlayerInArea(){
		yield return new WaitForSeconds (3f);
		foreach(EnemyAttack Enemy in GameObject.FindObjectsOfType<EnemyAttack>()){
			Enemy.StartCoroutine (Enemy.Attack ());
		}
		GameObject.FindObjectOfType<NotificationSystem>().StartCoroutine(NotificationSystem.UpdateNotification("敵の攻撃が開始しました"));
		yield return null;
	}

    public IEnumerator Death(Enemy EnemyScript)
    {
        StartCoroutine(GameManager.FinishGame(true));
        Airframe Frame = Airframe.AirFrame.GetComponent<Airframe>();
        EnemyScript.CryBox.pitch = Random.Range(0.65f, 1.3f);
        EnemyScript.CryBox.Play();
        StopCoroutine(EnemyScript.Breth);
        EnemyScript.MyMaterial.EnableKeyword("_EMISSION");
        EnemyScript.MyMaterial.SetColor("_EmissionColor", Color.red);
        StartCoroutine(EnemyScript.StateChange(EnemyScript.CryBox));
        StartCoroutine(EnemyScript.ShakeBody());
        EnemyScript.CameraS.StartCoroutine(EnemyScript.CameraS.Flash(0.8f, true, 0.35f, gameObject));

        while (true)
        {
            if (EnemyScript.CryBox.isPlaying == false)
            {
                Frame.StartCoroutine(NotificationSystem.UpdateNotification(gameObject.name + "を撃破しました！"));
                GameObject.Find("engine").GetComponent<AudioSource>().Stop();
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}
