using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunReticle : MonoBehaviour {

    public void EnableReticle()
    {
        StartCoroutine(SerchEnemy());
    }
    void Start()
    {
        StartCoroutine(SerchEnemy());

    }
    public IEnumerator SerchEnemy()
    {

        RaycastHit Hit;
        const int LayerMask = 1 << (int)Layers.Enemy | 1 << (int)Layers.EnemyMissile | 1 << (int)Layers.EnemyArmor;

        while (!GameManager.IsGameOver)
        {
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out Hit, 30000, LayerMask))
            {
                StartCoroutine(Gun.MuzzuleLookTgt(ray.GetPoint(4000)));

                GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                StartCoroutine(Gun.MuzzuleLookTgt(ray.GetPoint(4000)));
                GetComponent<Image>().color = Color.green;
            }
            yield return null;
        }
    }
}
