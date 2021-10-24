using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PiggyBank : MonoBehaviour
{
    int moneyCount;
    public Camera cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            other.gameObject.transform.DOKill();
            other.gameObject.SetActive(false);
            moneyCount = CollisionManager.Instance.MoneyPiles.Count;
            transform.localScale += new Vector3(1.2f / moneyCount, 1.2f / moneyCount, 1.2f / moneyCount);
            cam.fieldOfView += 10f / moneyCount;
        }
    }
}
