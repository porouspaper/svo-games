using UnityEngine;

public class FoodLogic : MonoBehaviour
{

    public void OnEaten()
    {
        FoodCollectorSettings m_FoodCollecterSettings = FindObjectOfType<FoodCollectorSettings>();
        m_FoodCollecterSettings.foods.Remove(gameObject);

        Destroy(gameObject);

    }
}
