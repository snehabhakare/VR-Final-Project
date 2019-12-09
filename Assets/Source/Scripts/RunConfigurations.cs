using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

public class RunConfigurations : MonoBehaviour
{
    int index;
    int num_config;
    VRTK_InteractableObject next;
    List<GameObject> Table1_relevant = new List<GameObject>();
    List<GameObject> Table1_irrelevant = new List<GameObject>();
    List<GameObject> Table2_relevant = new List<GameObject>();
    List<GameObject> Table2_irrelevant = new List<GameObject>();
    GameObject Table1_marker;
    GameObject Table2_marker;
    int[] configs = { 0, 1, 2, 3 };
    string path1;
    string path2;
    string path3;
    public StreamWriter writer1;
    public StreamWriter writer2;
    public StreamWriter writer3;
    public float TimeElapsed;
    float start;
    float timeInterval = 1.0f;
    float currentTime = 0.0f;
    
    void FetchIcons()
    {
        GameObject Table1_icons = GameObject.Find("/Table1/Terrain/Icons");
        GameObject Table2_icons = GameObject.Find("/Table2/Terrain/Icons");
        List<GameObject> result = new List<GameObject>();
        Component[] children1 = Table1_icons.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children1)
        {
            if (child.name == "Crates" || child.name == "Money" || child.name == "Routes" || child.name == "Government")
            {
                Table1_relevant.Add(child.gameObject);
            }
            else if (child.name == "Park" || child.name == "Traffic" || child.name == "Flood" || child.name == "Religion")
            {
                Table1_irrelevant.Add(child.gameObject);
            }
        }
        Component[] children2 = Table2_icons.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children2)
        {
            if (child.name == "Crates" || child.name == "Money" || child.name == "Routes" || child.name == "Government")
            {
                Table2_relevant.Add(child.gameObject);
            }
            else if (child.name == "Park" || child.name == "Traffic" || child.name == "Flood" || child.name == "Religion")
            {
                Table2_irrelevant.Add(child.gameObject);
            }
        }
    }

    void FetchMarkers()
    {
        Table1_marker = GameObject.Find("/Table1/Terrain/Marker");
        Table2_marker = GameObject.Find("/Table2/Terrain/Marker");
    }

    void ResetIcons() {
        start = TimeElapsed;
        writer1.WriteLine(TimeElapsed.ToString() + " : Start config " + configs[index]);
        
        foreach (GameObject icon in Table1_relevant) icon.GetComponent<ToggleInfo>().Init();
        foreach (GameObject icon in Table1_irrelevant) icon.GetComponent<ToggleInfo>().Init();
        foreach (GameObject icon in Table2_relevant) icon.GetComponent<ToggleInfo>().Init();
        foreach (GameObject icon in Table2_irrelevant) icon.GetComponent<ToggleInfo>().Init();

        switch (configs[index])
        {
            case 0: // Relevant:Table2; Irrelevant:Table1
                foreach (GameObject icon in Table1_relevant) icon.SetActive(false);
                foreach (GameObject icon in Table1_irrelevant) icon.SetActive(true);
                foreach (GameObject icon in Table2_relevant) icon.SetActive(true);
                foreach (GameObject icon in Table2_irrelevant) icon.SetActive(false);
                break;
            case 1: // Relevant:Table2; Irrelevant:Table2
                foreach (GameObject icon in Table1_relevant) icon.SetActive(false);
                foreach (GameObject icon in Table1_irrelevant) icon.SetActive(false);
                foreach (GameObject icon in Table2_relevant) icon.SetActive(true);
                foreach (GameObject icon in Table2_irrelevant) icon.SetActive(true);
                break;
            case 2: // Relevant:Table1; Irrelevant:Table2
                foreach (GameObject icon in Table1_relevant) icon.SetActive(true);
                foreach (GameObject icon in Table1_irrelevant) icon.SetActive(false);
                foreach (GameObject icon in Table2_relevant) icon.SetActive(false);
                foreach (GameObject icon in Table2_irrelevant) icon.SetActive(true);
                break;
            case 3: // Relevant:Table1; Irrelevant:Table1
                foreach (GameObject icon in Table1_relevant) icon.SetActive(true);
                foreach (GameObject icon in Table1_irrelevant) icon.SetActive(true);
                foreach (GameObject icon in Table2_relevant) icon.SetActive(false);
                foreach (GameObject icon in Table2_irrelevant) icon.SetActive(false);
                break;
        }
    }

    void ResetMarkers() {
        writer1.WriteLine(TimeElapsed.ToString() + " : Predicted Location Table1 "+ (Table1_marker.transform.localPosition.x).ToString() +  " " + (Table1_marker.transform.localPosition.z).ToString());
        writer1.WriteLine(TimeElapsed.ToString() + " : Predicted Location Table2 " + (Table2_marker.transform.localPosition.x).ToString() +  " " + (Table2_marker.transform.localPosition.z).ToString());
        writer1.WriteLine(TimeElapsed.ToString() + " : End config " + configs[index]);
        WriteData();
        GameObject.Find("/Next/Canvas/Text").GetComponent<Text>().text = "Finalize the marker.";
        Table1_marker.GetComponent<FinalMarker1>().Init();
        Table2_marker.GetComponent<FinalMarker2>().Init();
    }

    void WriteData() {
        writer3.WriteLine("Configuration" + configs[index].ToString() + "," + (TimeElapsed-start).ToString());
        switch (configs[index])
        {
            case 0: // Relevant:Table2; Irrelevant:Table1
                foreach (GameObject icon in Table2_relevant) icon.GetComponent<ToggleInfo>().Write();
                foreach (GameObject icon in Table1_irrelevant) icon.GetComponent<ToggleInfo>().Write();
                break;
            case 1: // Relevant:Table2; Irrelevant:Table2
                foreach (GameObject icon in Table2_relevant) icon.GetComponent<ToggleInfo>().Write();
                foreach (GameObject icon in Table2_irrelevant) icon.GetComponent<ToggleInfo>().Write();
                break;
            case 2: // Relevant:Table1; Irrelevant:Table2
                foreach (GameObject icon in Table1_relevant) icon.GetComponent<ToggleInfo>().Write();
                foreach (GameObject icon in Table2_irrelevant) icon.GetComponent<ToggleInfo>().Write();
                break;
            case 3: // Relevant:Table1; Irrelevant:Table1
                foreach (GameObject icon in Table1_relevant) icon.GetComponent<ToggleInfo>().Write();
                foreach (GameObject icon in Table1_irrelevant) icon.GetComponent<ToggleInfo>().Write();
                break;
        }
        Table1_marker.GetComponent<FinalMarker1>().Write();
        Table2_marker.GetComponent<FinalMarker2>().Write();
        writer3.WriteLine();
    }

    void Start()
    {
        // rearrange configs based on the ID
        int ID = 19;
        index = ID%4;
        num_config = 0;
        path1 = "Assets/Source/Logs/log_"+ID+".txt";
        path2 = "Assets/Source/Logs/displacement_" + ID + ".txt";
        path3 = "Assets/Source/Logs/data_" + ID + ".csv";
        TimeElapsed = 0;
        currentTime = 0;
        writer1 = new StreamWriter(path1, true);
        writer2 = new StreamWriter(path2, true);
        writer3 = new StreamWriter(path3, true);
        FetchMarkers();
        FetchIcons();
        ResetIcons();
    }

    void Update()
    {
        TimeElapsed += Time.deltaTime;
        if ((TimeElapsed - currentTime) > 0) {
            Vector3 p = Camera.current.gameObject.transform.position;
            writer2.WriteLine(currentTime.ToString() + ": " + p);
            currentTime++;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnEnable()
    {
        next = this.gameObject.GetComponent<VRTK_InteractableObject>();
        next.InteractableObjectUsed += InteractableObjectUsed;
        next.InteractableObjectUnused += InteractableObjectUnused;
    }

    void OnDisable()
    {
        next.InteractableObjectUsed -= InteractableObjectUsed;
        next.InteractableObjectUnused -= InteractableObjectUnused;
    }

    void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        GameObject.Find("/Next/Canvas/Text").GetComponent<Text>().text = "Are you sure?";
    }

    void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        ResetMarkers();
        index = (index+1)%4;
        num_config++;
        if (num_config < 4) {
            ResetIcons();
        }
        else
        {
            GameObject.Find("/Table1").SetActive(false);
            GameObject.Find("/Table2").SetActive(false);
            this.gameObject.SetActive(false);
            writer1.Close();
            writer2.Close();
            writer3.Close();
        }
    }
}