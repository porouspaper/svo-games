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
    public float timeNeeded = 2;
    public FoodCollectorSettings m_FoodCollecterSettings;


    void CreateFood(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var loc = new Vector3(UnityEngine.Random.Range(-range, range), 1f,
                UnityEngine.Random.Range(-range, range)) + transform.position;
            InstantiateFood(loc);
        }
    }

    void CreateFoodLocal(Vector3 loc)
    {
        InstantiateFood(loc);
    }

    void InstantiateFood(Vector3 loc)
    {
        GameObject f = Instantiate(food, loc,
        Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 90f)));
        m_FoodCollecterSettings.totalApples += 1;
        m_FoodCollecterSettings.foods.Add(f);
    }

    public void ResetFoodArea(GameObject[] agents)
    {

        CreateFood(numFood);
        m_FoodCollecterSettings.totalApples += numFood;
    }



    public static int CompareLocX(Vector3 v1, Vector3 v2)
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
            GameObject[] foodsObj = GameObject.FindGameObjectsWithTag("food");
            if (foodsObj.Length < 500)
            {

                Func<GameObject, Vector3> getLoc = (x) => x.transform.position;

                Vector3[] locsX = new Vector3[foodsObj.Length];
                Vector3[] locsY = new Vector3[foodsObj.Length];

   
                for (int i = 0; i < foodsObj.Length; i++)
                {
                    if (foodsObj[i] != null)
                    {
                        locsX[i] = getLoc(foodsObj[i]);
                        locsY[i] = getLoc(foodsObj[i]);
                    }
                }

                Array.Sort(locsX, CompareLocX);
                Array.Sort(locsY, CompareLocY);
                Array.Sort(foodsObj, CompareFoodX);

                
                Vector3 prev = Vector3.zero;
                for (int i = 0; i < locsX.Length; i++)
                {
                    int count = 1;
                    int myLen = probabilities.Length;

                    //print(foods[i].layer);

                    HashSet<Vector3> countedFoods = new HashSet<Vector3>();
                    if ( locsX.Length == 1 || i > 0 && Vector3.Distance(prev, locsX[i]) > radius)
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
                                        countedFoods.Add(locsX[j]);
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
                                        if (!countedFoods.Contains(locsY[j]))
                                        {
                                            print("Distance good");
                                            count += 1;
                                        }
                                    }
                                }
                            }
                        }

                        count = Math.Min(count, myLen);
                        print("count " + count);
                        if (m_FoodCollecterSettings.SchellingCoop)
                        {
                            if (count == 1)
                            {
                                foodsObj[i].layer = 8;
                                print("set layer invisible");
                            }
                            else
                            {
                                foodsObj[i].layer = 0;
                                print("set collision possible");

                            }
                        }
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
