using System.Collections;
using System.Collections.Generic;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneField gameScene;

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene.Name);
    }
}
