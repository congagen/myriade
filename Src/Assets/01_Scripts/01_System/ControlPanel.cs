using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Networking;

using System.IO;


public class ControlPanel : MonoBehaviour
{
    //--------------------------------------------------------------------------
    [Header("Project")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public GameObject listenerObj;

    public GameObject controlPanelObj;
    public bool demoMode = false;
    public string appVersion = "1.0";

    private bool isInit = false;
    public bool lockData = false;
    public Canvas mainUI;

    //--------------------------------------------------------------------------
    [Header("Audio")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public AudioMixer mixer;

    public float initStereo = 70;

    // Chance
    public float sliderAInitVal = 50f;

    // Min Pitch
    public float sliderBInitVal = 0.1f;
    // Max Pitch
    public float sliderCInitVal = 0.8f;

    // Min Rate
    public float sliderDInitVal = 0.1f;
    // Max Rate
    private float sliderEInitVal = 4f;

    // Nav Speed
    public float sliderFInitVal = 1.0f;

    // Rotation
    public float sliderGInitVal = 0.0f;

    // Wander Speed
    public float sliderHInitVal = 1;

    [Space(15)]

    public List<string> demoSamplePaths;
    // public List<string> importFolderPaths;

    [HideInInspector]
    public string importFolderDataKey = "importFolderPath";

    public Dictionary<int, List<AudioClip>> sampleFolderDict = new Dictionary<int, List<AudioClip>>();

    //--------------------------------------------------------------------------
    [Header("Buttons")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public Button loadButton;
    public Button saveButton;

    public Button changeSampleDirButton;    //public Text playbackButtonText;

    public Button resetButton;
    public Button randomButton;

    //public Button fullscreenToggleBtn;

    //--------------------------------------------------------------------------
    [Header("UI Sliders")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public Slider volumeSlider;
    public string ampDataKey = "masterVolume";

    [Space(10)]

    public Slider sliderA;
    public InputField sliderAValue;
    public string sliderADataKey = "slider_a";

    [Space(10)]

    public Slider sliderB;
    public InputField sliderBValue;
    public string sliderBDataKey = "slider_b";

    [Space(10)]

    public Slider sliderC;
    public InputField sliderCValue;
    public string sliderCDataKey = "slider_c";

    [Space(10)]

    public Slider sliderD;
    public InputField sliderDValue;
    public string sliderDDataKey = "slider_d";

    [Space(10)]

    public Slider sliderE;
    public InputField sliderEValue;
    public string sliderEDataKey = "slider_e";

    [Space(10)]

    public Slider sliderF;
    public InputField sliderFValue;
    public string sliderFDataKey = "slider_f";

    [Space(10)]

    public Slider sliderG;
    public InputField sliderGValue;
    public string sliderGDataKey = "slider_g";

    [Space(10)]

    public Slider sliderH;
    public InputField sliderHValue;
    public string sliderHDataKey = "slider_h";

    [Space(10)]

    //public Dropdown playModeDropdown;
    public string playModeDataKey = "playmode";


    [Space(10)]
    public Dropdown sampleDropDown;
    [Space(10)]


    public Dropdown seqModeDropdown;
    public string seqModeDataKey = "seqMode";

    [Space(10)]

    public Dropdown pitchModeDropdown;
    public string pitchModeDataKey = "pitchMode";

    //--------------------------------------------------------------------------
    [Header("Conf")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public bool init = false;
    public bool isPlaying = false;

    // public double playbackPosition = 0;
    //public InputField playbackLabel;
    public GameObject navObject;
    public InputField xPosLabel;
    public InputField zPosLabel;

    public string xPosDataKey = "xPos";
    public string yPosDataKey = "yPos";
    public string zPosDataKey = "zPos";

    public string playbackPositionDataKey = "playbackPosition";

    //public Button logo;

    //float zoomSensitivity = 0.1f;


    void toggleWander()
    {
        Debug.Log("toggleWander");
        if (PlayerPrefs.GetInt("wander") == 1)
        {
            PlayerPrefs.SetInt("wander", 0);
        }
        else
        {
            PlayerPrefs.SetInt("wander", 1);
        }
        Debug.Log(PlayerPrefs.GetInt("wander"));
    }


    void togglePlayback()
    {
        Debug.Log("togglePlayback");
        if (PlayerPrefs.GetInt("play_audio") != 0)
        {
            Debug.Log("A");
            PlayerPrefs.SetInt("play_audio", 0);
        }
        else
        {
            Debug.Log("B");
            PlayerPrefs.SetInt("play_audio", 1);
        }
        Debug.Log(PlayerPrefs.GetInt("play_audio"));
    }


    void setPlayback(int state)
    {
        Debug.Log("setPlayback");
        if (state == 0)
        {
            PlayerPrefs.SetInt("play_audio", 0);
        }
        else
        {
            PlayerPrefs.SetInt("play_audio", 1);
        }
        Debug.Log(PlayerPrefs.GetInt("play_audio"));
    }


    //void loadProject()
    //{
    //    Debug.Log("Load Project..");
    //    setPlayback(0);

    //    transform.GetComponent<ProjectDataMgmt>().LoadProject();

    //    updateSamples();
    //}


    public void updateSamples()
    {
        Debug.Log("updateSamples");

        sampleDropDown.ClearOptions();
        sampleFolderDict.Clear();
        int idx = 0;

        List<string> dropDownItems = new List<string>();
        string folderPath = PlayerPrefs.GetString(importFolderDataKey);

        if (folderPath != "")
        {
            string[] filePaths = Directory.GetFiles(folderPath);

            if (filePaths.Length > 0)
            {
                sampleFolderDict[idx] = new List<AudioClip>();
                dropDownItems.Add("Imported");

                // Add ImportFolder Dir
                foreach (string filePath in filePaths)
                {
                    Debug.Log(Path.GetExtension(filePath).ToLower());

                    if (Path.HasExtension(filePath))
                    {
                        if (Path.GetExtension(filePath).ToLower() == ".wav")
                        {
                            StartCoroutine(LoadAudio(filePath, idx));
                        }
                    }
                }

                idx += 1;

                // Add ImportFolder Files
                foreach (string filePath in filePaths)
                {
                    Debug.Log(Path.GetExtension(filePath).ToLower());

                    if (Path.HasExtension(filePath))
                    {
                        if (Path.GetExtension(filePath).ToLower() == ".wav")
                        {
                            string fileName = Path.GetFileName(filePath);
                            StartCoroutine(LoadAudio(filePath, idx));
                            dropDownItems.Add(fileName);
                            idx += 1;
                        }
                    }
                }
            }
        }

        // Add Default Sample
        foreach (string p in demoSamplePaths)
        {
            AudioClip[] clipFiles = Resources.LoadAll<AudioClip>(p);
            sampleFolderDict[idx] = new List<AudioClip>();

            foreach (AudioClip a in clipFiles)
            {
                sampleFolderDict[idx].Add(a);
            }

            dropDownItems.Add(p);
            idx += 1;
        }

        sampleDropDown.AddOptions(dropDownItems);

    }


    //void saveProject() {
    //    Debug.Log("Save Project..");

    //    ProjectData p = new ProjectData();

    //    p.appVersion = appVersion;

    //    p.xPos = PlayerPrefs.GetFloat(xPosDataKey);
    //    p.yPos = PlayerPrefs.GetFloat(yPosDataKey);
    //    p.zPos = PlayerPrefs.GetFloat(zPosDataKey);

    //    p.playMode = PlayerPrefs.GetInt(playModeDataKey);
    //    p.seqMode = PlayerPrefs.GetInt(seqModeDataKey);
    //    p.pitchMode = PlayerPrefs.GetInt(pitchModeDataKey);

    //    p.sliderA = PlayerPrefs.GetFloat(sliderADataKey);
    //    p.sliderB = PlayerPrefs.GetFloat(sliderBDataKey);
    //    p.sliderC = PlayerPrefs.GetFloat(sliderCDataKey);
    //    p.sliderD = PlayerPrefs.GetFloat(sliderDDataKey);
    //    p.sliderE = PlayerPrefs.GetFloat(sliderEDataKey);
    //    p.sliderF = PlayerPrefs.GetFloat(sliderFDataKey);
    //    p.sliderG = PlayerPrefs.GetFloat(sliderGDataKey);
    //    p.sliderH = PlayerPrefs.GetFloat(sliderHDataKey);

    //    p.selectedSample = sampleDropDown.options[sampleDropDown.value].text;

    //    p.playbackPosition = transform.GetComponent<PlaybackClock>().playbackPositionOffset.ToString()+ transform.GetComponent<PlaybackClock>().playbackPosition.ToString();

    //    if (PlayerPrefs.HasKey(importFolderDataKey)) {
    //        if (PlayerPrefs.GetString(importFolderDataKey) != "") {
    //            p.importPath = PlayerPrefs.GetString(importFolderDataKey);
    //        }
    //    }

    //    transform.GetComponent<ProjectDataMgmt>().SaveProject(p);
    //}


    void resetProject()
    {
        Debug.Log("Reset Project");


        PlayerPrefs.SetFloat(ampDataKey, 50);
        PlayerPrefs.SetInt(seqModeDataKey, 0);
        PlayerPrefs.SetInt(pitchModeDataKey, 0);

        PlayerPrefs.SetFloat(sliderADataKey, sliderAInitVal);
        PlayerPrefs.SetFloat(sliderBDataKey, sliderBInitVal);
        PlayerPrefs.SetFloat(sliderCDataKey, sliderCInitVal);
        PlayerPrefs.SetFloat(sliderDDataKey, sliderDInitVal);
        PlayerPrefs.SetFloat(sliderEDataKey, sliderEInitVal);
        PlayerPrefs.SetFloat(sliderFDataKey, sliderFInitVal);
        PlayerPrefs.SetFloat(sliderGDataKey, sliderGInitVal);
        PlayerPrefs.SetFloat(sliderHDataKey, sliderHInitVal);
    }


    private WWW GetAudioFromFile(string path)
    {
        string audioToLoad = string.Format(path);
        WWW request = new WWW(audioToLoad);
        return request;
    }


    void UpdateSampleDir()
    {
        Debug.Log("Load Samples..");
        setPlayback(0);

        transform.GetComponent<ProjectDataMgmt>().ImportSampleFolder();
    }


    private IEnumerator LoadAudio(string filePath, int sampledictIndex)
    {
        WWW request = GetAudioFromFile("file://" + filePath);
        yield return request;
        AudioClip audioClip = request.GetAudioClip();
        string fileName = Path.GetFileName(filePath);
        audioClip.name = Path.GetFileName(filePath);

        if (sampleFolderDict.ContainsKey(sampledictIndex))
        {
            sampleFolderDict[sampledictIndex].Add(audioClip);
        }
        else
        {
            sampleFolderDict[sampledictIndex] = new List<AudioClip>();
            sampleFolderDict[sampledictIndex].Add(audioClip);
        }
    }


    void initUI()
    {
        navObject.transform.position = new Vector3(0, 0, 0);

        if (PlayerPrefs.HasKey(ampDataKey))
        {
            volumeSlider.value = PlayerPrefs.GetFloat(ampDataKey);
            mixer.SetFloat(ampDataKey, volumeSlider.value);
        }

        if (PlayerPrefs.HasKey(sliderADataKey))
        {
            sliderA.value = PlayerPrefs.GetFloat(sliderADataKey);
        }

        if (PlayerPrefs.HasKey(sliderBDataKey))
        {
            sliderB.value = PlayerPrefs.GetFloat(sliderBDataKey);
        }

        if (PlayerPrefs.HasKey(sliderCDataKey))
        {
            sliderC.value = PlayerPrefs.GetFloat(sliderCDataKey);
        }

        if (PlayerPrefs.HasKey(sliderDDataKey))
        {
            sliderD.value = PlayerPrefs.GetFloat(sliderDDataKey);
        }

        if (PlayerPrefs.HasKey(sliderEDataKey))
        {
            sliderE.value = PlayerPrefs.GetFloat(sliderEDataKey);
        }

        if (PlayerPrefs.HasKey(sliderFDataKey))
        {
            sliderF.value = PlayerPrefs.GetFloat(sliderFDataKey);
        }

        if (PlayerPrefs.HasKey(sliderGDataKey))
        {
            sliderG.value = PlayerPrefs.GetFloat(sliderGDataKey);
        }

        if (PlayerPrefs.HasKey(sliderHDataKey))
        {
            sliderH.value = PlayerPrefs.GetFloat(sliderHDataKey);
        }

        if (PlayerPrefs.HasKey(seqModeDataKey))
        {
            seqModeDropdown.value = PlayerPrefs.GetInt(seqModeDataKey);
        }

        if (PlayerPrefs.HasKey(pitchModeDataKey))
        {
            pitchModeDropdown.value = PlayerPrefs.GetInt(pitchModeDataKey);
        }

        PlayerPrefs.SetInt("play_audio", 0);

    }


    void updateData()
    {

        if (!lockData)
        {
            PlayerPrefs.SetFloat("xPos", navObject.transform.position.x);
            PlayerPrefs.SetFloat("yPos", navObject.transform.position.y);
            PlayerPrefs.SetFloat("zPos", navObject.transform.position.z);
        }

        if (PlayerPrefs.HasKey(ampDataKey))
        {
            if (!volumeSlider.IsInvoking())
            {
                volumeSlider.value = PlayerPrefs.GetFloat(ampDataKey);
                mixer.SetFloat(ampDataKey, ((float)(volumeSlider.value / 200) * 80) - 80);
            }
        }

        //if (!playbackLabel.isFocused) { 
        //    playbackLabel.text = transform.GetComponent<PlaybackClock>().playbackPosition.ToString();
        //}

        if (PlayerPrefs.HasKey(sliderADataKey) && !sliderAValue.isFocused)
        {
            sliderAValue.text = PlayerPrefs.GetFloat(sliderADataKey).ToString();
            if (!sliderA.IsInvoking())
            {
                sliderA.value = PlayerPrefs.GetFloat(sliderADataKey);
            }
        }

        if (PlayerPrefs.HasKey(sliderBDataKey) && !sliderBValue.isFocused)
        {
            sliderBValue.text = PlayerPrefs.GetFloat(sliderBDataKey).ToString();
            if (!sliderB.IsInvoking())
            {
                sliderB.value = PlayerPrefs.GetFloat(sliderBDataKey);
            }
        }

        if (PlayerPrefs.HasKey(sliderCDataKey) && !sliderCValue.isFocused)
        {
            sliderCValue.text = PlayerPrefs.GetFloat(sliderCDataKey).ToString();
            if (!sliderC.IsInvoking())
            {
                sliderC.value = PlayerPrefs.GetFloat(sliderCDataKey);
            }
        }

        if (PlayerPrefs.HasKey(sliderDDataKey) && !sliderDValue.isFocused)
        {
            sliderDValue.text = PlayerPrefs.GetFloat(sliderDDataKey).ToString();
            if (!sliderD.IsInvoking())
            {
                sliderD.value = PlayerPrefs.GetFloat(sliderDDataKey);
            }
        }

        if (PlayerPrefs.HasKey(sliderEDataKey) && !sliderEValue.isFocused)
        {
            sliderEValue.text = PlayerPrefs.GetFloat(sliderEDataKey).ToString();
            if (!sliderE.IsInvoking())
            {
                sliderE.value = PlayerPrefs.GetFloat(sliderEDataKey);
            }
            // mixer.SetFloat("reverbWet", (((float)(PlayerPrefs.GetFloat(sliderEDataKey) / 100) * 80) - 80));
        }

        if (PlayerPrefs.HasKey(sliderFDataKey) && !sliderFValue.isFocused)
        {
            sliderFValue.text = PlayerPrefs.GetFloat(sliderFDataKey).ToString();
            if (!sliderF.IsInvoking())
            {
                sliderF.value = PlayerPrefs.GetFloat(sliderFDataKey);
            }
        }

        if (PlayerPrefs.HasKey(sliderGDataKey) && !sliderGValue.isFocused)
        {
            sliderGValue.text = PlayerPrefs.GetFloat(sliderGDataKey).ToString();
            if (!sliderG.IsInvoking())
            {
                sliderG.value = PlayerPrefs.GetFloat(sliderGDataKey);
            }
        }

        if (PlayerPrefs.HasKey(sliderHDataKey) && !sliderHValue.isFocused)
        {
            sliderHValue.text = PlayerPrefs.GetFloat(sliderHDataKey).ToString();
            if (!sliderH.IsInvoking())
            {
                sliderH.value = PlayerPrefs.GetFloat(sliderHDataKey);
            }
        }

        //PlayerPrefs.SetFloat(ampDataKey, volumeSlider.value);
    }


    void toggleFullscreen()
    {

        mainUI.gameObject.SetActive(!mainUI.gameObject.activeSelf);

    }


    void UpdateInput()
    {

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (PlayerPrefs.GetInt("showTrails") == 1)
            {
                PlayerPrefs.SetInt("showTrails", 0);
            }
            else
            {
                PlayerPrefs.SetInt("showTrails", 1);
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("Toggle Wander");
            toggleWander();
        }

    }


    void playKey()
    {
        Debug.Log("1");
        Debug.Log(Input.GetKey(""));
    }


    void manualValueFloat(string paramKey, float paramValue, Slider paramSlider)
    {
        if (!lockData)
        {
            paramSlider.value = paramValue;
            PlayerPrefs.SetFloat(paramKey, paramValue);
        }

    }


    void manualValueInt(string paramKey, int paramValue, Slider paramSlider)
    {
        if (!lockData)
        {
            paramSlider.value = paramValue;
            PlayerPrefs.SetInt(paramKey, paramValue);
        }
    }


    public void updatePlaybackPosition(decimal newPosition)
    {
        transform.GetComponent<PlaybackClock>().playbackPosition = (decimal)newPosition;
        PlayerPrefs.SetString(playbackPositionDataKey, transform.GetComponent<PlaybackClock>().playbackPosition.ToString());
    }


    void RandomizePrefsB()
    {
        float xP = Random.Range(-999999, 999999);
        float zP = Random.Range(-999999, 999999);

        navObject.transform.position = new Vector3(xP, 0, zP);
    }


    void RandomizePrefsA()
    {

        transform.GetComponent<ControlPanel>().lockData = true;

        PlayerPrefs.SetFloat(sliderADataKey, Random.Range(20, 60f));

        PlayerPrefs.SetFloat(sliderBDataKey, Random.Range(0.01f, 0.5f));
        PlayerPrefs.SetFloat(sliderCDataKey, Random.Range(0.1f, 1f));

        PlayerPrefs.SetFloat(sliderDDataKey, Random.Range(0.1f, 0.5f));
        PlayerPrefs.SetFloat(sliderEDataKey, Random.Range(0.5f, 5f));

        PlayerPrefs.SetFloat(sliderFDataKey, Random.Range(0f, 5));

        PlayerPrefs.SetFloat(sliderGDataKey, Random.Range(0f, 100f));
        PlayerPrefs.SetFloat(sliderHDataKey, Random.Range(0f, 50));

        transform.GetComponent<ControlPanel>().lockData = false;
    }


    void Update()
    {

        if (isInit)
        {
            updateData();
            UpdateInput();

            if (!xPosLabel.isFocused)
            {
                xPosLabel.text = ((float)navObject.transform.position.x).ToString();
            }
            else
            {
                PlayerPrefs.SetFloat(xPosDataKey, float.Parse(xPosLabel.text));
                navObject.transform.position = new Vector3(
                    (float)float.Parse(xPosLabel.text),
                    navObject.transform.position.y,
                    navObject.transform.position.z
                    );
            }

            if (!zPosLabel.isFocused)
            {
                zPosLabel.text = ((float)navObject.transform.position.z).ToString();
            }
            else
            {
                PlayerPrefs.SetFloat(yPosDataKey, float.Parse(zPosLabel.text));
                navObject.transform.position = new Vector3(
                    navObject.transform.position.x,
                    navObject.transform.position.y,
                    (float)float.Parse(zPosLabel.text)
                    );
            }

        }
        else
        {
            PlayerPrefs.DeleteAll();
            resetProject();

            transform.GetComponent<PlaybackClock>().playbackPosition = 0;
            updateSamples();

            //loadButton.onClick.AddListener( delegate{ loadProject(); });
            //saveButton.onClick.AddListener( delegate{ saveProject(); });
            //changeSampleDirButton.onClick.AddListener(delegate { UpdateSampleDir(); });

            randomButton.onClick.AddListener(RandomizePrefsA);
            resetButton.onClick.AddListener(resetProject);

            seqModeDropdown.onValueChanged.AddListener(delegate { PlayerPrefs.SetInt(seqModeDataKey, seqModeDropdown.value); });
            pitchModeDropdown.onValueChanged.AddListener(delegate { PlayerPrefs.SetInt(pitchModeDataKey, pitchModeDropdown.value); });

            volumeSlider.onValueChanged.AddListener(delegate { manualValueFloat(ampDataKey, volumeSlider.value, volumeSlider); });
            volumeSlider.onValueChanged.AddListener(delegate { mixer.SetFloat(ampDataKey, ((float)(volumeSlider.value / 100) * 80) - 80); });

            sliderA.onValueChanged.AddListener(delegate { manualValueFloat(sliderADataKey, sliderA.value, sliderA); });
            sliderAValue.onEndEdit.AddListener(delegate { manualValueFloat(sliderADataKey, float.Parse(sliderAValue.text), sliderA); });

            sliderB.onValueChanged.AddListener(delegate { manualValueFloat(sliderBDataKey, sliderB.value, sliderB); });
            sliderBValue.onEndEdit.AddListener(delegate { manualValueFloat(sliderBDataKey, float.Parse(sliderBValue.text), sliderB); });

            sliderC.onValueChanged.AddListener(delegate { manualValueFloat(sliderCDataKey, sliderC.value, sliderC); });
            sliderCValue.onEndEdit.AddListener(delegate { manualValueFloat(sliderCDataKey, float.Parse(sliderCValue.text), sliderC); });

            sliderD.onValueChanged.AddListener(delegate { manualValueFloat(sliderDDataKey, sliderD.value, sliderD); });
            sliderDValue.onEndEdit.AddListener(delegate { manualValueFloat(sliderDDataKey, float.Parse(sliderDValue.text), sliderD); });

            sliderE.onValueChanged.AddListener(delegate { manualValueFloat(sliderEDataKey, sliderE.value, sliderE); });
            sliderEValue.onEndEdit.AddListener(delegate { manualValueFloat(sliderEDataKey, float.Parse(sliderEValue.text), sliderE); });

            sliderF.onValueChanged.AddListener(delegate { manualValueFloat(sliderFDataKey, sliderF.value, sliderF); });
            sliderFValue.onEndEdit.AddListener(delegate { manualValueFloat(sliderFDataKey, float.Parse(sliderFValue.text), sliderF); });

            sliderG.onValueChanged.AddListener(delegate { manualValueFloat(sliderGDataKey, sliderG.value, sliderG); });
            sliderGValue.onEndEdit.AddListener(delegate { manualValueFloat(sliderGDataKey, float.Parse(sliderGValue.text), sliderG); });

            sliderH.onValueChanged.AddListener(delegate { manualValueFloat(sliderHDataKey, sliderH.value, sliderH); });
            sliderHValue.onEndEdit.AddListener(delegate { manualValueFloat(sliderHDataKey, float.Parse(sliderHValue.text), sliderH); });

            mixer.SetFloat("reverbWet", 50);

            initUI();
            isInit = true;

        }

    }


}

