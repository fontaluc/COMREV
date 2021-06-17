using FinerGames.PitchDetector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public PitchDetector detector;
    float curTime;
    float startTime = 0;
    private float firstPitch;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.startColor = Color.red; //The color at the beginning of the polyline
        lineRenderer.endColor = Color.red; //The color at the end of discount
        lineRenderer.startWidth = 3f; //The width at the beginning of the polyline
        lineRenderer.endWidth = 3f; //The width at the end of the polyline
        lineRenderer.useWorldSpace = false; //Use world coordinates or local coordinates

    }


    // What needs to be noted here is that before using this method, you must ensure that there is at least one straight line in front, that is to say, two points have been set, otherwise
    // The system will default the position of the first two points to Vector3.zero
    public void AddPosition(Vector3 v3)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, v3);
    }

    public void Reset()
    {
        lineRenderer.positionCount = 0;
        startTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (detector.Pitch > 1.0f)
        {
            curTime = Time.time;
            if (startTime == 0)
            {
                startTime = curTime;
                firstPitch = detector.Pitch;
            }
            
            AddPosition(new Vector3((curTime - startTime) * 100, detector.Pitch - firstPitch));
            Debug.Log(curTime);
            Debug.Log(detector.Pitch);
        }
    }
}
