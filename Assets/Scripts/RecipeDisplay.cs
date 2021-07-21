using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
    [SerializeField]
    private PickIngredients pickIngredients;

    [SerializeField]
    private GameObject hamburgerPos;

    [SerializeField]
    private GameObject emptyHGPrefab;

    public GameObject Hamburger;

    private GameObject tmpIngredient;

    public Text DebugLog;

    public void GenRecipe()
    {
        tmpIngredient = null;
        Hamburger = Instantiate(emptyHGPrefab, hamburgerPos.transform.position, hamburgerPos.transform.rotation, hamburgerPos.transform.parent);

        foreach (string ingredient in GameManager.order.ingredients)
        {
            GameObject result = pickIngredients.ingredients.Find(x => x.name == ingredient).ingredient;

            Debug.Log(result);

            if (tmpIngredient != null)
            {
                tmpIngredient = Instantiate(result, tmpIngredient.transform.Find("DisplayPos").transform);
            }
            else
            {
                Debug.Log(Hamburger);
                tmpIngredient = Instantiate(result, Hamburger.transform);
            }

            tmpIngredient.transform.parent = Hamburger.transform;
        }

        Hamburger.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void ResetHamburger(float delay = 0f)
    {
        Destroy(Hamburger, delay);
    }
}