using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public Transform FirstPilePosition;
    float distance = .25f;
    float smoothTime = .008f;
    private Vector3 velocity = Vector3.zero;
    public Transform cart;
    public GameObject pile1, pile2, pile3;

    private void LateUpdate()
    {
        if (MoneyPiles.Count > 0)
        {
            if (Vector3.Distance(MoneyPiles[0].transform.position, cart.position) > .2f)
            {
                Vector3 targetPosition = new Vector3(cart.position.x, FirstPilePosition.position.y, cart.position.z);

                MoneyPiles[0].transform.position = Vector3.SmoothDamp(MoneyPiles[0].transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
        for (int i = 1; i < MoneyPiles.Count; i++)
        {
            if (Vector3.Distance(MoneyPiles[i - 1].transform.position, MoneyPiles[i].transform.position) > .2f)
            {
                Vector3 targetPosition = new Vector3(MoneyPiles[i - 1].transform.position.x, MoneyPiles[i - 1].transform.position.y + distance, MoneyPiles[i - 1].transform.position.z);
                
                MoneyPiles[i].transform.position = Vector3.SmoothDamp(MoneyPiles[i].transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
    int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyGroup"))
        {
            if (MoneyPiles.Count > 0)
            {
                other.transform.DOMoveY(FirstPilePosition.position.y + distance, .2f).OnComplete(() =>
                {
                    CollectMoney(other.gameObject, FirstPilePosition.position);
                });
            }
            else
            {
                other.transform.DOMoveY(FirstPilePosition.position.y + distance, .2f).OnComplete(() =>
                {
                    CollectMoney(other.gameObject, FirstPilePosition.position);
                });
            }
            if (MoneyPiles.Count == 1)
            {
                pile1.SetActive(true);
            }
            else if (MoneyPiles.Count == 2)
            {
                pile2.SetActive(true);
            }
            else if (MoneyPiles.Count == 3)
            {
                pile3.SetActive(true);
            }
        }
    }

    void CollectMoney(GameObject go, Vector3 pos)
    {
        go.GetComponent<Collider>().enabled = false;
        go.tag = "Untagged";
        go.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        go.transform.position = new Vector3(pos.x, pos.y + (index * distance), pos.z);
        go.transform.eulerAngles = new Vector3(90, Random.Range(-45, 45), 0);
        MoneyPiles.Add(go);
        index++;
    }
}
