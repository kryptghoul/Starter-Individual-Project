using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupController : MonoBehaviour
{
    public Transform Effect;

    void Start()
    {
        Effect.GetComponent<ParticleSystem>().enableEmission = false;
    }

    private void OnTriggerEnter2D(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Effect.GetComponent<ParticleSystem>().enableEmission = true;
            StartCoroutine(stopEffects());
        }
    }

    IEnumerator stopEffects()
    {
        yield return new WaitForSeconds(.4f);

        Effect.GetComponent<ParticleSystem>().enableEmission = false;
    }

}