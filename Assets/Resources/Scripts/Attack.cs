using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {
	public static Queue<GameObject> missiles = new Queue<GameObject> ();
	//public GameObject[] missiles = new GameObject[4];
	
	// Use this for initialization
	void Start () {
		missiles.Enqueue (GameObject.Find ("missileA"));
		missiles.Enqueue (GameObject.Find ("missileB"));
		missiles.Enqueue (GameObject.Find ("missileC"));
		missiles.Enqueue (GameObject.Find ("missileD"));
        StartCoroutine(shoot());
	}

    public IEnumerator shoot()
    {
       float reloading = 0.5f;
        while (!GameManager.GameOver)
        {
            reloading += Time.deltaTime;
            if(reloading >= 0.3f)
            {
                if ((Input.GetAxis("RTrigger") == 1 || Input.GetKeyDown(KeyCode.C)) && missiles.Count >= 1)
                {
                    Debug.Log(Attack.missiles.Count);
                    StartCoroutine(missiles.Dequeue().GetComponent<missile>().straight());
                    reloading = 0f;
                }
            }
            yield return null;
        }
        Debug.Log("end");
        yield return null;
    }
}
