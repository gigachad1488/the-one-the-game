using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveAttack : MonoBehaviour
{
    public CubeBoss cubeBoss;

    public float side = 1f;
    public float speed = 10f;

    private void Start()
    {
        if (side > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        GetComponent<Rigidbody2D>().velocityX = speed * side * cubeBoss.mult;
        Invoke(nameof(Destroy), 2f);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
