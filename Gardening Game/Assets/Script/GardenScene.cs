using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;


public class GardenScene : MonoBehaviour
{
    public GardenerAgent[] agents;
    public GardenArea[] listArea;
    public bool schellingCoop;
    public bool allSelfish;
    public bool allCooperative;
    public bool allCompetitive;
    StatsRecorder m_Recorder;
    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        m_Recorder = Academy.Instance.StatsRecorder;

    }

    void EnvironmentReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("food"));


        foreach(GardenerAgent agent in agents)
        {
            if (allSelfish)
            {
                agent.svoDegrees = 0f;
            }
            else if (allCooperative)
            {
                agent.svoDegrees = 45f;
            }
            else if (allCompetitive)
            {
                agent.svoDegrees = 315f;
            }
        }
        listArea = FindObjectsOfType<GardenArea>();
        foreach (var fa in listArea)
        {
            fa.ResetArea(agents);
        }
    }

    void ClearObjects(GameObject[] objects)
    {
        foreach (var food in objects)
        {
            Destroy(food);
        }
    }

    public void Update()
    {
        
    }
}