using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public Text countdownText;
    public Text countText;
    public Text winText;
    public Text loseText;
    public Text introText;

    public Transform particle;

    public AudioSource startmusic;
    public AudioSource backgroundmusic;
    public AudioSource winmusic;
    public AudioSource losemusic;
    public AudioSource pickupmusic;

    private Rigidbody2D rb2d;
    private int count;
    private bool facingRight;

    public int secondsLeft = 10;
    public bool takingAway = false;

    void Start()
    {
        particle.GetComponent<ParticleSystem>().enableEmission = false;

        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        winText.text = "";
        loseText.text = "";
        SetCountText();
        introText.text = "Use  W,A,S,D  to  collect  4 trash in 10 seconds!";

        startmusic.Play();
        Invoke("playAudio", 2.0f);
        Invoke("timer", 2.0f);

        countdownText.enabled = (false);


    }

    void playAudio()
    {
        startmusic.Stop();
        backgroundmusic.Play();
    }

    void timer()
    {
        countdownText.enabled = (true);
        introText.enabled = (false);
        countdownText.text = "00:" + secondsLeft;
    }


    void Update()
    {

        //animator.SetFloat("speed", Mathf.Abs(speed));


       if (Input.GetKeyDown(KeyCode.Escape))
       {
                Application.Quit();
       }

        if (takingAway == false && secondsLeft > 0)
        {
            StartCoroutine(TimerTake());
        }

        if (takingAway == false && secondsLeft <= 0)
        {
            StartCoroutine(TimerTake());
            gameObject.active = false;
            loseText.text = "You lose!";
            if (backgroundmusic.isPlaying)
            {
                backgroundmusic.Stop();
                losemusic.Play();
            }
        }

        if (takingAway == true && secondsLeft <= 9)
        {
            introText.enabled = false;
        }

    }

    void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Trash"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            pickupmusic.Play();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            gameObject.active = false;
            takingAway = true;
            loseText.text = "You lose!";
            if (backgroundmusic.isPlaying)
            {
                backgroundmusic.Stop();
                losemusic.Play();
            }
        }

        particle.GetComponent<ParticleSystem>().enableEmission = true;
        StartCoroutine(stopParticles ());

        IEnumerator stopParticles()
        {
            yield return new WaitForSeconds(.4f);
            particle.GetComponent<ParticleSystem>().enableEmission = false;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 4)
        {
            winText.text = "You win!";
            if (backgroundmusic.isPlaying)
            {
                backgroundmusic.Stop();
                winmusic.Play();
            }
            takingAway = true;
            gameObject.active = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb2d.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        if (secondsLeft < 10)
        {
        countdownText.text = "00:0" + secondsLeft;
        }
        else
        { 
        countdownText.text = "00:" + secondsLeft;
        }
        takingAway = false;

    }
}