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
    public float timeNeeded = 2;
    public FoodCollectorSettings m_FoodCollecterSettings;

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
        m_FoodCollecterSettings.totalApples += 1;
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
        m_FoodCollecterSettings.totalApples = numFood;

    }


    public static int CompareLocX(Vector3 v1, Vector3 v2)
    {
        if (v1.x > v2.x)
        {
            return 1;
        }
        else if (v1.x < v2.x)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public static int CompareFoodX(GameObject v1, GameObject v2)
    {
        if (v1.transform.position.x > v2.transform.position.x)
        {
            return 1;
        }
        else if (v1.transform.position.x < v2.transform.position.x)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public static int CompareLocY(Vector3 v1, Vector3 v2)
    {
        if (v1.z > v2.z)
        {
            return 1;
        }
        else if (v1.z < v2.z)
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
        //print("updating");

        if (Time.time > timeElapsed)
        {
            GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
            if (foods.Length < 1000)
            {

                Func<GameObject, Vector3> getLoc = (x) => x.transform.position;

                Vector3[] locsX = new Vector3[foods.Length];
                Vector3[] locsY = new Vector3[foods.Length];

                for (int i = 0; i < foods.Length; i++)
                {
                    locsX[i] = getLoc(foods[i]);
                    locsY[i] = getLoc(foods[i]);
                }

                Array.Sort(locsX, CompareLocX);
                Array.Sort(locsY, CompareLocY);
                Array.Sort(foods, CompareFoodX);

                Vector3 prev = Vector3.zero;
                for (int i = 0; i < locsX.Length; i++)
                {
                    int count = 1;
                    int myLen = probabilities.Length;

                    //print(foods[i].layer);

                    if (locsX.Length == 1 || i > 0 && Vector3.Distance(prev, locsX[i]) > radius)
                    {
                        prev = locsX[i];
                        for (int j = i - myLen; j <= i + myLen; j++)
                        {
                            if (j != i)
                            {
                                if (j >= 0 && j < locsX.Length)
                                {
                                    if (Vector3.Distance(locsX[i], locsX[j]) < radius)
                                    {
                                        print("Distance good");
                                        count += 1;
                                    }
                                }
                            }
                        }

                        int translate_i = Array.IndexOf(locsY, locsX[i]);

                        for (int j = translate_i - myLen; j <= translate_i + myLen; j++)
                        {
                            if (j != translate_i)
                            {
                                if (j >= 0 && j < locsY.Length)
                                {
                                    if (Vector3.Distance(locsY[translate_i], locsY[j]) < radius)
                                    {
                                        print("Distance good");
                                        count += 1;
                                    }
                                }
                            }
                        }

                        count = Math.Min(count, myLen);
                        /*print("count " + count);
                        if (count == 1)
                        {
                            foods[i].layer = 8;
                            print("set layer invisible");
                        }
                        else
                        {
                            foods[i].layer = 0;
                            print("set collision possible");

                        }*/
                        float p = probabilities[count - 1];
                        var random = new System.Random();
                        float gen_p = (float)random.NextDouble();
                        if (gen_p < p)
                        {
                            //print("creating food");
                            Vector3 offset = new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, UnityEngine.Random.Range(-2f, 2f));
                            CreateFoodLocal(locsX[i] + offset);
                        }

                    }

                }


            }

            timeElapsed = timeNeeded + timeElapsed;

        }

    }

}
