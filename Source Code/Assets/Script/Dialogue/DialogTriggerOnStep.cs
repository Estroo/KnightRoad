using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTriggerOnStep : MonoBehaviour
{
    public Dialogue dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
