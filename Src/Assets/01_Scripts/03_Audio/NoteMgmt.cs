using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using System.Linq;

[RequireComponent (typeof(AudioSource))]
public class NoteMgmt: MonoBehaviour
{
    public GameObject settingsObj;

    public int noteIndex;
    private float noteColor;
    public int playCount = 0;

    public float minAmp = 0.05f;
    private float ampVolMulti = 20f;

    public bool showTrail = false;

    private float playbackClock = 0;
    float sinVal = 0;
    float cosVal = 0;

    //private bool locked;
    private AudioSource audioPlayer;

    private Color activeColor  = new Color(0.25f, 0.25f, 0.25f, 1f);
    private Color defaultColor = new Color(0.15f, 0.15f, 0.9f, 1f);

    public float maxRepeatRate = 3f;
    public float minRepeatRate = 0.1f;
    public float repeatRate = 2;

    public float trailR = 0;
    public float trailG = 0;
    public float trailB = 0;

    //float timeSinceHit = 0;


    void Start () {
        //locked = true;
        audioPlayer = transform.GetComponent<AudioSource>();

        //for (int i = 1; i < 128; i++) {
        //    AudioSource a = gameObject.AddComponent<AudioSource>();
        //    a = audioPlayer;
        //}    
    }


    void playNote() // Mathf.Pow(2, (note+transpose)/12.0);
    {
        audioPlayer.enabled = true;
        audioPlayer.mute = false;
        audioPlayer.ignoreListenerPause = true;
        audioPlayer.ignoreListenerVolume = true;

        playCount += 1;
        int currentFolder = settingsObj.GetComponent<ControlPanel>().sampleDropDown.value;
        List<AudioClip> sampleList = settingsObj.GetComponent<ControlPanel>().sampleFolderDict[currentFolder];
        if (sampleList.Count == 0) { return; }

        float semi = PlayerPrefs.GetFloat(settingsObj.GetComponent<ControlPanel>().sliderGDataKey);

        if (audioPlayer.clip != sampleList[0]) {
            audioPlayer.clip = sampleList[0];
        }

        bool multisample = sampleList.Count > 1;
        float amplitude = 0.5f;

        float musicScale = PlayerPrefs.GetFloat(settingsObj.GetComponent<ControlPanel>().sliderFDataKey);

        audioPlayer.priority = 0;
        audioPlayer.volume = 1;
        audioPlayer.pitch = 1 + semi;

        Debug.Log(sampleList);

        if (multisample) {
            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 0) {
                Debug.Log("AHA");
                int sIndex = Random.Range(0, sampleList.Count - 1);
                audioPlayer.clip = sampleList[sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 1) {
                Debug.Log("1");
                float piSize = Mathf.PI / sampleList.Count;
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin((float)noteIndex * piSize)));
                audioPlayer.clip = sampleList[sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 2) {
                Debug.Log("2");
                float piSize = Mathf.PI / sampleList.Count;
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin((float)noteIndex * piSize)));
                audioPlayer.clip = sampleList[(sampleList.Count - 1) - sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 3) {
                float piSize = Mathf.PI / sampleList.Count;
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin((float)playCount * piSize)));
                audioPlayer.clip = sampleList[sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 4) {
                float piSize = Mathf.PI / (sampleList.Count);
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin(((float)playCount * noteIndex) * piSize)));
                audioPlayer.clip = sampleList[sIndex];
            }

        } else {
            audioPlayer.clip = sampleList[0];
            audioPlayer.volume = 1;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 0) {
            audioPlayer.pitch = 1 + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 1) {
            float p = 3f / (musicScale / ((float)noteIndex + 1f));
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 2) {
            float p = (float)Mathf.Pow(2.0f, ((float)noteIndex / musicScale));
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 2) {
            float p = (float)Mathf.Pow(2.0f, ((float)noteIndex / musicScale));
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 3) {
            float ah = Mathf.PI / musicScale;
            float p = musicScale * (Mathf.Abs((float)Mathf.Sin(playCount * (float)noteIndex)) * ah);
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 4) {
            float ah = Mathf.PI / musicScale;
            float p = musicScale * (Mathf.Abs((float)Mathf.Sin(playCount)) * ah);
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 5) {
            float p = 3 / Random.Range(0.1f, musicScale);
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        //Debug.Log("Samples: " + sampleList.Count.ToString());
        //Debug.Log("Amp:     " + amplitude.ToString());

        if (!audioPlayer.isPlaying) {
            audioPlayer.PlayOneShot(audioPlayer.clip, amplitude);
        } else {
            audioPlayer.Stop();
            audioPlayer.PlayOneShot(audioPlayer.clip, amplitude);
        }

    }


    void noteAnimationA() {
        float map_size = PlayerPrefs.GetFloat("map_size") * 0.01f;
        float sizeOffset = PlayerPrefs.GetFloat("sizeOffset");

        float confX = PlayerPrefs.GetFloat("xSize") * map_size;
        float confY = PlayerPrefs.GetFloat("ySize") * map_size;

        sinVal = Mathf.Sin((playbackClock * map_size + confX) * noteIndex);
        cosVal = Mathf.Cos((playbackClock * map_size + confY) * noteIndex);

        float x_pos = (sinVal * ((noteIndex + 1) * 0.1f)) * sizeOffset;
        float z_pos = (cosVal * ((noteIndex + 1) * 0.1f)) * sizeOffset;

        transform.position = new Vector3(x_pos, 0f, z_pos);
    }


    private void OnEnable()
    {
        if (transform.GetComponent<AudioSource>())
        {
            repeatRate = minRepeatRate + (Mathf.PerlinNoise(transform.position.x, transform.position.z) * maxRepeatRate);

            InvokeRepeating("playNote", repeatRate, repeatRate);
        }
    }

}