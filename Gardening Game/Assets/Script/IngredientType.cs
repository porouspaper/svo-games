using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientType : MonoBehaviour
{
    public int type;
    public bool respawn = true;
    public Material ingredient1;
    public Material ingredient2;
    public Material ingredient3;
    public Material ingredient4;
    public GardenArea myArea;

    public void SetType(int k)
    {
        type = k % 4;
        switch (type)
        {
            case 0:
                gameObject.GetComponentInChildren<Renderer>().material = ingredient1;
                gameObject.tag = "ingredient1";
                break;
            case 1:
                gameObject.GetComponentInChildren<Renderer>().material = ingredient2;
                gameObject.tag = "ingredient2";
                break;
            case 2:
                gameObject.GetComponentInChildren<Renderer>().material = ingredient3;
                gameObject.tag = "ingredient3";
                break;
            case 3:
                gameObject.GetComponentInChildren<Renderer>().material = ingredient4;
                gameObject.tag = "ingredient4";
                break;
        }
    }

    public void OnDestroy()
    {
        if (respawn)
        {
            transform.position = new Vector3(Random.Range(-myArea.range, myArea.range), 0f,
                Random.Range(-myArea.range, myArea.range)) + myArea.transform.position;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }


}
