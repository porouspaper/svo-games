using UnityEngine;
using System.Collections.Generic;
using System;

public class FoodCollectorArea : MonoBehaviour
{
    public GameObject food;
    public int numFood;
    public float range;
    public float radius;
    public float[] probabilities;
    private float timeElapsed = 0;
    private float timeNeeded = 3;

    void CreateFood(int num, GameObject type)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, new Vector3(UnityEngine.Random.Range(-range, range), 1f,
                UnityEngine.Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 90f)));
            f.GetComponent<FoodLogic>().myArea = this;
        }
    }

    void CreateFoodLocal(Vector3 loc)
    {
        GameObject f = Instantiate(food, loc,
        Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 90f)));
        f.GetComponent<FoodLogic>().myArea = this;
    }

    public void ResetFoodArea(GameObject[] agents)
    {
        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(UnityEngine.Random.Range(-range, range), 2f,
                    UnityEngine.Random.Range(-range, range))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0, 360)));
            }
        }

        CreateFood(numFood, food);

    }

    private void FixedUpdate()
    {
        if (Time.time > timeElapsed)
        {
            //print("updating");
            GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
            Dictionary<Vector3, int> map = new Dictionary<Vector3, int>();
            foreach (GameObject food in foods)
            {
                //print("food loop");
                Vector3 pos = food.transform.position;
                for (float i = -radius; i <= radius; i += 0.2f)
                {
                    for (float j = -radius; j <= radius; j += 0.2f)
                    {
                        //print("checking radius loop");
                        Vector3 pt = new Vector3(i, 0, j) + pos;
                        float dist = Vector3.Distance(pos, pt);
                        if (dist < radius)
                        {
                            print("putting in");
                            if (map.ContainsKey(pt))
                            {
                                int val = map[pt];
                                map[pt] = val + 1;
                            }
                            else
                            {
                                map.Add(pt, 1);
                            }
                        }
                    }
                }
            }

            print(map.Count);
            foreach (Vector3 pos in map.Keys)
            {
                int myLen = probabilities.Length;
                float p = probabilities[Math.Min(map[pos], myLen) - 1];
                var random = new System.Random();
                float gen_p = (float)random.NextDouble();
                if (gen_p < p)
                {
                    print("creating food");
                    CreateFoodLocal(pos);
                }

            }
            timeElapsed = timeNeeded + timeElapsed;

        }
    }

}
