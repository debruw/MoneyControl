using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTakerGroup : MonoBehaviour
{
    public GameObject[] moneyTakers;

    public void DeactivateTakers()
    {
        foreach (var item in moneyTakers)
        {
            item.GetComponent<Collider>().enabled = false;
        }
    }
}
