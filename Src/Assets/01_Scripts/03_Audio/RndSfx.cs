using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


namespace MyriadeM
{
    public class RndSfx : MonoBehaviour
    {
        [Space(5)]

        public GameObject settingsObj;

        private float maxVolume = 0.25f;
        public float repeatRate = 1;

        [Space(5)]

        public bool randomRepeatRate = true;
        public float minRepeatRate = 0.5f;
        public float maxRepeatRate = 1;

        [Space(5)]

        public bool randomPitch = true;
        public float minPitch = 1f;
        public float maxPitch = 2f;

        [Space(5)]

        public bool indicateState = true;
        public bool randomColor = true;

        public bool nameColor = false;
        public List<string> clipFolders = new List<string>();

        private List<AudioClip> aClips = new List<AudioClip>();

        private AudioClip currentClip;
        private Vector3 objSize;
        private Color objColor;

        public int objNoiseIndex;
        private float noteColor;
        public int playCount = 0;

        public float minAmp = 0.05f;
        //private float ampVolMulti = 20f;

        public bool showTrail = false;

        //private float playbackClock = 0;
        //float sinVal = 0;
        //float cosVal = 0;

        private bool locked;
        public AudioSource audioPlayer;

        private Color activeColor    = new Color(0.25f, 0.25f, 0.25f, 1f);
        private Color nonActiveColor = new Color(0.10f, 0.00f, 0.40f, 1f);

        private float posNoise = 1;
        private float pitchNoise = 1;
        private float repeatNoise = 1;

        public float trailR = 0;
        public float trailG = 0;
        public float trailB = 0;
        //float timeSinceHit  = 0;

        private string clipName;

        [HideInInspector]
        public bool isInit = false;
        public bool isActive = false;

        float timeSinceHit = 0;
        public Color currentColor = new Color(1f, 1f, 1f, 1f);
        private bool noiseActive = false;


        void InitContent(GameObject rootObj, List<string> gameObjectPaths)
        {
            foreach (string path in gameObjectPaths)
            {
                if (path != "")
                {
                    AudioClip[] prefabFiles = Resources.LoadAll<AudioClip>(path);
                    foreach (AudioClip a in prefabFiles)
                    {
                        aClips.Add(a);
                    }
                }
            }

            isInit = true;
        }


        void PlaySample()
        {
            Debug.Log("SeqMode: " + PlayerPrefs.GetInt("seqMode").ToString());

            playCount += 1;
            timeSinceHit = 0;
            currentColor = new Color(1, 1, 1, 1);

            int currentFolder = settingsObj.GetComponent<ControlPanel>().sampleDropDown.value;
            List<AudioClip> sampleList = settingsObj.GetComponent<ControlPanel>().sampleFolderDict[currentFolder];
            if (sampleList.Count == 0) { return; }           
            int samIdx = 0;

            if (PlayerPrefs.GetInt("seqMode") == 0)
            {
                samIdx = (int)(posNoise * (sampleList.Count - 1));
            }

            if (PlayerPrefs.GetInt("seqMode") == 1)
            {
                samIdx = (int)(Mathf.Abs(Mathf.Sin(posNoise)) * (sampleList.Count - 1));
            }

            if (PlayerPrefs.GetInt("seqMode") == 2)
            {
                float si = Mathf.PI / sampleList.Count;
                samIdx = (int)(Mathf.Abs(Mathf.Sin(si * (playCount * (objNoiseIndex)))) * sampleList.Count);
            }

            if (PlayerPrefs.GetInt("seqMode") == 3)
            {
                samIdx = (int)Random.Range(0, sampleList.Count - 1);
            }

            if (audioPlayer.clip != sampleList[samIdx])
            {
                currentClip = sampleList[samIdx];
            }
            

            if (transform.GetComponent<AudioSource>()) {
                audioPlayer.Stop();
                audioPlayer.enabled = true;
                audioPlayer.mute = false;
                audioPlayer.ignoreListenerPause = true;
                audioPlayer.ignoreListenerVolume = true;
                audioPlayer.priority = 0;
                audioPlayer.volume = maxVolume;

                audioPlayer.PlayOneShot(currentClip);
            }
        }


        IEnumerator MainFunc()
        {
            yield return new WaitForSeconds(repeatRate);

            minPitch = PlayerPrefs.GetFloat("slider_b");
            maxPitch = PlayerPrefs.GetFloat("slider_c");
            minRepeatRate = PlayerPrefs.GetFloat("slider_d");
            maxRepeatRate = PlayerPrefs.GetFloat("slider_e");

            repeatRate = minRepeatRate + Mathf.Abs((repeatNoise * maxRepeatRate));

            //yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            while (true)
            {
                transform.GetComponent<AudioSource>().volume = maxVolume;

                minPitch = PlayerPrefs.GetFloat("slider_b");
                maxPitch = PlayerPrefs.GetFloat("slider_c");
                minRepeatRate = PlayerPrefs.GetFloat("slider_d");
                maxRepeatRate = PlayerPrefs.GetFloat("slider_e");

                float new_p = minPitch + Mathf.Abs(pitchNoise * maxPitch);

                if (PlayerPrefs.GetInt("pitchMode") == 1) {
                    float sinP = Mathf.PI / 12;
                    new_p = minPitch + Mathf.Abs(Mathf.Sin(playCount * sinP) * maxPitch);
                }

                if (PlayerPrefs.GetInt("pitchMode") == 2) {
                    new_p = Random.Range((float)minPitch, (float)maxPitch);
                }

                transform.gameObject.GetComponent<AudioSource>().pitch = new_p;

                repeatRate = minRepeatRate + Mathf.Abs((repeatNoise * maxRepeatRate));

                if (!noiseActive) {
                    transform.GetComponent<Renderer>().material.color = nonActiveColor;
                }
                else {
                    if (PlayerPrefs.GetInt("play_audio") == 1) {
                        PlaySample();
                    }
                }

                yield return new WaitForSeconds(repeatRate);
            }
        }


