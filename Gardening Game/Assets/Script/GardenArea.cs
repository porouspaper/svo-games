using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class GardenArea : MonoBehaviour
{
    public GameObject goal;
    public int numGoal; //of each type

    public GameObject ingredient;
    public int numBall;

    public Agent[] allAgents;
    public GardenScene settings;



    public float range;


    public float level;

    private void SetWalls(float halfRange)
    {
        
        GameObject Wall1 = GameObject.Find("Wall1");
        GameObject Wall2 = GameObject.Find("Wall2");
        GameObject Wall3 = GameObject.Find("Wall3");
        GameObject Wall4 = GameObject.Find("Wall4");
        Wall1.transform.localPosition = new Vector3(0, 1, -halfRange);
        Wall2.transform.localPosition = new Vector3(halfRange, 1, 0);
        Wall3.transform.localPosition = new Vector3(0, 1, halfRange);
        Wall4.transform.localPosition = new Vector3(-halfRange, 1, 0);



    }

    private void Start()
    {
        level = Academy.Instance.EnvironmentParameters.GetWithDefault("level", 0.0f);
        if (level == 1) //1 cooperative, only goal area
        {
            numGoal = 1;
            numBall = 4;
            range = 4;
            SetWalls(4.1f);

        }
        if (level == 2) //1 selfish, only goal area
        {
            numGoal = 1;
            numBall = 4;
            range = 4;
            SetWalls(4.1f);
        }
        if (level == 3) //1 cooperative and selfish and some space (learn to push and pick space)
        {
            numGoal = 1;
            numBall = 4;
            range = 8;
            SetWalls(9);
        }
        if (level == 4) //make space big (learn to push more)
        {
            numGoal = 2;
            numBall = 100;
            range = 50;
            SetWalls(50);
        }
    }


    void CreateIngredient(int num)
    {


        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(ingredient, new Vector3(Random.Range(-range, range), 0f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            IngredientType m_IngredientType = f.GetComponent<IngredientType>();
            m_IngredientType.SetType(i % 4);
            m_IngredientType.myAreaPosition = this.transform.position;
            m_IngredientType.range = this.range;
        }

    }

    void CreateGoals(int num, bool isCoop)
    {

        for (int i = 0; i < num; i++)
        {
            if (isCoop)
            {
                GameObject f = Instantiate(goal, new Vector3(Random.Range(-range + 4, range - 4), -0.5f,
                    Random.Range(4, range - 4)) + transform.position,
                    Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                GoalLogic m_GoalLogic = f.GetComponent<GoalLogic>();
                m_GoalLogic.isCooperative = isCoop;
                m_GoalLogic.settings = this.settings;
            }
            else
            {
                GameObject f = Instantiate(goal, new Vector3(Random.Range(-range + 4, range - 4), -0.5f,
                    Random.Range(-range + 4, -4)) + transform.position,
                    Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                GoalLogic m_GoalLogic = f.GetComponent<GoalLogic>();
                m_GoalLogic.isCooperative = isCoop;
                m_GoalLogic.settings = this.settings;
            }
        }
    }

    void CreateGoalMiddle(bool isCoop)
    {
        GameObject f = Instantiate(goal, new Vector3(0, -0.5f, 0) + transform.position,
            Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        GoalLogic m_GoalLogic = f.GetComponent<GoalLogic>();
        m_GoalLogic.isCooperative = isCoop;
        m_GoalLogic.settings = this.settings;
    }

    public void ResetArea(GardenerAgent[] agents)
    {

        foreach (GardenerAgent agent in agents)
        {

            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(Random.Range(-range + 0.5f, range - 0.5f), 0f,
                    Random.Range(-range + 0.5f, range - 0.5f))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }

        CreateIngredient(numBall);

        if(level == 1)
        {
            CreateGoalMiddle(true);
        }
        else if(level == 2)
        {
            CreateGoalMiddle(false);
        }
        else
        {
            CreateGoals(numGoal, true);
            CreateGoals(numGoal, false);
        }

        GameObject[] ingredients1 = GameObject.FindGameObjectsWithTag("ingredient1");
        GameObject[] ingredients2 = GameObject.FindGameObjectsWithTag("ingredient2");
        GameObject[] ingredients3 = GameObject.FindGameObjectsWithTag("ingredient3");
        GameObject[] ingredients4 = GameObject.FindGameObjectsWithTag("ingredient4");

        GameObject[] coopGoals = GameObject.FindGameObjectsWithTag("coopGoal");
        GameObject[] selfGoals = GameObject.FindGameObjectsWithTag("selfGoal");


        foreach (GardenerAgent agent in agents)
        {
            foreach (GameObject f in ingredients1)
            {
                IngredientType ing = f.GetComponent<IngredientType>();
                if (agent.myType != ing.type)
                {
                    Physics.IgnoreCollision(agent.GetComponent<Collider>(), f.GetComponent<Collider>());
                }
            }
            foreach (GameObject f in ingredients2)
            {
                IngredientType ing = f.GetComponent<IngredientType>();
                if (agent.myType != ing.type)
                {
                    Physics.IgnoreCollision(agent.GetComponent<Collider>(), f.GetComponent<Collider>());
                }
            }
            foreach (GameObject f in ingredients3)
            {
                IngredientType ing = f.GetComponent<IngredientType>();
                if (agent.myType != ing.type)
                {
                    Physics.IgnoreCollision(agent.GetComponent<Collider>(), f.GetComponent<Collider>());
                }
            }
            foreach (GameObject f in ingredients4)
            {
                IngredientType ing = f.GetComponent<IngredientType>();
                if (agent.myType != ing.type)
                {
                    Physics.IgnoreCollision(agent.GetComponent<Collider>(), f.GetComponent<Collider>());
                }
            }
            foreach (GameObject f in coopGoals)
            {
                Physics.IgnoreCollision(agent.GetComponent<Collider>(), f.GetComponent<Collider>());
            }
            foreach (GameObject f in selfGoals)
            {
                Physics.IgnoreCollision(agent.GetComponent<Collider>(), f.GetComponent<Collider>());
            }
        }
    }
}
