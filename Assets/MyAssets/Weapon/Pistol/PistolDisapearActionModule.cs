using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolDisapearActionModule : ActionModule
{
    private float time = 1f;
    private float timer;
    private float timerTickTime = 0.1f;
    private WaitForSeconds timerTick;

    public override void AfterSet()
    {
        timer = 0;
        timerTick = new WaitForSeconds(timerTickTime);
    }

    public override ModuleData GetData()
    {
        ModuleData data = new ModuleData();
        data.className = className;
        data.level = level;

        return data;
    }

    public override void OnAction(Vector2 direction)
    {     
        if (timer <= 0)
        {
            timer = time;
            StartCoroutine(DisapearTimer());
        }
        else
        {
            timer = time;
        }
    }

    public override void SetData(ModuleData data)
    {
        level = data.level;
    }

    private IEnumerator DisapearTimer()
    {
        while (timer > 0)
        {
            timer -= timerTickTime;
            yield return timerTick;
        }

        action.weaponModel.SetActive(false);
    }
}
