using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDecal : MonoBehaviour
{
    private SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void Initialize(Sprite randomSprite)
    {
        sr.sprite = randomSprite;
        Vector3 randomRotation = new Vector3(transform.rotation.x, transform.rotation.y, UnityEngine.Random.Range(0, 360));
        transform.rotation = Quaternion.Euler(randomRotation);
        transform.localScale *= UnityEngine.Random.Range(0.8f, 1.2f);
    }
}
