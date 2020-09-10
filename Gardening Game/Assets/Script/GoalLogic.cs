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
    public GardenScene settings;
    public int cookTime = 5;
    public List<GameObject> ingredients = new List<GameObject>();
    public float timeLeft = 5.0f;
    public int numFoodToSpawn;
    //public Text textbox;


    private void Start()
    {
        if (isCooperative)
        {
            gameObject.GetComponentInChildren<Renderer>().material = cooperativeMaterial;
            gameObject.tag = "coopGoal";
        }
        else
        {
            gameObject.GetComponentInChildren<Renderer>().material = selfishMaterial;
            gameObject.tag = "selfGoal";
        }

        timeLeft = 5.0f;

        
    }

    // cooks food
    public void SpawnFood(int[] types) {

        Vector3 cube_size = new Vector3(4, 4, 4);
        numFoodToSpawn = ingredients.Count;
        GameObject[] f = new GameObject[numFoodToSpawn];
        //print("spawning this many food " + numFoodToSpawn);
        for (int i = 0; i < numFoodToSpawn; i++)
        {
            f[i] = Instantiate(food, new Vector3(Random.Range(-cube_size.x / 2, cube_size.x / 2), 1f,
                    Random.Range(-cube_size.z / 2, cube_size.z / 2)) + transform.position,
                    Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
            FoodScript fs = f[i].GetComponent<FoodScript>();
            fs.types = types;
            fs.fromCooperative = isCooperative;
            settings.foodCount += 1;

        }
        

    }



    //register an object
    private void OnTriggerEnter(Collider col)
    {
        //print(this.isActiveAndEnabled);
        if (col.gameObject.CompareTag("ingredient1") || col.gameObject.CompareTag("ingredient2")
            || col.gameObject.CompareTag("ingredient3") || col.gameObject.CompareTag("ingredient4"))
        {
            //print("registering ingredient");
            if (ingredients.Count == 0 && !ingredients.Contains(col.gameObject))
            {
                timeLeft = 5.0f;
                ingredients.Add(col.gameObject);
                //print("cook started, first ingredient registered");
            }
            else if (!ingredients.Contains(col.gameObject))
            {
                //print("added ingredient to register");
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

    private void FixedUpdate()
    {
        //print(ingredients.Count);
        //print("time left " + timeLeft);
        //print(this.isActiveAndEnabled);
        if (ingredients.Count > 0 && settings.foodCount < 1000)
        {
            if (timeLeft >= 0)
            {
                timeLeft -= Time.deltaTime;
                print("cooking");
            }
            else
            {
                timeLeft = 5.0f;
                //print("spawning");
                int[] arr = new int[settings.agents.Length];
                foreach (GameObject obj in ingredients)
                {
                    //print(obj.GetType());
                    IngredientType m_ingredientType = obj.GetComponentInChildren<IngredientType>();

                    if (m_ingredientType == null)
                    {
                        //print("oh no, it is null");
                    }
                    int k = m_ingredientType.type;
                    //print("arr k");
                    //print(arr[k]);
                    arr[k] += 1;



                }

                if (isCooperative)
                {
                    for (int i = 0; i < settings.agents.Length; i++) {
                        settings.publicContribution[i] += arr[i];
                        //print(settings.publicContribution[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < settings.agents.Length; i++)
                    {
                        settings.privateContribution[i] += arr[i];
                        //print(settings.privateContribution[i]);
                    }
                }
                SpawnFood(arr);
                foreach (GameObject obj in ingredients)
                {
                    //print(obj.GetType());
                    IngredientType m_ingredientType = obj.GetComponentInChildren<IngredientType>();

                    if (m_ingredientType == null)
                    {
                        print("oh no, it is null");
                    }
                    int k = m_ingredientType.type;
                    m_ingredientType.OnDestroy();

                }
                ingredients.Clear();
            }
        }
        //textbox.text = timeLeft.ToString();


    }


}
