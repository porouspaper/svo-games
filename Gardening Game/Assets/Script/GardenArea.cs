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
            m_IngredientType.respawn = true;
            m_IngredientType.myArea = this;
        }
    }

    void CreateGoals(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(goal, new Vector3(Random.Range(-range, range), -0.5f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            GoalLogic m_GoalLogic = f.GetComponent<GoalLogic>();
            m_GoalLogic.isCooperative = true;
        }

        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(goal, new Vector3(Random.Range(-range, range), -0.5f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            GoalLogic m_GoalLogic = f.GetComponent<GoalLogic>();
            m_GoalLogic.isCooperative = false;
        }
    }

    public void ResetArea(GameObject[] agents)
    {
        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(Random.Range(-range, range), 2f,
                    Random.Range(-range, range))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }


        CreateIngredient(numBall);
        CreateGoals(numGoal);

        GameObject[] ingredients = GameObject.FindGameObjectsWithTag("ingredient");
        GameObject[] goals = GameObject.FindGameObjectsWithTag("goal");

        foreach (GameObject agent in agents)
        {
            foreach (GameObject f in ingredients)
            {
                GardenerAgent myagent = agent.GetComponent<GardenerAgent>();
                IngredientType ing = f.GetComponent<IngredientType>();
                if (myagent.myType != ing.type)
                {
                    Physics.IgnoreCollision(agent.GetComponent<Collider>(), f.GetComponent<Collider>());
                }
            }
            foreach(GameObject f in goals)
            {
                Physics.IgnoreCollision(agent.GetComponent<Collider>(), f.GetComponent<Collider>());
            }
        }
    }
}
