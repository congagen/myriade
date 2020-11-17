using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIMgmt : MonoBehaviour
{
    public Button settingsBtn;
    public Button helpBtn;
    public Button exitBtn_S;
    public Button exitBtn_H;
    public Button playbackButton;

    [Space(10)]

    public GameObject navMarker;

    [Space(10)]

    public GameObject controlPanel;
    public GameObject helpScreen;
    public GameObject mainHud;

    public bool theatherMode = false;

    //public Ray ray;
    //public RaycastHit hit;


    void toggleSettingsScreen() 
    {
        helpScreen.SetActive(false);
        controlPanel.SetActive(!controlPanel.active);

        if (controlPanel.active) {
            mainHud.SetActive(false);
        }
        else {
            mainHud.SetActive(true);
        }
    }

    void toggleHelpScreen()
    {
        controlPanel.SetActive(false);
        helpScreen.SetActive(!helpScreen.active);

        if (helpScreen.active) {
            mainHud.SetActive(false);
        }
        else {
            mainHud.SetActive(true);
        }
    }


    void togglePlayback()
    {
        Debug.Log("togglePlayback");
        if (PlayerPrefs.GetInt("play_audio") != 0) {
            PlayerPrefs.SetInt("play_audio", 0);

        }
        else {
            PlayerPrefs.SetInt("play_audio", 1);
        }
    }


    void exitUI() {
        controlPanel.SetActive(false);
        helpScreen.SetActive(false);
        mainHud.SetActive(true);
    }


    void initUI() {
        controlPanel.SetActive(false);
        helpScreen.SetActive(false);
        mainHud.SetActive(true);
    }


    void Start() {
        helpScreen.SetActive(false);
        controlPanel.SetActive(false);

        playbackButton.onClick.AddListener(delegate { togglePlayback(); });

        exitBtn_S.onClick.AddListener(delegate { exitUI(); });
        exitBtn_H.onClick.AddListener(delegate { exitUI(); });

        settingsBtn.onClick.AddListener(delegate { toggleSettingsScreen(); });
        helpBtn.onClick.AddListener(delegate { toggleHelpScreen(); });
    }


    void Update()
    {

        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.U)) {
            theatherMode = !theatherMode;

            if (!theatherMode) {
                initUI();
            }
        }

        if (theatherMode || PlayerPrefs.GetFloat("slider_g") < 0.2f) {
            navMarker.SetActive(false);
        } else {
            if (PlayerPrefs.GetFloat("slider_g") > 0.2f) {
                Debug.Log("Set Active");
                if (!navMarker.active) {
                    navMarker.SetActive(true);
                }
                Debug.Log("A");
            }

        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            togglePlayback();
        }

        if (theatherMode) {
            mainHud.SetActive(false);
            helpScreen.SetActive(false);
            controlPanel.SetActive(false);
        }

        if (PlayerPrefs.GetInt("play_audio") == 1) {
            playbackButton.transform.Find("Text").GetComponent<Text>().text = "II";
        }
        else {
            playbackButton.transform.Find("Text").GetComponent<Text>().text = ">";
        }

        if (controlPanel.activeSelf) {
            PlayerPrefs.SetInt("control_panel", 1);
        }
        else {
            PlayerPrefs.SetInt("control_panel", 0);
        }


    }

}
