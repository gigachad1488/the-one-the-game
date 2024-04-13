using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDelay : MonoBehaviour
{
    public float delay = 0f;

    public Canvas endCanvas;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);

        Instantiate(endCanvas);
    }
}
