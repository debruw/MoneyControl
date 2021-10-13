using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public List<GameObject> MoneyPiles;
    int currentId = 0;

    private void Update()
    {
        for (int i = 0; i < MoneyPiles.Count; i++)
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyGroup"))
        {
            other.gameObject.SetActive(false);
            MoneyPiles[currentId].SetActive(true);
            currentId++;
        }
    }
}
