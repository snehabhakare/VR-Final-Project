using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class FinalMarker2 : MonoBehaviour
{
    RunConfigurations script;
    VRTK_InteractableObject marker;
    bool first;
    float start;

    public void Init()
    {
        first = true;
        start = 0;
        transform.localPosition = new Vector3(2, 0.5f, 0.5f);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void Write()
    {
        script.writer3.WriteLine("Table2 Marker," + (script.TimeElapsed - start).ToString() + "," + (transform.localPosition.x).ToString() + "," + (transform.localPosition.z).ToString());
    }

    void Start()
    {
        marker = GetComponent<VRTK_InteractableObject>();
        GameObject next = GameObject.Find("Next");
        script = next.GetComponent<RunConfigurations>();
        Init();
    }

    void OnEnable()
    {
        marker = (marker == null ? GetComponent<VRTK_InteractableObject>() : marker);
        if (marker != null)
        {
            marker.InteractableObjectGrabbed += InteractableObjectGrabbed;
            marker.InteractableObjectUngrabbed += InteractableObjectUngrabbed;
        }
    }

    void OnDisable()
    {
        if (marker != null)
        {
            marker.InteractableObjectGrabbed += InteractableObjectGrabbed;
            marker.InteractableObjectUngrabbed += InteractableObjectUngrabbed;
        }
    }

    void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (first)
        {
            start = script.TimeElapsed;
            script.writer1.WriteLine(script.TimeElapsed.ToString() + " : Grabbed Table2 marker");
            first = false;
        }
    }

    void InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {

    }
}
