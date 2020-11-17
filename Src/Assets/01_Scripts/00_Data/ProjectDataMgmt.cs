
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class ProjectDataMgmt : MonoBehaviour
{


    public static ProjectData parseProjectFile(string jsonString)
    {
        return JsonUtility.FromJson<ProjectData>(jsonString);
    }

    public void LoadProject() {
        transform.GetComponent<ControlPanel>().lockData = true;
        transform.GetComponent<ControlPanel>().lockData = false;
    }

    public void SaveProject(ProjectData saveData)
    {
        string savePanelMessage = "Save Project";
        string[] fileExts = new string[] { "txt", "json" };
        string initFname = "Project.json";
        BinaryFormatter bf = new BinaryFormatter();
    }

    public void OnOpenFileButtonClicked()
    {

    }

    public void OnOpenMultipleFilesButtonClicked()
    {

    }

    public void ImportSampleFolder()
    {

    }

    public void OnOpenMultipleFoldersButtonClicked()
    {

    }

    public void OnSaveFileButtonClicked()
    {
        string savePanelMessage = "Save Project";
        string[] fileExts = new string[] { "txt", "json" };
        string initFname = "Project.txt";


    }


    public void OnLoadImageButtonClicked()
    {

    }

    public void OnLoadFileButtonClicked()
    {

    }

    public void OnSaveTextButtonClicked()
    {
   
    }

    public void OnSaveScreenshotButtonClicked()
    {

    }

    public void OnListFilesInDirectoriesButtonClicked()
    {
  
    }
}
