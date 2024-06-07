using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTerminal : MonoBehaviour
{
    public Canvas interactCanvas;
    public SpriteRenderer screen;
    public SpriteRenderer dangerIcon;

    private void Start()
    {
        interactCanvas.gameObject.SetActive(false);
        screen.color = Color.green;
        dangerIcon.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactCanvas.gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            GameManager.instance.SpawnBoss();
            GetComponent<Collider2D>().enabled = false;
            screen.color = Color.red;
            dangerIcon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactCanvas.gameObject.SetActive(false);
    }
}
