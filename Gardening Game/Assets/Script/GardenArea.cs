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

    void CreateGoals(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(goal, new Vector3(Random.Range(-range + 4, range - 4), -0.5f,
                Random.Range(-range + 4, range - 4)) + transform.position,
                Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            GoalLogic m_GoalLogic = f.GetComponent<GoalLogic>();
            m_GoalLogic.isCooperative = true;
            m_GoalLogic.settings = this.settings;
        }

        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(goal, new Vector3(Random.Range(-range, range), -0.5f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            GoalLogic m_GoalLogic = f.GetComponent<GoalLogic>();
            m_GoalLogic.isCooperative = false;
            m_GoalLogic.settings = this.settings;

        }
    }

    public void ResetArea(GardenerAgent[] agents)
    {

        foreach (GardenerAgent agent in agents)
        {

            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(Random.Range(-range, range), 0f,
                    Random.Range(-range, range))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }

        CreateIngredient(numBall);
        CreateGoals(numGoal);

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
