using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;


public class GardenScene : MonoBehaviour
{
    public GameObject[] agents;
    public GardenArea[] listArea;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        //add recording stats?
    }

    void EnvironmentReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("Food"));

        agents = GameObject.FindGameObjectsWithTag("Agent");
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
}