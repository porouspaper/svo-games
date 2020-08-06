using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class FoodCollectorSettings : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public FoodCollectorArea[] listArea;

    public Text scoreText;

    public int agent0return;
    public int agent1return;


    public int agent0laser;
    public int agent1laser;

    public float equality;



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

        agent0return = 0;
        agent1return = 0;
        agent0laser = 0;
        agent1laser = 0;
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
        if ((Time.frameCount % 100)== 0)
        {
            m_Recorder.Add("CollectiveReturn", agent0return + agent1return);
            m_Recorder.Add("Agent0Return", agent0return);
            m_Recorder.Add("Agent1Return", agent1return);
            m_Recorder.Add("Agent0Laser", agent0laser);
            m_Recorder.Add("Agent1Laser", agent1laser);
            m_Recorder.Add("TotalLaser", agent0laser + agent1laser);

            int n = (int) (Time.frameCount / 100);

            equality = 1 - (agent0return - agent1return) / (2 * agent0return + agent1return);
            m_Recorder.Add("Equality", equality);
        }
    }
}
