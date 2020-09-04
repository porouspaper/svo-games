using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System;

public class GardenScene : MonoBehaviour
{
    public GardenerAgent[] agents;
    public GardenArea[] listArea;
    public bool schellingCoop;
    public bool allSelfish;
    public bool allCooperative;
    public bool allCompetitive;
    StatsRecorder m_Recorder;


    public int[] publicContribution;
    public int[] privateContribution;
    public float[] collectiveReturn;
    public int foodCount;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        m_Recorder = Academy.Instance.StatsRecorder;
    }

    void EnvironmentReset()
    {
        publicContribution = new int[agents.Length];
        privateContribution = new int[agents.Length];
        collectiveReturn = new float[agents.Length];
        foodCount = 0;


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
        if ((Time.frameCount % 100) == 0)
        {
            int sumPublicGood = 0;
            for(int i =0; i < agents.Length; i++)
            {
                m_Recorder.Add("PublicGood/Agent" + i.ToString(), publicContribution[i]);
                sumPublicGood += publicContribution[i];
            }
            m_Recorder.Add("PublicGood/Average", sumPublicGood / agents.Length);

            int sumPrivateGood = 0;
            for (int i = 0; i < agents.Length; i++)
            {
                m_Recorder.Add("PrivateGood/Agent" + i.ToString(), privateContribution[i]);
                sumPrivateGood += privateContribution[i];
            }
            m_Recorder.Add("PrivateGood/Average", sumPrivateGood / agents.Length);

            float sumCollectiveReturn = 0;
            for (int i = 0; i < agents.Length; i++)
            {
                m_Recorder.Add("CollectiveReturn/Agent" + i.ToString(), collectiveReturn[i]);
                sumCollectiveReturn += collectiveReturn[i];
            }
            m_Recorder.Add("CollectiveReturn/Average", sumCollectiveReturn / agents.Length);

            double equality;
            if (sumCollectiveReturn > 0)
            {
                float sumDifferences = 0;
                for (int i = 0; i < agents.Length; i++)
                {
                    for (int j = 0; j < agents.Length; j++)
                    {
                        sumDifferences += Math.Abs(collectiveReturn[i] - collectiveReturn[j]);
                    }
                }
                equality = 1 - (sumDifferences / (2 * agents.Length * sumCollectiveReturn));
            }
            else
            {
                equality = 1;
            }
            m_Recorder.Add("Equality", (float) equality);

        }

    }
}