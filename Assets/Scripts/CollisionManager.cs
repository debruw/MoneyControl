using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TapticPlugin;

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
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        if (MoneyPiles.Count > 0)
        {
            if (Vector3.Distance(MoneyPiles[0].transform.position, cart.position) > .2f)
            {
                Vector3 targetPosition = new Vector3(FirstPilePosition.position.x, FirstPilePosition.position.y, FirstPilePosition.position.z);

                MoneyPiles[0].transform.position = Vector3.SmoothDamp(MoneyPiles[0].transform.position, targetPosition, ref velocity, smoothTime);
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
    }

    int index;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyGroup"))
        {
            if (MoneyPiles.Count == 1)
            {
                pile1.SetActive(true);
            }
            if (MoneyPiles.Count == 2)
            {
                pile2.SetActive(true);
            }
            if (MoneyPiles.Count == 3)
            {
                pile3.SetActive(true);
            }
            if (MoneyPiles.Count > 0)
            {
                other.transform.DOMoveY(MoneyPiles[MoneyPiles.Count - 1].transform.position.y, .2f).OnComplete(() =>
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
        }
    }

    void CollectMoney(GameObject go, Vector3 pos)
    {
        TapticManager.Impact(ImpactFeedback.Light);
        SoundManager.Instance.playSound(SoundManager.GameSounds.TakeMoney);
        go.GetComponent<Collider>().enabled = false;
        go.tag = "Untagged";
        go.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        go.transform.position = new Vector3(pos.x, pos.y + (index * distance), pos.z);
        go.transform.eulerAngles = new Vector3(90, Random.Range(-45, 45), 0);
        MoneyPiles.Add(go);
        index++;
    }

    public void ClosePiles()
    {
        pile1.SetActive(false);
        pile2.SetActive(false);
        pile3.SetActive(false);
    }

    public Transform piggyBankTarget;
   
    public IEnumerator MoveMoneysToPiggy()
    {
        for (int i = MoneyPiles.Count - 1; i >= 0; i--)
        {
            MoneyPiles[i].GetComponent<Collider>().enabled = true;
            MoneyPiles[i].tag = "Money";
            MoneyPiles[i].transform.DOMove(piggyBankTarget.transform.position, 1);
            yield return new WaitForSeconds(.05f);
        }
        pile1.SetActive(false);
        pile2.SetActive(false);
        pile3.SetActive(false);
        MoneyPiles[0].transform.parent.gameObject.SetActive(false);

        yield return new WaitForSeconds(.5f);
        piggyBankTarget.gameObject.GetComponentInParent<Animator>().SetTrigger("Jump");
        StartCoroutine(GameManager.Instance.WaitAndGameWin());
    }
}
