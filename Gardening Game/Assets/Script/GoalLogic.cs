using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic : MonoBehaviour
{
    public GameObject food;
    public int range = 5;
    public bool isCooperative;

    public Material cooperativeMaterial;
    public Material selfishMaterial;


    private void Start()
    {
        if (isCooperative)
        {
            gameObject.GetComponentInChildren<Renderer>().material = cooperativeMaterial;
        }
        else
        {
            gameObject.GetComponentInChildren<Renderer>().material = selfishMaterial;
        }
    }

    // cooks food
    public void SpawnFood(int[] types) {

        Vector3 cube_size = transform.localScale;
        GameObject f = Instantiate(food, new Vector3(Random.Range(-cube_size.x / 2, cube_size.x/2), 1f,
                Random.Range(-cube_size.z/2, cube_size.z/2)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
        FoodScript fs = f.GetComponent<FoodScript>();
        fs.types = types;
        fs.fromCooperative = isCooperative;
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject agent in agents)
        {
            Collider cols = agent.GetComponent<BoxCollider>();
            Physics.IgnoreCollision(cols, fs.GetComponent<Collider>());

        }

    }


}