        private void Update()
        {
            noiseActive = (PlayerPrefs.GetFloat("slider_a") > (posNoise * 100));

            if (transform.GetComponent<AudioSource>().isPlaying && noiseActive)
            {
                timeSinceHit += 0.05f / repeatRate;
                //float clr = 1f - timeSinceHit;

                if (timeSinceHit <= 1f) {
                    currentColor = new Color(0, 1 - timeSinceHit, 2 - timeSinceHit, 1);
                } else {
                    currentColor = nonActiveColor;
                }

            } else {
                currentColor = nonActiveColor;
            }

            transform.GetComponent<Renderer>().material.color = currentColor;
        }


        private void OnEnable()
        {
            //int year = System.DateTime.Now.Year;
            //int day = System.DateTime.Now.Day;
            //int hour = System.DateTime.Now.Hour;
            //int minute = System.DateTime.Now.Minute;
            //int sec = System.DateTime.Now.Second;

            playCount = 0;

            if (transform.parent.GetComponent<MyriadeE.TileContentManager>() != null) {
                objNoiseIndex = transform.parent.GetComponent<MyriadeE.TileContentManager>().indexName;
            }

            if (transform.parent)
            {
                float xps = transform.parent.parent.localPosition.x;
                float zps = transform.parent.parent.localPosition.z;
                posNoise = Mathf.PerlinNoise((zps * 1.123456789f), (xps * 1.123456789f));

                transform.GetComponent<Renderer>().material.color = nonActiveColor;

                if (!settingsObj) {
                    settingsObj = transform.parent.gameObject.GetComponent<MyriadeE.TileContentManager>().settingsObject;
                }

                // float val = settingsObj.GetComponent<MyriadeE.s;

                if (!transform.GetComponent<AudioSource>()) {
                    transform.gameObject.AddComponent<AudioSource>();
                }
                audioPlayer = transform.GetComponent<AudioSource>();
                audioPlayer.enabled = true;

                int currentFolder = settingsObj.GetComponent<ControlPanel>().sampleDropDown.value;
                //Debug.log(currentFolder);
                List<AudioClip> sampleList = settingsObj.GetComponent<ControlPanel>().sampleFolderDict[1];

                if (sampleList.Count == 0) { return; }
                repeatNoise = Mathf.PerlinNoise((xps * 1.123456789f), (zps * 1.123456789f));
                pitchNoise  = Mathf.PerlinNoise((xps * 1.123456789f), (zps * 1.123456789f));

                int samIdx = 0;

                if (PlayerPrefs.GetInt("seqMode") == 0) {
                    samIdx = (int)(posNoise * (sampleList.Count - 1));
                }
                else {
                    samIdx = (int)Random.Range(0, sampleList.Count - 1);
                }

                if (sampleList.Count > samIdx) {
                    if (audioPlayer.clip != sampleList[samIdx])
                    {
                        currentClip = sampleList[samIdx];
                    }
                }

                float p = minPitch + (pitchNoise * maxPitch);
                transform.gameObject.GetComponent<AudioSource>().pitch = p;

                if (transform.GetComponent<Renderer>())
                {
                    objColor = nonActiveColor;
                }

                activeColor = new Color(
                       Random.Range(0.0f, 1.0f),
                       Random.Range(0.0f, 1.0f),
                       Random.Range(0.0f, 1.0f)
                   );

                //if (nameColor)
                //{
                //    string sName = currentClip.name;
                //    int nameNum = 1;

                //    foreach (char c in sName)
                //        nameNum += ((byte)c);

                //    float r = Mathf.Abs(Mathf.Cos(samIdx * nameNum * (0.01f * Mathf.PI)));
                //    float g = Mathf.Abs(Mathf.Sin(samIdx * nameNum * (0.01f * Mathf.PI)));
                //    float b = Mathf.Abs(Mathf.Cos((-samIdx * nameNum) * (0.01f * Mathf.PI)));
                //    activeColor = new Color(r, g, b);
                //    Debug.Log(activeColor);
                //}

                isInit = true;
                isActive = true;
                StartCoroutine(MainFunc());
            } else {
                transform.gameObject.SetActive(false);
            }

        }


        private void OnDisable()
        {
            CancelInvoke();
            isInit = false;
            isActive = false;
        }


    }
}