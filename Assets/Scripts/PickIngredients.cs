using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using UnityEngine.UI;

[System.Serializable]
public struct Ingredient
{
    public GameObject ingredient;
    public string name;
}

public class PickIngredients : MonoBehaviour
{
    [SerializeField]
    private EffectPlayer effectPlayer;

    [SerializeField]
    private RecipeDisplay recipeDisplay;

    [SerializeField]
    private PlayUIManager UIManager;

    [SerializeField]
    private GameObject[] pointers;

    [SerializeField]
    private GameManager gameManager;

    public List<Ingredient> ingredients = new List<Ingredient>();

    private List<string> ingredientOnPan = new List<string>();

    [SerializeField]
    private GameObject hamburgerPos;

    [SerializeField]
    private GameObject emptyHGPrefab;

    private GameObject Hamburger;

    private GameObject ingredient;

    [SerializeField]
    private Text debug;

    private bool isCorrect ;

    private static string onIngredient;

    [SerializeField]
    private SoundManager soundManager;

    private void Start()
    {
        ingredient = null;
        isCorrect = false;
        Hamburger = Instantiate(emptyHGPrefab, hamburgerPos.transform.position, hamburgerPos.transform.rotation, hamburgerPos.transform.parent);
    }


    private void OnTriggerEnter(Collider other)
    {
        SetPointer(other.tag);

        onIngredient = other.tag;
    }

    private void OnTriggerStay(Collider other)
    {
        SetPointer(other.tag);

        onIngredient = other.tag;
    }

    private void OnTriggerExit(Collider other)
    {
        SetPointer("");
        onIngredient = null;
    }

    public void OnTouch()
    {
        if (onIngredient == "TrashCan")
        {
            debug.text = "버림";

            ResetHamburger(0f);

            gameManager.foodTrashCnt++;

            effectPlayer.playAbandonEffect();

            soundManager.WrongOrder();

            return;
        }

        foreach (Ingredient prefab in ingredients)
        {
            if (onIngredient == prefab.name)
            {
                ingredientOnPan.Add(prefab.name);

                if (ingredient != null)
                {
                    ingredient = Instantiate(prefab.ingredient, ingredient.transform.Find("StackPos").transform);
                }
                else
                {
                    ingredient = Instantiate(prefab.ingredient, Hamburger.transform);
                }

                ingredient.transform.parent = Hamburger.transform;


                if (prefab.name != "BreadSesameTop") 
                {
                    effectPlayer.playStackEffect();
                    soundManager.ButtonSound();
                }

                break;
            }
        }

        if (ingredientOnPan[ingredientOnPan.Count - 1] == "BreadSesameTop")
        {
            debug.text = "Check Recipe";

            if (ingredientOnPan.Count != GameManager.order.ingredients.Count)
            {
                GameManager.order.isSat = false;

                gameManager.unsatisfiedCunstomerCnt++;

                StartCoroutine("WrongUI");

                debug.text = "Wrong Burger!";

                recipeDisplay.ResetHamburger();

                ResetHamburger(0f);

                effectPlayer.playWrongStackEffect();

                soundManager.WrongOrder();

                gameManager.NewOrder();
            }
            else
            {
                for (int i = 0; i < ingredientOnPan.Count; i++)
                {
                    if (ingredientOnPan[i] != GameManager.order.ingredients[i])
                    {
                        gameManager.unsatisfiedCunstomerCnt++;
                        GameManager.order.isSat = false;
                        StartCoroutine("WrongUI");
                        debug.text = "Wrong Burger!";

                        recipeDisplay.ResetHamburger();

                        ResetHamburger(0f);

                        effectPlayer.playWrongStackEffect();
                        soundManager.WrongOrder();

                        gameManager.NewOrder();

                        isCorrect = false;
                        break;
                    }

                    isCorrect = true;
                }

                if (isCorrect)
                {
                    if(GameManager.order.isSat)
                    {
                        if (GameManager.order.isSpecialOrder)
                        {
                            gameManager.specialOrderClearCnt++;
                            UIManager.AddStar();
                            effectPlayer.playSpecialSuccessEffect();
                            soundManager.SpecialOrderClear();
                        }
                        else
                        {
                            gameManager.normalOrderClearCnt++;
                            effectPlayer.playNormalSuccessEffect();
                            soundManager.NormalOrderClear();
                        }

                        StartCoroutine("SatUI");
                    }
                    else
                    {
                        StartCoroutine("UnsatUI");
                        soundManager.LateOrder();
                    }

                    ResetHamburger(0f);

                    debug.text = GameManager.order.name + "완료!";

                    recipeDisplay.ResetHamburger();

                    gameManager.NewOrder();
                }
            }
        }
    }

    IEnumerator SatUI()
    {
        UIManager.customerOrder.SetActive(false);
        UIManager.customerSat.SetActive(true);

        yield return new WaitForSeconds(2f);

        UIManager.customerOrder.SetActive(true);
        UIManager.customerSat.SetActive(false);

        yield break;
    }

    IEnumerator UnsatUI()
    {
        UIManager.customerOrder.SetActive(false);
        UIManager.customerUnSat.SetActive(true);

        yield return new WaitForSeconds(2f);

        UIManager.customerOrder.SetActive(true);
        UIManager.customerUnSat.SetActive(false);

        yield break;
    }

    IEnumerator WrongUI()
    {
        UIManager.customerOrder.SetActive(false);
        UIManager.customerWrong.SetActive(true);

        yield return new WaitForSeconds(2f);

        UIManager.customerOrder.SetActive(true);
        UIManager.customerWrong.SetActive(false);

        yield break;
    }

    private void ResetHamburger(float delay = 2f)
    {
        ingredientOnPan.Clear();

        Destroy(Hamburger, delay);

        Hamburger = Instantiate(emptyHGPrefab, hamburgerPos.transform.position, hamburgerPos.transform.rotation, hamburgerPos.transform.parent);
    }

    private void SetPointer(string tagName)
    {
        foreach (GameObject tmp in pointers)
        {
            tmp.SetActive(false);
        }

        switch (tagName)
        {
            case "Cheese":
            case "Lettuces":
            case "BreadSesameBottom":
            case "PurpleOnions":
            case "BreadSesameTop":
            case "Bacons":
            case "Tomatoes":
                pointers[1].SetActive(true);
                break;
            case "Patty":
            case "BulgogiPatty":
            case "ChickenPatty":
            case "FriedEgg":
                pointers[2].SetActive(true);
                break;
            case "Ketchup":
                pointers[3].SetActive(true);
                break;
            default:
                pointers[0].SetActive(true);
                break;
        }
    }
}