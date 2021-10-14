using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private static CollisionManager _instance;

    public static CollisionManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public List<GameObject> MoneyPiles;
    public List<GameObject> MoneyPilesPool;
    public Transform FirstPilePosition;
    float distance = .275f;
    float smoothTime = .005f;
    private Vector3 velocity = Vector3.zero;
    public Transform cart;

    private void LateUpdate()
    {
        if (MoneyPiles.Count > 0)
        {
            if (Vector3.Distance(MoneyPiles[0].transform.position, cart.position) > distance)
            {
                Vector3 targetPosition = new Vector3(cart.position.x, MoneyPiles[0].transform.position.y, cart.position.z);

                MoneyPiles[0].transform.position = Vector3.SmoothDamp(MoneyPiles[0].transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
        for (int i = 1; i < MoneyPiles.Count; i++)
        {
            if (Vector3.Distance(MoneyPiles[i - 1].transform.position, MoneyPiles[i].transform.position) > distance)
            {
                Vector3 targetPosition = new Vector3(MoneyPiles[i - 1].transform.position.x, MoneyPiles[i].transform.position.y, MoneyPiles[i - 1].transform.position.z);

                MoneyPiles[i].transform.position = Vector3.SmoothDamp(MoneyPiles[i].transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }

    int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyGroup"))
        {
            other.gameObject.SetActive(false);
            MoneyPilesPool[0].transform.position = new Vector3(FirstPilePosition.position.x, FirstPilePosition.position.y + (index * distance), FirstPilePosition.position.z);
            MoneyPilesPool[0].SetActive(true);
            MoneyPiles.Add(MoneyPilesPool[0]);
            MoneyPilesPool.RemoveAt(0);
            index++;
        }
    }

    public void RearrangePiles()
    {

    }
}
