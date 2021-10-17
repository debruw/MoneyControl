using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTaker : MonoBehaviour
{
    public int needValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < needValue; i++)
            {
                CollisionManager.Instance.MoneyPiles.RemoveAt(0);
            }
        }
    }
}
