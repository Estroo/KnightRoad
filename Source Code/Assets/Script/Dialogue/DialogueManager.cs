using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Image dialoguePortrait;
    public GameObject DialogueLayer;
    public Queue<string> sentences;

    public Animator anim;

    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Action") && (PlayerControl.inDialog = true))
            DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        anim.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        dialoguePortrait.sprite = dialogue.portrait;
        PlayerControl.inDialog = true;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(sentence));
    }

    void EndDialogue()
    {
        PlayerControl.inDialog = false;
        anim.SetBool("IsOpen", false);
    }

    IEnumerator TypeDialogue(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
}
