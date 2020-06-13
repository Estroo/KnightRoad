using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Flip(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Flip(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }
}
