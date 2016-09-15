using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VRController : MonoBehaviour {
    private GameObject TouchObject;
    private Coroutine PressTriggerCoroutine;
    // Use this for initialization
    void Start() {

    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject);
        TouchObject = col.gameObject;
        TouchObject.GetComponent<Image>().color = Color.red;
        PressTriggerCoroutine = StartCoroutine(PressTrigger());
    }

    void OnTriggerExit(Collider col)
    {
        TouchObject.GetComponent<Image>().color = Color.white;
        TouchObject = null;
        StopCoroutine(PressTriggerCoroutine);
    }

    IEnumerator PressTrigger()
    {
        while (true)
        {
            SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
            var device = SteamVR_Controller.Input((int)trackedObject.index);

            if (TouchObject && device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                TouchObject.GetComponent<TitleButton>().OnClick();
            }
            yield return null;
        }
    }
}
