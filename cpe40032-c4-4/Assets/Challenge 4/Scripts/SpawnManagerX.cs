using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float spawnRangeX = 10;
    private float spawnZMin = 15; // set min spawn Z
    private float spawnZMax = 25; // set max spawn Z

    public int enemyCount;
    public int waveCount = 1;
    public float enemySpeed = 25;
    public float increaseSpeed = 25;

    public GameObject player;

    private AudioSource playerAudio;
    public AudioClip levelUpSound;

    private GameManager gameManager;

    void Start()
    {
        playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Problem 2 - A new wave spawns when the player gets a powerup
        // Solution: Set enemyCount correctly, replace "Powerup" to "Enemy"
        // From enemyCount = GameObject.FindGameObjectsWithTag("Powerup").Length; to enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
        // Spawn enemies if there are no enemies left and the game is still not over
        if (enemyCount == 0 && gameManager.isGameOver == false)
        {
            SpawnEnemyWave(waveCount);
            playerAudio.PlayOneShot(levelUpSound, 2.0f);
        }
    }

    // Generate random spawn position for powerups and enemy balls
    Vector3 GenerateSpawnPosition ()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }


    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15); // make powerups spawn at player end

        // If no powerups remain, spawn a powerup
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0) // check that there are zero powerups
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // Spawn number of enemy balls based on wave number
        // Problem 4 - 2 enemies are spawned in every wave
        // Solution: Replace 2 with enienemiesToSpawn as parameter from i < 2 to i < enemiesToSpawn
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        waveCount++;
        // Increments the speed of the enemy by 25 on each wave
        enemySpeed += increaseSpeed;
        ResetPlayerPosition(); // put player back at start
    }

    // Move player back to position in front of own goal
    public void ResetPlayerPosition ()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}