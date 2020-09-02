//Detect when the orange block has touched the goal.
//Detect when the orange block has touched an obstacle.
//Put this script onto the orange block. There's nothing you need to set in the editor.
//Make sure the goal is tagged with "goal" in the editor.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoalDetector : MonoBehaviour
{
    /// <summary>
    /// The associated agent.
    /// This will be set by the agent script on Initialization.
    /// Don't need to manually set.
    /// </summary>
    GoalLogic m_goalLogic;
    public int cookTime;
    public List<GameObject> ingredients = new List<GameObject>();
    public float timeLeft = 5.0f;
    //public Text textbox;

    private void Start()
    {
        //textbox.text = timeLeft.ToString();

    }
    //register an object
    private void OnTriggerEnter(Collider col)
    {
        print(this.isActiveAndEnabled);
        print("collide something");
        if (col.gameObject.CompareTag("ingredient1") || col.gameObject.CompareTag("ingredient2")
            || col.gameObject.CompareTag("ingredient3") || col.gameObject.CompareTag("ingredient4"))
        {
            print("collision ingredient");
            if (ingredients.Count == 0 && !ingredients.Contains(col.gameObject))
            {
                timeLeft = cookTime;
                ingredients.Add(col.gameObject);
                print("cook started");
            }
            else if(!ingredients.Contains(col.gameObject))
            {
                ingredients.Add(col.gameObject);
            }
        }
    }

    //unregister an object
    private void OnTriggerExit(Collider col)
    {
        //print(this.isActiveAndEnabled);
        if (col.gameObject.CompareTag("ingredient1") || col.gameObject.CompareTag("ingredient2")
                   || col.gameObject.CompareTag("ingredient3") || col.gameObject.CompareTag("ingredient4"))
        {
            if (ingredients.Contains(col.gameObject))
            {
                ingredients.Remove(col.gameObject);
            }

        }

    }

    private void Update()
    {
        //print(ingredients.Count);
        //print(timeLeft);
        //print(this.isActiveAndEnabled);
        if (ingredients.Count > 0) {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                //print("cooking");
            }
            else
            {
                //print("spawning");
                m_goalLogic = GetComponent<GoalLogic>();
                int[] arr = new int[4];
                foreach(GameObject obj in ingredients){
                    //print(obj.GetType());
                    IngredientType m_ingredientType = obj.GetComponentInChildren<IngredientType>();

                    if (m_ingredientType == null)
                    {
                        print("oh no, it is null");
                    }
                    int k = m_ingredientType.type;
                    //print("arr k");
                    //print(arr[k]);
                    arr[k] += 1;
                    m_ingredientType.OnDestroy();


                }
                ingredients.Clear();

                m_goalLogic.SpawnFood(arr);
            }
        }
        //textbox.text = timeLeft.ToString();


    }
}
