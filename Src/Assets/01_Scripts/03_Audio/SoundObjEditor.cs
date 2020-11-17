using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundObjEditor : MonoBehaviour
{

    public GameObject editorIcons;
    public GameObject editorBtnA;
    public GameObject editorBtnB;
    public GameObject highLightObj;
    public bool isSelected = false;

    void Update()
    {
        string dbId = ((int)transform.position.x).ToString();
        dbId += "-" + ((int)transform.position.y).ToString();
        dbId += "-" + ((int)transform.position.z).ToString();

        if (Input.GetMouseButtonDown(1))
        {
            isSelected = false;
        }

        if (isSelected && !editorIcons.activeSelf)
        {
            editorIcons.SetActive(true);
            Debug.Log("Reading: " + dbId);
            Debug.Log(PlayerPrefs.GetString(dbId));
        }

        if (!isSelected && editorIcons.activeSelf)
        {
            editorIcons.SetActive(false);
            Debug.Log(PlayerPrefs.GetString(dbId));
        }

    }

    private void OnDisable()
    {
        isSelected = false;
    }

    private void Awake()
    {
        isSelected = false;
    }
}
