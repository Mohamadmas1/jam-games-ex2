using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    [Tooltip("The spawn points for the players during the game")]
    [SerializeField] private List<Transform> playerGameSpawnPoints;
    [SerializeField] private List<GameObject> startScreenWaitingText;
    [SerializeField] private List<GameObject> startScreenAvatars;
    [SerializeField] private List<RuntimeAnimatorController> playerAnimators;
    [SerializeField] private List<AudioSource> playerAudioSources;
    [SerializeField] private List<AudioClip> shlomoThrowSounds;
    [SerializeField] private List<AudioClip> tzipiThrowSounds;
    [SerializeField] private List<AudioClip> shlomoHitSounds;
    [SerializeField] private List<AudioClip> tzipiHitSounds;
    [SerializeField] private AudioSource managerAudioSource;
    [SerializeField] private AudioClip gameBgMusic;
    [SerializeField] private AudioClip EndGameSound;
    [SerializeField] private List<PlayerLifeUI> playerLives;
    [SerializeField] private List<Transform> playerDiaperUIMasks;
    [SerializeField] private GameObject shlomoWinScreen;
    [SerializeField] private GameObject tzipiWinScreen;

    private List<PlayerController> playerControllers;

    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject endUI;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerControllers = new List<PlayerController>(2);

        managerAudioSource.PlayOneShot(gameBgMusic);

        startUI.SetActive(true);
        gameUI.SetActive(false);
        endUI.SetActive(false);
    }

    public void SpawnPlayer(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        playerInput.transform.position = playerGameSpawnPoints[playerIndex].position;
        playerControllers.Add(playerInput.GetComponent<PlayerController>());
        playerControllers[playerIndex].inputEnabled = false;
        playerControllers[playerIndex].spriteAnimator.runtimeAnimatorController = playerAnimators[playerIndex];
        playerControllers[playerIndex].audioSource = playerAudioSources[playerIndex];
        playerControllers[playerIndex].throwSounds = playerIndex == 0 ? shlomoThrowSounds : tzipiThrowSounds;
        playerControllers[playerIndex].hitSounds = playerIndex == 0 ? shlomoHitSounds : tzipiHitSounds;
        startScreenWaitingText[playerIndex].SetActive(false);
        startScreenAvatars[playerIndex].SetActive(true);
        playerLives[playerIndex].health = playerInput.GetComponent<Health>();
        playerInput.GetComponent<PickupItem>().diaperUIMask = playerDiaperUIMasks[playerIndex];

        if (playerControllers.Count == 2)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        playerControllers[0].inputEnabled = true;
        playerControllers[1].inputEnabled = true;

        startUI.SetActive(false);
        gameUI.SetActive(true);
    }

    public void EndGame(int looserIndex)
    {
        managerAudioSource.Stop();
        managerAudioSource.PlayOneShot(EndGameSound);

        if (looserIndex == 0)
        {
            tzipiWinScreen.SetActive(true);
        }
        else if (looserIndex == 1)
        {
            shlomoWinScreen.SetActive(true);
        }

        playerControllers[0].inputEnabled = false;
        playerControllers[1].inputEnabled = false;

        endUI.SetActive(true);
        restartButton.Select();
        gameUI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
