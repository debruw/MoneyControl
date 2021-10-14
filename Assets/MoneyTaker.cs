using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTaker : MonoBehaviour
{
    public int needValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyPile"))
        {
            Debug.Log("111");
            CollisionManager.Instance.MoneyPiles.Remove(other.gameObject);
        }
    }
}
