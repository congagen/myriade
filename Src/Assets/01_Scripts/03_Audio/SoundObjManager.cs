using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundObjManager : MonoBehaviour
{

    public Ray ray;
    public RaycastHit hit;

    private string objSampleKey;
    private string objAmpKey;
    private string objMinPitchKey;
    private string objMaxPitchKey;
    private string objLoopDurationKey;

    public GameObject editorUI;
    public GameObject mainUI;


    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1)) {
            mainUI.SetActive(true);
            editorUI.SetActive(false);
        }


        if (Input.GetMouseButtonDown(0))
        {            
            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.collider.gameObject;
                string dbId = ((int)obj.transform.position.x).ToString();
                dbId += "-" + ((int)obj.transform.position.y).ToString();
                dbId += "-" + ((int)obj.transform.position.z).ToString();

                PlayerPrefs.SetString("selectedSoundObj", dbId);
                string objSample = PlayerPrefs.GetString(dbId + objSampleKey);
                string objAmp = PlayerPrefs.GetString(dbId + objAmpKey);
                string objMinPitch = PlayerPrefs.GetString(dbId + objMinPitchKey);
                string objMaxPitch = PlayerPrefs.GetString(dbId + objMaxPitchKey);
                string objLoopDuration = PlayerPrefs.GetString(dbId + objLoopDurationKey);

                PlayerPrefs.SetString(dbId, "YES");
                hit.collider.gameObject.GetComponent<SoundObjEditor>().isSelected = true;

                mainUI.SetActive(false);
                editorUI.SetActive(true);

            }
        }
    }
}
