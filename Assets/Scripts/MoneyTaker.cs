using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MoneyTaker : MonoBehaviour
{
    public int needValue;
    public Animator UpperBoneAnimator;
    float count;
    float random;
    SkinnedMeshRenderer[] materials;
    public Image image, image2;

    private void Start()
    {
        random = Random.Range(1f, 3f);
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        count += Time.deltaTime;
        if (count > random)
        {
            count = 0;
            random = Random.Range(1f, 3f);
            UpperBoneAnimator.SetTrigger("Jump");
        }
        if (Vector3.Distance(transform.position, Camera.main.gameObject.transform.position) < 15)
        {
            foreach (var item in materials)
            {
                for (int i = 0; i < item.materials.Length; i++)
                {
                    item.materials[i].SetColor("_Color", new Color(item.materials[i].GetColor("_Color").r, item.materials[i].GetColor("_Color").g, item.materials[i].GetColor("_Color").b, (transform.position.z - Camera.main.gameObject.transform.position.z - 5) / 20));
                }
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, (transform.position.z - Camera.main.gameObject.transform.position.z - 5) / 15);
            image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, (transform.position.z - Camera.main.gameObject.transform.position.z - 5) / 15);
        }
    }

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
