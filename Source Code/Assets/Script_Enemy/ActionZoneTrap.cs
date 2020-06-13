using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZoneTrap : MonoBehaviour
{
    public GameObject trap;

    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        { 
            trap.GetComponent<PointFallingTrap>().setEnterZone(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trap.GetComponent<PointFallingTrap>().setEnterZone(false);
        }
    }

}
