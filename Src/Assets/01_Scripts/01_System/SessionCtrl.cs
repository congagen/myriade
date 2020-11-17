using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SessionCtrl : MonoBehaviour
{
    public GameObject helpScreen;


    public string mapSizeKeyword = "MapSize";
    public string playbackKeyword = "play_audio";


    void manageKeyboard() {


        // Play / Pause
        if (Input.GetKeyUp(KeyCode.H))
        {
            helpScreen.SetActive(!helpScreen.active);
        }


        // Play / Pause
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    int cur = PlayerPrefs.GetInt(playbackKeyword);

        //    if (cur == 1)
        //    {
        //        PlayerPrefs.SetInt(playbackKeyword, 0);
        //    }
        //    else
        //    {
        //        PlayerPrefs.SetInt(playbackKeyword, 1);
        //    }
        //}

        // Map Size
        //if (Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    int cur = PlayerPrefs.GetInt(mapSizeKeyword);
        //    if (cur < 100)
        //    {
        //        PlayerPrefs.SetInt(mapSizeKeyword, cur + 1);
        //    }
        //}

        //if (Input.GetKeyUp(KeyCode.DownArrow))
        //{
        //    int cur = PlayerPrefs.GetInt(mapSizeKeyword);
        //    if (cur > 1)
        //    {
        //        PlayerPrefs.SetInt(mapSizeKeyword, cur - 1);
        //    }
        //}
    }


    void Update()
    {
        manageKeyboard();
    }

}
