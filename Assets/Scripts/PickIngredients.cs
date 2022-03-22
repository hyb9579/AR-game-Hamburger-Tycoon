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

// ȭ�� �ϴ��� ���ø� Ŭ���ϸ� ������ (ȭ�� ��� ���� ���ϴ� ��ῡ �����ϸ�) ��Ḧ ���ÿ� �װ� ����� ��ũ��Ʈ
public class PickIngredients : MonoBehaviour
{
    // �÷����� �߻��ϴ� ���� ȿ���� �߻���Ű�� ���� ȿ���� ����ϴ� EffectPlayer ����
    [SerializeField]
    private EffectPlayer effectPlayer;

    // ������ ���� �������� �����Ǹ� ���� ǥ���� �� �ֵ��� �ϱ����� RecipeDisplay ����
    [SerializeField]
    private RecipeDisplay recipeDisplay;

    [SerializeField]
    private PlayUIManager UIManager;

    // ���� ��ῡ �����ϸ�, ��ῡ �˸´� ���������� �ٲ� ���� �ش� ��ᰡ ���õǾ������� �˷��ֱ� ���� ��ᵵ������ �迭�� ����
    [SerializeField]
    private GameObject[] pointers;

    [SerializeField]
    private GameManager gameManager;

    // ���������� �����ϴ� ��� ������Ʈ�� ��� �̸��� Ingredient Ŭ���� ������Ʈ�� ��ҷ� ���� ����Ʈ
    public List<Ingredient> ingredients = new List<Ingredient>();

    // ����ڰ� �������� �÷� ���� ��� �̸����� �����ϴ� ����Ʈ
    private List<string> ingredientOnPan = new List<string>();

    // �������� ó������ ��ġ�� ����� ��ġ�� ������ ���ӿ�����Ʈ
    [SerializeField]
    private GameObject hamburgerPos;

    // ó�� �Ǵ� ���� �ܹ��Ÿ� ���½��Ѿ� �Ҷ�, ����ִ� �ܹ��Ű� �ʿ��ѵ�, �׶� ����� ���� ������Ʈ�� ����
    [SerializeField]
    private GameObject emptyHGPrefab;

    // ��ũ��Ʈ���� ���ÿ� ���� �ܹ��Ÿ� ������ ����
    private GameObject Hamburger;

    // ��Ḧ �ϳ��� ��ġ�Ҷ�, �Ź� �����Ǵ� ��� ���� ������Ʈ�� �����ϴ� ����
    private GameObject ingredient;

    [SerializeField]
    private Text debug;

    // ������ �ϼ�������, ��Ḧ ����� �׾Ҵ��� Ȯ���Ѵ� ����
    private bool isCorrect ;

    // collider�� ��ῡ �ٰ�������, �ش� ��ᰡ �������� �˷��ִ� ����
    private static string onIngredient;

    [SerializeField]
    private SoundManager soundManager;

    // ���� ���۵ǰ� ���� �ʱ�ȭ
    private void Start()
    {
        ingredient = null;
        isCorrect = false;
        Hamburger = Instantiate(emptyHGPrefab, hamburgerPos.transform.position, hamburgerPos.transform.rotation, hamburgerPos.transform.parent);
    }

    // �տ� ��ᰡ ������ ��ῡ �˸��� ���������� �ٲ��, �ش� ����� �̸��� ������
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

    // ������ ���۵ǰ� �ϴ��� ���ø� Ŭ���ϸ� �տ� ���� ��ῡ ���� �˸��� �۾� ����
    public void OnTouch()
    {
        // ���� �������뿡 ��������, ���ø� ��ġ�ϸ� �ش� ���� ��Ḧ ��� ������ ���½�Ű�� �ش� ȿ�� ����
        if (onIngredient == "TrashCan")
        {
            debug.text = "����";

            ResetHamburger(0f);

            gameManager.foodTrashCnt++;

            effectPlayer.playAbandonEffect();

            soundManager.WrongOrder();

            return;
        }

        // ��ᵥ���Ͱ� ��� ingredients���� �ϳ��� ���Ͽ� ���� �������� �ϴ� ��ᰡ �������� Ȯ���ϰ� �״´�
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

        // ���� �������� ���� ���� ���� ���̸� ������ �ϼ��Ѱ����� ����, ������ ����� �Ϸ��ߴ���, ���� �������� �Ǵ��Ѵ�
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

                    debug.text = GameManager.order.name + "�Ϸ�!";

                    recipeDisplay.ResetHamburger();

                    gameManager.NewOrder();
                }
            }
        }
    }

    // ���� �Ϸ��� ����� ���� UIManager�� ���� ȿ�� ����ϴ� �ڷ�ƾ �޼ҵ��
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

    // ���� �ܹ��� ����
    private void ResetHamburger(float delay = 2f)
    {
        ingredientOnPan.Clear();

        Destroy(Hamburger, delay);

        Hamburger = Instantiate(emptyHGPrefab, hamburgerPos.transform.position, hamburgerPos.transform.rotation, hamburgerPos.transform.parent);
    }

    // ������ ��ῡ ���� ���� �ش� ��ῡ ��︮�� ���������� �ٲ�
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