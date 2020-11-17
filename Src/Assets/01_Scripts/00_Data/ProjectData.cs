using UnityEngine;
using System.IO;
using System.Collections;

[System.Serializable]
public class ProjectData
{
    public string projectName = "";
    public string projectId = "";
    public string appVersion = "";

    public bool visMode = false;

    public string selectedSample = "";

    public float playbackDirection = 1;
    public string playbackPosition = "0";

    public float xPos = 0;
    public float yPos = 0;
    public float zPos = 0;

    public float xRot = 0;
    public float zRot = 0;

    public int patternSize = 48;
    public int playMode = 1;
    public int seqMode = 1;
    public int pitchMode = 1;

    public float sliderA = 1;
    public float sliderB = 1;
    public float sliderC = 1;
    public float sliderD = 1;
    public float sliderE = 80;
    public float sliderF = 1;
    public float sliderG = 0;
    public float sliderH = 0;

    public string importPath = "";

}