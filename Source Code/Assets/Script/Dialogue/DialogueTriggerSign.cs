using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerSign : MonoBehaviour
{
    public Dialogue dialogue;

    private bool isOnSign;

    void Update()
    {
        HandleOnSign();
    }

    void HandleOnSign()
    {
        if (Input.GetButtonDown("Action") && isOnSign == true)
        {
            TriggerDialogue();
            isOnSign = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            isOnSign = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            isOnSign = false;
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        //FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }
}