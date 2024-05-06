using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitHealCdTimer : MonoBehaviour
{
    private bool canHeal = true;
    public float time = 2;

    public bool CanHeal()
    {
        if (canHeal)
        {
            canHeal = false;
            StartCoroutine(Timer());
            return true;
        }

        return false;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        canHeal = true;
    }
}
