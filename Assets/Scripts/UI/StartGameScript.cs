using System.Collections;
using System.Collections.Generic;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartGameScript : MonoBehaviour
{
    [SerializeField] private VideoPlayer splashVideo;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private SceneField gameScene;

    private void Start()
    {
        splashVideo.gameObject.SetActive(true);
        menuUI.SetActive(false);
        StartCoroutine(ShowMenu());
    }

    private IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds((float)splashVideo.clip.length);
        splashVideo.gameObject.SetActive(false);
        menuUI.SetActive(true);
    }

    public void StartGame()
    {

        SceneManager.LoadScene(gameScene.Name);
        Debug.Log("Game Started");
    }
}