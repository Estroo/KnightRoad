using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string GameScene = "MainLevel";

    public GameObject mainMenu;
    public GameObject optionMenu;
    public GameObject soundMenu;

    public GameObject graphicsMenu;

    public Toggle MyToggleFullscreen;
    public Toggle MyToggleUltra;
    public Toggle MyToggleStandard;

    public Dropdown myDropdown;

    public Animator animatorCanvas;

    private int indexController;
    private int IndexResolution;

    private int previousIndex;
    private int[] menuState; //0 = menuIndex; 1 = menuLenght;

    private SetVolume SliderScript;

    private bool dropdownOpen = false;

    public AudioSource MusicMenu;
    public AudioSource ValidateChoiceSound;


    public void Start()
    {
        SliderScript = GetComponent<SetVolume>();
        menuState = new int[2];
        fillMenuArray("mainMenu");
        indexController = 0;
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
        Screen.fullScreen = false;
        QualitySettings.SetQualityLevel(0, true);
        if (PlayerPrefs.GetInt("GraphicQuality") == 0)
        {
            QualitySettings.SetQualityLevel(0, true);
            MyToggleUltra.isOn = true;
            MyToggleStandard.isOn = false;
        }
        else if (PlayerPrefs.GetInt("GraphicQuality") == 1)
        {
            QualitySettings.SetQualityLevel(1, true);
            MyToggleUltra.isOn = false;
            MyToggleStandard.isOn = true;
        }
        if (PlayerPrefs.GetInt("Screenmanager Is Fullscreen mode") == 0)
        {
            setFullscreen(true);
            MyToggleFullscreen.isOn = true;
        }
        else
        {
            setFullscreen(false);
            MyToggleFullscreen.isOn = false;
        }
        if (PlayerPrefs.GetInt("ResolutionQuality") == 0)
        {
            myDropdown.value = 0;
            IndexResolution = 0;
        }
        else if (PlayerPrefs.GetInt("ResolutionQuality") == 1) 
        {
            myDropdown.value = 1;
            IndexResolution = 1;
        }
        else if (PlayerPrefs.GetInt("ResolutionQuality") == 2)
        {
            myDropdown.value = 2;
            IndexResolution = 2;
        }
        else if (PlayerPrefs.GetInt("ResolutionQuality") == 3)
        {
            myDropdown.value = 3;
            IndexResolution = 3;
        }
        else if (PlayerPrefs.GetInt("ResolutionQuality") == 4)
        {
            myDropdown.value = 4;
            IndexResolution = 4;
        }
        else
        {
            myDropdown.value = 4;
            IndexResolution = 4;
        }
        setResolution(0);
    }

    private bool checkVerticalAxisDown = false;
    private bool checkVerticalAxisUp = false;
    private bool checkHorizontalAxisRight = false;
    private bool checkHorizontalAxisLeft = false;

    private void Update()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menuSelection();
            SelectionResolution();
            if (Input.GetButtonDown("Jump"))
                menuConfirmSelection();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void setResolution(int resolutionValue)
    {
        if (myDropdown.value == 0)
        {
            Screen.SetResolution(1024, 576, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionQuality", 0);
        }
        else if (myDropdown.value == 1)
        {
            Screen.SetResolution(1280, 720, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionQuality", 1);
        }
        else if (myDropdown.value == 2)
        {
            Screen.SetResolution(1366, 768, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionQuality", 2);
        }
        else if (myDropdown.value == 3)
        {
            Screen.SetResolution(1600, 900, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionQuality", 3);
        }
        else if (myDropdown.value == 4)
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionQuality", 4);
        }
        dropdownOpen = false;
        ValidateChoiceSound.Play();
    }
    public void setFullscreen(bool fullScreenValue)
    {

        Screen.fullScreen = fullScreenValue;
        if (!fullScreenValue)
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, fullScreenValue);
        }
        if (fullScreenValue == true)
            PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
        else
            PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 1);
        ValidateChoiceSound.Play();
    }

    public void setUltra(bool toggleValue)
    {
        if (toggleValue == true)
        {
            PlayerPrefs.SetInt("GraphicQuality", 0);
            QualitySettings.SetQualityLevel(0, true);
            MyToggleUltra.isOn = true;
            MyToggleStandard.isOn = false;
        }
        else
        {
            PlayerPrefs.SetInt("GraphicQuality", 1);
            QualitySettings.SetQualityLevel(1, true);
            MyToggleUltra.isOn = false;
            MyToggleStandard.isOn = true;
        }
        ValidateChoiceSound.Play();
    }
    public void setStandard(bool toggleValue)
    {
        if (toggleValue == true)
        {
            PlayerPrefs.SetInt("GraphicQuality", 1);
            QualitySettings.SetQualityLevel(1, true);
            MyToggleUltra.isOn = false;
            MyToggleStandard.isOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("GraphicQuality", 0);
            QualitySettings.SetQualityLevel(0, true);
            MyToggleUltra.isOn = true;
            MyToggleStandard.isOn = false;
        }
        ValidateChoiceSound.Play();
    }

    public void startGame()
    {
        FadeToLevel();
        ValidateChoiceSound.Play();
    }

    public void FadeToLevel()
    {
        animatorCanvas.SetTrigger("FadeOut");
        StartCoroutine(onFadeComplete(1.0f));
    }

    IEnumerator onFadeComplete(float TimeDelay)
    {
        yield return new WaitForSeconds(TimeDelay);
        SceneManager.LoadScene(GameScene);
    }

    public void quitGame()
    {
        ValidateChoiceSound.Play();
        Application.Quit();
    }

    public void showOption()
    {
        fillMenuArray("optionMenu");
        ValidateChoiceSound.Play();
        StartCoroutine(fadeScene(mainMenu, optionMenu));
    }

    public void showSound()
    {
        fillMenuArray("soundMenu");
        ValidateChoiceSound.Play();
        StartCoroutine(fadeScene(optionMenu, soundMenu));
    }
    public void showGraphics()
    {
        fillMenuArray("graphicsMenu");
        ValidateChoiceSound.Play();
        StartCoroutine(fadeScene(optionMenu, graphicsMenu));
    }

    public void goBackFromOption()
    {
        fillMenuArray("mainMenu");
        ValidateChoiceSound.Play();
        StartCoroutine(fadeScene(optionMenu, mainMenu));
        indexController = 1;
    }

    public void goBackFromSound()
    {
        fillMenuArray("optionMenu");
        ValidateChoiceSound.Play();
        StartCoroutine(fadeScene(soundMenu, optionMenu));
        indexController = 3;
    }

    public void goBackFromGraphics()
    {
        fillMenuArray("optionMenu");
        ValidateChoiceSound.Play();
        StartCoroutine(fadeScene(graphicsMenu, optionMenu));
        indexController = 1;
    }

    IEnumerator fadeScene(GameObject FromMenu, GameObject ToMenu)
    {
        var CanFrom = FromMenu.GetComponent<CanvasGroup>();
        var CanTo = ToMenu.GetComponent<CanvasGroup>();

        for (float f = 0; f <= 0.15f; f += Time.deltaTime)
        {
            CanFrom.alpha = Mathf.Lerp(1f, 0f, f / 0.15f);
            yield return null;
        }
        CanFrom.alpha = 1;

        FromMenu.SetActive(false);
        ToMenu.SetActive(true);

        for (float f = 0; f <= 0.15f; f += Time.deltaTime)
        {
            CanTo.alpha = Mathf.Lerp(0f, 1f, f / 0.15f);
            yield return null;
        }
        CanTo.alpha = 1;
    }

    public void fillMenuArray(string menuName)
    {

        previousIndex = 0;
        indexController = 0;
        if (menuName == "mainMenu")
        {
            menuState[0] = 1;
            menuState[1] = 2;
        }
        else if (menuName == "optionMenu")
        {
            menuState[0] = 2;
            menuState[1] = 4;
        }
        else if (menuName == "graphicsMenu")
        {
            menuState[0] = 3;
            menuState[1] = 3;
        }
        else if (menuName == "soundMenu")
        {
            menuState[0] = 4;
            menuState[1] = 4;
        }
    }

    public void menuSelection()
    {
        if (dropdownOpen == false)
        {
            if (Input.GetAxisRaw("Vertical") == -1)
            {
                if (checkVerticalAxisDown == false)
                {
                    previousIndex = indexController;
                    indexController++;
                    checkVerticalAxisDown = true;
                    if (indexController > menuState[1])
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
                        indexController = menuState[1];
                }
            }
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                checkVerticalAxisDown = false;
                checkVerticalAxisUp = false;
            }

            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                if (checkHorizontalAxisLeft == false)
                {
                    checkHorizontalAxisLeft = true;
                    if (menuState[0] == 4)
                    {
                        if (indexController == 0)
                            SliderScript.SetLevelMasterControler(-0.10f);
                        else if (indexController == 1)
                            SliderScript.SetLevelFootstepsControler(-0.10f);
                        else if (indexController == 2)
                            SliderScript.SetLevelEffectsControler(-0.10f);
                        else if (indexController == 3)
                            SliderScript.SetLevelMusicControler(-0.10f);
                    }
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                if (checkHorizontalAxisRight == false)
                {
                    checkHorizontalAxisRight = true;
                    if (menuState[0] == 4)
                    {
                        if (indexController == 0)
                            SliderScript.SetLevelMasterControler(0.10f);
                        else if (indexController == 1)
                            SliderScript.SetLevelFootstepsControler(0.10f);
                        else if (indexController == 2)
                            SliderScript.SetLevelEffectsControler(0.10f);
                        else if (indexController == 3)
                            SliderScript.SetLevelMusicControler(0.10f);
                    }
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                checkHorizontalAxisLeft = false;
                checkHorizontalAxisRight = false;
            }
        }

        if (menuState[0] == 1)
        {
            mainMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
            mainMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
        }
        else if (menuState[0] == 2)
        {
            if (previousIndex == 0)
                optionMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().color = Color.white;
            else
                optionMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
            if (indexController == 0)
                optionMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
            else
                optionMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
        }

        else if (menuState[0] == 3)
        {
            if (previousIndex == 0)
                graphicsMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetComponentInChildren<Text>().color = Color.white;
            else if (previousIndex == 1)
                graphicsMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().color = Color.white;
            else if (previousIndex == 2)
                graphicsMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().color = Color.white;
            else
                graphicsMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;

            if (indexController == 0)
                graphicsMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
            else if (indexController == 1)
                graphicsMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
            else if (indexController == 2)
                graphicsMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
            else
                graphicsMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
        }

        else if (menuState[0] == 4)
        {
            if (previousIndex != 4)
                soundMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetComponentInChildren<Text>().color = Color.white;
            else
                soundMenu.gameObject.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;


            if (indexController != 4)
                soundMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
            else
                soundMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
        }
    }

    public void menuConfirmSelection()
    {
        if (menuState[0] == 1)
        {
            if (indexController == 0)
                startGame();
            else if (indexController == 1)
            {
                mainMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
                showOption();
            }
            else if (indexController == 2)
                quitGame();
        }
        else if (menuState[0] == 2)
        {
            if (indexController == 0)
            {
                MyToggleFullscreen.isOn = !MyToggleFullscreen.isOn;
                setFullscreen(!Screen.fullScreen);
            }
            else if (indexController == 1)
            {
                optionMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
                showGraphics();
            }
            else if (indexController == 2)
            {
                optionMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
                // goBackFromOption(); toDo
            }
            else if (indexController == 3)
            {
                optionMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
                showSound();
            }
            else if (indexController == 4)
            {
                optionMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
                goBackFromOption();
            }
        }
        else if (menuState[0] == 3)
        {
            if (indexController == 0)
            {
                if (dropdownOpen == false)
                {
                    myDropdown.Show();
                    dropdownOpen = true;
                }
                else
                {
                    myDropdown.Hide();
                    dropdownOpen = false;
                }
            }
            else if (indexController == 1)
            {
                if (MyToggleUltra.isOn == true)
                    setUltra(false);
                if (MyToggleUltra.isOn == false)
                    setUltra(true);
            }
            else if (indexController == 2)
            {
                if (MyToggleStandard.isOn == true)
                    setStandard(false);
                if (MyToggleStandard.isOn == false)
                    setStandard(true);
            }
            else if (indexController == 3)
            {
                graphicsMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
                goBackFromGraphics();
            }
        }
        else if (menuState[0] == 4)
        {
            if (indexController == 4)
            {
                soundMenu.gameObject.transform.GetChild(indexController).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().color = Color.white;
                goBackFromSound();
            }
        }
    }

    public void SelectionResolution()
    {
        if (dropdownOpen == true)
        {
            if (Input.GetAxis("Vertical") < -0.6f)
            {
                if (checkVerticalAxisDown == false)
                {
                    if (IndexResolution < 4)
                    {
                        IndexResolution++;
                        myDropdown.value = IndexResolution;
                        myDropdown.Show();
                        dropdownOpen = true;
                    }
                    checkVerticalAxisDown = true;
                }
            }
            if (Input.GetAxis("Vertical") > 0.6f)
            {
                if (checkVerticalAxisUp == false)
                {
                    if (IndexResolution > 0)
                    {
                        IndexResolution--;
                        myDropdown.value = IndexResolution;
                        myDropdown.Show();
                        dropdownOpen = true;
                    }
                    checkVerticalAxisUp = true;
                }
            }
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                checkVerticalAxisDown = false;
                checkVerticalAxisUp = false;
            }
            if (Input.GetAxis("Horizontal") > 0.6f)
            {
                if (checkHorizontalAxisLeft == false)
                {
                    if (IndexResolution < 4)
                    {
                        IndexResolution++;
                        myDropdown.value = IndexResolution;
                        myDropdown.Show();
                        dropdownOpen = true;
                    }
                    checkHorizontalAxisLeft = true;
                }
            }
            if (Input.GetAxis("Horizontal") < -0.6f)
            {
                if (checkHorizontalAxisRight == false)
                {
                    if (IndexResolution > 0)
                    {
                        IndexResolution--;
                        myDropdown.value = IndexResolution;
                        myDropdown.Show();
                        dropdownOpen = true;
                    }
                    checkHorizontalAxisRight = true;
                }
            }
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                checkHorizontalAxisLeft = false;
                checkHorizontalAxisRight = false;
            }
        }
    }
}
