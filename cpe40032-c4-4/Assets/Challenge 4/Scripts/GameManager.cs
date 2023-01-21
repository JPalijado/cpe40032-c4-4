using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;
    public int enemyScore = 0;
    public int targetScore;
    public bool isGameOver = false;

    private AudioSource playerAudio;
    public AudioClip winSound;
    public AudioClip loseSound;
    
    void Start()
    {
        playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
    }

    void Update()
    {   // Checks if the game is over
        if (playerScore == targetScore || enemyScore == targetScore)
            isGameOver = true;
    }

    // Adds score to the player
    public void AddPlayerScore(int value)
    {
        playerScore += value;
        if (playerScore == targetScore)
        {
            Debug.Log("Victory!");
            playerAudio.PlayOneShot(winSound, 3.0f);
        }
        else
        {
            Debug.Log("Player Score: " + playerScore);
        }
    }

    // Adds score to the enemy
    public void AddEnemyScore(int value)
    {
        enemyScore += value;
        if (enemyScore == targetScore)
        {
            Debug.Log("Defeat");
            playerAudio.PlayOneShot(loseSound, 3.0f);
        }
        else
        {
            Debug.Log("Enemy Score: " + enemyScore);
        }
    }
}
