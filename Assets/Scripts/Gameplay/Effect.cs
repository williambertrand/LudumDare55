using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float duration;
    void Start()
    {
        StartCoroutine(TTL());
    }


    private IEnumerator TTL()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
