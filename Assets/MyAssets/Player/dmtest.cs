using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dmtest : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {              
        if (collision.TryGetComponent<Health>(out Health health))
        {
            health.Damage(5);
        }
    }
}
