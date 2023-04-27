using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    [SerializeField]
    private GameObject smoke = null;

    private void Start()
    {
        smoke.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
        {
            smoke.SetActive(true);
            Destroy(gameObject, 3f);
        } 
    }
}
