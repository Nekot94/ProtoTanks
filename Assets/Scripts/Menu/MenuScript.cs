using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    [SerializeField]
    private GameObject overlay;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        ShowLaunchScreen();
    }

    public void ShowLaunchScreen()
    {
        Time.timeScale = 0f;
        // AudioListener.volume = 0;
        audioSource.Stop();
        overlay.SetActive(true);
    }

    public void StandartGame()
    {
        StartSetup();
        gameManager.StartGame();
    }

    public void OnePlayerGame()
    {
        StartSetup();
        gameManager.m_numberOfLivePlayers = 1;
        gameManager.m_TankPrefabs[1] = gameManager.m_TankPrefabs[2];
        gameManager.StartGame();
    }

    public void PvPGame()
    {
        StartSetup();
        gameManager.m_numberOfPlayers = 2;
        gameManager.StartGame();
    }

    private void StartSetup()
    {
        overlay.SetActive(false);
        // AudioListener.volume = 1f;
        audioSource.Stop();
        audioSource.Play();
        Time.timeScale = 1f;
    }
}
