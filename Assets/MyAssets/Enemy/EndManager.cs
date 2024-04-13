using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    [SerializeField]
    private Button menuButton;

    private void Start()
    {
        menuButton.onClick.AddListener(ToMenu);
        Time.timeScale = 0;
    }

    private async void ToMenu()
    {
        await SceneManager.LoadSceneAsync("MainMenu");
    }
}
