using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    PlayerControl player;
    private int indexController;
    private int previousIndex;
    private bool checkVerticalAxisDown = false;
    private bool checkVerticalAxisUp = false;

    public GameObject pauseMenu;

    public void Start()
    {
        indexController = 0;
        previousIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    public void Update()
    {
        if (player.dead == true)
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                if (Input.GetAxisRaw("Vertical") == -1)
                {
                    if (checkVerticalAxisDown == false)
                    {
                        previousIndex = indexController;
                        indexController++;
                        checkVerticalAxisDown = true;
                        if (indexController > 2)
                            indexController = 0;
                    }
                }
                if (Input.GetAxisRaw("Vertical") == 1)
                {
                    if (checkVerticalAxisUp == false)
                    {
                        previousIndex = indexController;
                        indexController--;
                        checkVerticalAxisUp = true;
                        if (indexController < 0)
                            indexController = 2;
                    }
                }
                if (Input.GetAxisRaw("Vertical") == 0)
                {
                    checkVerticalAxisDown = false;
                    checkVerticalAxisUp = false;
                }
                pauseMenu.transform.GetChild(0).gameObject.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
                pauseMenu.transform.GetChild(0).gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = new Color(140f / 255f, 255f / 255f, 255f / 255f);
                if (Input.GetButtonDown("Jump"))
                {
                    if (indexController == 0)
                        resetLevel();
                    else if (indexController == 1)
                        exitLevel();
                }
            }
        }
    }

    public void resetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void exitLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
