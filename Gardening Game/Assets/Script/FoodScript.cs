using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public int[] types;
    public bool fromCooperative;



    public int[] OnEaten()
    {
        Destroy(gameObject);
        return types;
    }
}
