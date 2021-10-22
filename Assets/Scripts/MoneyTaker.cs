using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MoneyTaker : MonoBehaviour
{
    public int needValue;
    public Animator UpperBoneAnimator, ToungeAnimator;
    float count;
    float random;
    SkinnedMeshRenderer[] materials;
    public Image image, image2;
    bool isCounting = true;

    private void Start()
    {
        random = Random.Range(1f, 3f);
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        count += Time.deltaTime;
        if (count > random && isCounting)
        {
            count = 0;
            random = Random.Range(1f, 3f);
            UpperBoneAnimator.SetTrigger("Jump");
            ToungeAnimator.SetTrigger("MoveTounge");
        }
        if (Vector3.Distance(transform.position, Camera.main.gameObject.transform.position) < 18)
        {
            foreach (var item in materials)
            {
                for (int i = 0; i < item.materials.Length; i++)
                {
                    item.materials[i].SetColor("_Color", new Color(item.materials[i].GetColor("_Color").r, item.materials[i].GetColor("_Color").g, item.materials[i].GetColor("_Color").b, (transform.position.z - Camera.main.gameObject.transform.position.z - 7) / 18));
                }
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, (transform.position.z - Camera.main.gameObject.transform.position.z - 7) / 18);
            image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, (transform.position.z - Camera.main.gameObject.transform.position.z - 7) / 18);
        }
    }


    List<GameObject> collectedMoneys = new List<GameObject>();
    public Transform firstmoneyPos;
    float distance = .25f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCounting = false;
            GetComponentInChildren<Animator>().SetTrigger("NomNom");
            GetComponentInParent<MoneyTakerGroup>().DeactivateTakers();
            for (int i = 0; i < needValue; i++)
            {
                collectedMoneys.Add(CollisionManager.Instance.MoneyPiles[0]);
                CollisionManager.Instance.MoneyPiles.RemoveAt(0);
                collectedMoneys[i].transform.parent = firstmoneyPos;
                collectedMoneys[i].transform.localPosition = new Vector3(0, i * distance, 0);
            }
            firstmoneyPos.DOMoveZ(firstmoneyPos.position.z + 1, .5f).OnComplete(() =>
            {
                firstmoneyPos.gameObject.SetActive(false);
            });
        }
    }
}
