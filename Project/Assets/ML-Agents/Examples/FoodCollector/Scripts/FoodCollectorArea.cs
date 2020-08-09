using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class FoodCollectorArea : MonoBehaviour
{
    public GameObject food;
    public int numFood;
    public float range;
    public float radius;
    public float[] probabilities;
    private float timeElapsed = 0;
    public float timeNeeded = 1;

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


    public static int CompareLoc(Vector3 v1, Vector3 v2)
    {
        if(v1.x > v2.x)
        {
            return 1;
        }
        else if(v1.x < v2.x)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time > timeElapsed)
        {
            //print("updating");
            GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
            //sort by x
            //for all x within 2
            //if 2, spawn with prob 2, etc.

            Func<GameObject, Vector3> getLoc = (x) => x.transform.position;
            Vector3[] locs = new Vector3[foods.Length];
            for(int i = 0; i < foods.Length; i++)
            {
                locs[i] = getLoc(foods[i]);
            }

            Array.Sort(locs, CompareLoc);

            Vector3 prev = Vector3.zero;
            for(int i = 0; i < locs.Length; i++)
            {
                if (i > 0 && Vector3.Distance(prev, locs[i]) > radius)
                {
                    prev = locs[i];
                    int myLen = probabilities.Length;
                    int count = 1;
                    for (int j = i - myLen; j <= i + myLen; j++)
                    {
                        if (j >= 0 && j < locs.Length)
                        {
                            if (Vector3.Distance(locs[i], locs[j]) < radius)
                            {
                                //print("Distance good");
                                count += 1;
                            }
                        }
                    }

                    count = Math.Min(count, myLen);

                    float p = probabilities[count - 1];
                    var random = new System.Random();
                    float gen_p = (float)random.NextDouble();
                    if (gen_p < p)
                    {
                        //print("creating food");
                        Vector3 offset = new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, UnityEngine.Random.Range(-2f, 2f));
                        CreateFoodLocal(locs[i] + offset);
                    }
                }
            }

            


            timeElapsed = timeNeeded + timeElapsed;

        }
    }

}
