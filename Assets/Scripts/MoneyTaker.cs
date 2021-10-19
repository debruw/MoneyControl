using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyTaker : MonoBehaviour
{
    public int needValue;
    public Animator UpperBoneAnimator;
    float count;
    float random;
    MeshRenderer[] materials;

    private void Start()
    {
        random = Random.Range(1f, 3f);
        materials = GetComponentsInChildren<MeshRenderer>();
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
        if (transform.position.z - Camera.main.gameObject.transform.position.z < 8)
        {
            foreach (var item in materials)
            {
                for (int i = 0; i < item.materials.Length; i++)
                {
                    item.materials[i].color = new Color(item.materials[i].color.r, item.materials[i].color.g, item.materials[i].color.b, (transform.position.z - Camera.main.gameObject.transform.position.z) / 8);
                }
            }
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
