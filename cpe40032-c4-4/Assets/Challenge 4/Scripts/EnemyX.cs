using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    private float speed;
    private Rigidbody enemyRb;
    private GameObject playerGoal;
    private SpawnManagerX spawnManagerScript;
    private GameObject player;
    private GameManager gameManager;

    private AudioSource playerAudio;
    public AudioClip hitSound;
    public AudioClip enemyScoreSound;
    public AudioClip playerScoreSound;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerAudio = player.GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // Problem 5 - The enemy balls are not moving anywhere
        // Solution: Assign playerGoal object
        playerGoal = GameObject.Find("Player Goal");
        // Bonus 2 - The enemies never get more difficult
        // Solution: Increment the speed each wave using the Spawn Manager script, then set the speed to the incremented speed from SpawnManagerX script
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerX>();
        speed = spawnManagerScript.enemySpeed;
    }

    void Update()
    {
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            playerAudio.PlayOneShot(playerScoreSound, 4.0f);
            Destroy(gameObject);
            // Adds score to the player
            gameManager.AddPlayerScore(1);
        } 
        else if (other.gameObject.name == "Player Goal")
        {
            playerAudio.PlayOneShot(enemyScoreSound, 4.0f);
            Destroy(gameObject);
            // Adds score to the enemy
            gameManager.AddEnemyScore(1);
        }

        // Plays a sound effect if the enemies collided with other gameobjects
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall"))
        {
            playerAudio.PlayOneShot(hitSound, 1.0f);
        }
    }
}
