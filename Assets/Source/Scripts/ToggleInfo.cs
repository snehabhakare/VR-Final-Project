using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ToggleInfo : MonoBehaviour
{
    public VRTK_InteractableObject icon;
    public GameObject info;
    RunConfigurations script;
    public int num_access;
    public float totalTime;
    float lastTime;
    
    public void Init()
    {
        info.SetActive(false);
        num_access = 0;
        totalTime = 0;
        lastTime = 0;
    }

    public void Write() {
        if(info.activeSelf) totalTime += (script.TimeElapsed - lastTime);
        script.writer3.WriteLine(icon.name + "," + totalTime.ToString() + "," + num_access);
    }

    void Start() {
        GameObject next = GameObject.Find("Next");
        script = next.GetComponent<RunConfigurations>();
        Init();
    }

    void OnEnable()
    {
        icon = (icon == null ? GetComponent<VRTK_InteractableObject>() : icon);
        if (icon != null)
        {
            icon.InteractableObjectUsed += InteractableObjectUsed;
            icon.InteractableObjectUnused += InteractableObjectUnused;
        }
    }

    void OnDisable()
    {
        if (icon != null)
        {
            icon.InteractableObjectUsed -= InteractableObjectUsed;
            icon.InteractableObjectUnused -= InteractableObjectUnused;
        }
    }

    void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        if (info.activeSelf)
        {
            totalTime += (script.TimeElapsed - lastTime);
            info.SetActive(false);
            script.writer1.WriteLine(script.TimeElapsed.ToString() + " : Deactivate " + info.name);
        }
        else {
            num_access++;
            lastTime = script.TimeElapsed;
            info.SetActive(true);
            script.writer1.WriteLine(script.TimeElapsed.ToString() + " : Activate " + info.name);
        }
    }

    void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        if (info.activeSelf)
        {
            totalTime += (script.TimeElapsed - lastTime);
            info.SetActive(false);
            script.writer1.WriteLine(script.TimeElapsed.ToString() + " : Deactivate " + info.name);
        }
        else
        {
            num_access++;
            lastTime = script.TimeElapsed;
            info.SetActive(true);
            script.writer1.WriteLine(script.TimeElapsed.ToString() + " : Activate " + info.name);
        }
    }
}
