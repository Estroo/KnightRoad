using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSword : MonoBehaviour
{
    public GameObject PlayerSword;
    private BoxCollider2D PlayerSwordCollider;
    void Start()
    {
        PlayerSwordCollider = PlayerSword.GetComponent<BoxCollider2D>();
    }

    public void disableSwordCollider()
    {
        PlayerSwordCollider.enabled = false;
    }

    public void enableSwordCollider()
    {
        PlayerSwordCollider.enabled = true;
    }
}
