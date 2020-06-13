using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator ChestAnim;
    private new GameObject gameObject;
    void Start()
    {
        gameObject = GetComponent<GameObject>();
        ChestAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (transform.gameObject.tag == "OpenChest")
        {
            ChestAnim.Play("ChestOpening");
        }
    }
}
