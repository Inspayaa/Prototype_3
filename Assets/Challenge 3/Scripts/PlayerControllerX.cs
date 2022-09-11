using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    public bool isLowEnough;
    public bool isOnGround;

    private float floatForce = 350.0f;
    private float impulseForce = 10.0f;
    private float gravityModifier = 1.5f;
    private float topBound = 14.0f;

    private Rigidbody playerRb;
    private AudioSource playerAudio;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();


        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        //call method before player input so that input wont overide player's topbound 
        SetTopBoundary();

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
            isOnGround = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
                playerAudio.PlayOneShot(bounceSound, 1.0f);
                playerRb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);
                isOnGround = true;
                Debug.Log("Ground_Contact");
        }

    }

    //prevent the player from going off screen through the top
    void SetTopBoundary()
    {
        if (transform.position.y > topBound)
        {
            isLowEnough = false;
            transform.position = new Vector3(transform.position.x, topBound, transform.position.z);
        }
        else
        {
            isLowEnough = true;
        }
    }
}
