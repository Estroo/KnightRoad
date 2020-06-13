using UnityEngine;
using System.Collections;

public class FallingTrap : MonoBehaviour
{
    GamePadController.Controller gamePad;

    private Vector3 initial_position;
    private Rigidbody2D rigidBody2D;
    private bool isMovingUp = false;
    private bool isGrounded = false;

    public float delayTime;
    public float moveSpeedUp;
    public float moveSpeedDown;

    private GameObject Player;
    Shake camShake;

    public GameObject FallingParticle;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        initial_position = transform.position;
        rigidBody2D = GetComponent<Rigidbody2D>();
        camShake = GameObject.FindGameObjectWithTag("Effect").GetComponent<Shake>();
        gamePad = GamePadController.GamePadOne;
    }

    void Update()
    {
        if (isMovingUp)
        {
            if (transform.position == initial_position)
            {
                isMovingUp = false;
                StartCoroutine(waitForDown(delayTime));
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, initial_position, Time.deltaTime * moveSpeedUp);
        }
        if (isGrounded)
            StartCoroutine(waitForUp(delayTime));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 17)
        {
            isGrounded = true;
            gameObject.GetComponent<AudioSource>().Play();
            float dist = transform.position.x - Player.transform.position.x;
            Destroy(Instantiate(FallingParticle, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity), 2.5f);
            if (dist >= -10f && dist <= 10f)
            {
                gamePad.SetVibration(100, 100, 0.15f);
                camShake.Shakee(0.1f, 0.1f);
            }
        }
    }

    IEnumerator waitForUp(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isGrounded = false;
        rigidBody2D.velocity = new Vector2(0, 0);
        rigidBody2D.gravityScale = 0.0f;
        isMovingUp = true;
    }

    IEnumerator waitForDown(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isGrounded = false;
        rigidBody2D.gravityScale = moveSpeedDown;
    }
}
