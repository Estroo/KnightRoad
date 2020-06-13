using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageEnemy : MonoBehaviour
{
    GamePadController.Controller gamePad;

    public GameObject BloodParticle;
    Shake camShake;

    void Start()
    {
        camShake = GameObject.FindGameObjectWithTag("Effect").GetComponent<Shake>();
        gamePad = GamePadController.GamePadOne;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") && gameObject.tag != "Dying")
        {
            gamePad.SetVibration(100, 100, 0.15f);
            gameObject.tag = "Dying";
            gameObject.GetComponent<Animator>().Play("Die");
            Destroy(Instantiate(BloodParticle, transform.position, Quaternion.identity), 1.0f);
            camShake.Shakee(0.1f, 0.1f);
            gameObject.GetComponent<AudioSource>().Play();
            Destroy(gameObject, 0.8f);
        }
    }
}