using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapearAfter : MonoBehaviour
{
    public float time;

    private void Start()
    {
        Invoke(nameof(Destroy), time);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
