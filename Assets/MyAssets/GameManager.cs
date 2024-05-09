using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MultiSceneData multiScenedata;
    private void Awake()
    {
        multiScenedata = GameObject.FindGameObjectWithTag("MultiScene").GetComponent<MultiSceneData>();
    }
}
