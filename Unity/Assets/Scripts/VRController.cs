using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VRController : MonoBehaviour {
    private GameObject TouchObject;
    private Coroutine PressTriggerCoroutine;
    // Use this for initialization

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject);
        TouchObject = col.gameObject;
        TouchObject.GetComponentInChildren<Text>().color = Color.red;
        PressTriggerCoroutine = StartCoroutine(PressTrigger());
    }

    void OnTriggerExit(Collider col)
    {
        TouchObject.GetComponentInChildren<Text>().color = Color.black;
        TouchObject = null;
        StopCoroutine(PressTriggerCoroutine);
    }

    IEnumerator PressTrigger()
    {
        while (true)
        {
            if (TouchObject && InputVRController.GetPress(InputVRController.InputPress.PressTrigger,true))
            {
                TouchObject.GetComponent<TitleButton>().OnClick();
            }
            yield return null;
        }
    }

}
