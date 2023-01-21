using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    private float boost = 20; // boost speed
    public ParticleSystem boostParticle;
    public float interval = 1.5f; // interval on each press of space bar
    private float timer;

    // Variables for sound effects
    public AudioClip powerupSound;
    public AudioClip unpowerupSound;
    public AudioClip boostSound;
    private AudioSource playerAudio;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        // Bonus 1 - The player needs a turbo boost
        // Solution: Add forward impulse to player by pressing spacebar, however the player can only boost every 1.5 seconds 
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > timer)
        {
            // Counts the interval on each press
            timer = Time.time + interval;
            // Adds the impulse force
            playerRb.AddForce(focalPoint.transform.forward * boost, ForceMode.Impulse);
            // Plays the particle effect
            boostParticle.Play();
            playerAudio.PlayOneShot(boostSound, 1.0f);
        }
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            playerAudio.PlayOneShot(powerupSound, 3.0f);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            // Problem 3 - The powerup never goes away
            // Solution: Call the PowerupCooldown Routine with the StartCoroutine() method
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
        playerAudio.PlayOneShot(unpowerupSound, 8.0f);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            // Problem 1 - Hitting an enemy sends it back towards you
            // Solution: Subtract the enemy positon minus the player's position
            // From transform.position - other.gameObject.transform.position to other.gameObject.transform.position - transform.position
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}
