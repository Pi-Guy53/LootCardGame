using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    public Transform destination;

    public void startMovement(Transform end)
    {
        destination = end;
    }

    private void Update()
    {
        if(destination != null)
        {
            transform.position = Vector3.Lerp(transform.position, destination.position, .25f);

            if(Vector3.Distance(transform.position, destination.position) < 1)
            {
                Destroy(gameObject);
            }
        }
    }
}