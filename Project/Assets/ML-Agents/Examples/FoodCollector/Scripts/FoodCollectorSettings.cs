using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using System;

public class FoodCollectorSettings : MonoBehaviour
{
    public GameObject[] agents;
    public FoodCollectorArea[] listArea;

    public int svoDegrees;

    public Agent[] rewardAgents;

    public Text scoreText;


    public int[] agentReturns;


    public int[] agentLasers;

    public float equality;

    public int[] applesEaten;
    public int totalApples;





    StatsRecorder m_Recorder;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        m_Recorder = Academy.Instance.StatsRecorder;
    }

    void EnvironmentReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("food"));

        agents = GameObject.FindGameObjectsWithTag("agent");
        listArea = FindObjectsOfType<FoodCollectorArea>();
        foreach (var fa in listArea)
        {
            fa.ResetFoodArea(agents);
        }

        agentReturns = new int[rewardAgents.Length];
        agentLasers = new int[rewardAgents.Length];
        applesEaten = new int[rewardAgents.Length];
        equality = 0;


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

        // Send stats via SideChannel so that they'll appear in TensorBoard.
        // These values get averaged every summary_frequency steps, so we don't
        // need to send every Update() call.
        if ((Time.frameCount % 100) == 0)
        {
            int collectiveReturn = 0;
            int collectiveLaser = 0;
            int collectiveEaten = 0;

            for (int i = 0; i < agentReturns.Length; i++)
            {
                collectiveReturn += agentReturns[i];
                m_Recorder.Add("Agent" + i.ToString() + "Return", agentReturns[i]);
            }


            for (int i = 0; i < agentLasers.Length; i++)
            {
                collectiveLaser += agentLasers[i];
                m_Recorder.Add("Agent" + i.ToString() + "Laser", agentLasers[i]);
            }

            for (int i = 0; i < applesEaten.Length; i++)
            {
                collectiveEaten += applesEaten[i];
                m_Recorder.Add("Agent" + i.ToString() + "AppleEaten", applesEaten[i]);
            }

            m_Recorder.Add("CollectiveReturn", collectiveReturn);
            m_Recorder.Add("TotalLaser", collectiveLaser);
            m_Recorder.Add("TotalEaten", collectiveEaten);

            if (collectiveReturn > 0)
            {
                int sumDifferences = 0;
                for(int i = 0; i < agentReturns.Length; i++)
                {
                    for (int j = 0; j < agentReturns.Length; j++)
                    {
                        sumDifferences += Math.Abs(agentReturns[i] - agentReturns[j]);
                    }
                }
                equality = 1 - (sumDifferences / (2 * agentReturns.Length * collectiveReturn ));
            }
            else
            {
                equality = 1;
            }
            m_Recorder.Add("Equality", equality);
            m_Recorder.Add("TotalApples", totalApples);
        }
    }

}
