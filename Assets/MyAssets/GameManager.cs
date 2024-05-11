using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] bosses;

    public Player player;

    public MultiSceneData multiScenedata;

    public Transform bossSpawnPoint;

    private void Awake()
    {
        multiScenedata = GameObject.FindGameObjectWithTag("MultiScene").GetComponent<MultiSceneData>();
    }

    private void Start()
    {
        SpawnBoss();
    }

    public void SpawnBoss()
    {
        IBoss boss = Instantiate(bosses[multiScenedata.bossId], bossSpawnPoint.position, Quaternion.identity).GetComponent<IBoss>();
        boss.difficultyMult = multiScenedata.mult;
        boss.aggroedPlayer = player;
    }
}
