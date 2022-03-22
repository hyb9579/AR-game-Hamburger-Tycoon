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

// 화면 하단의 접시를 클릭하면 선택한 (화면 가운데 손을 원하는 재료에 접근하면) 재료를 접시에 쌓게 만드는 스크립트
public class PickIngredients : MonoBehaviour
{
    // 플레이중 발생하는 여러 효과를 발생시키기 위해 효과를 담당하는 EffectPlayer 참조
    [SerializeField]
    private EffectPlayer effectPlayer;

    // 오더가 새로 들어왔을때 레시피를 새로 표시할 수 있도록 하기위해 RecipeDisplay 참조
    [SerializeField]
    private RecipeDisplay recipeDisplay;

    [SerializeField]
    private PlayUIManager UIManager;

    // 손이 재료에 접근하면, 재료에 알맞는 조리도구로 바뀌어서 현재 해당 재료가 선택되어있음을 알려주기 위해 재료도구들을 배열로 참조
    [SerializeField]
    private GameObject[] pointers;

    [SerializeField]
    private GameManager gameManager;

    // 프리팹으로 존재하는 재료 오브젝트와 재료 이름을 Ingredient 클래스 오브젝트를 요소로 갖는 리스트
    public List<Ingredient> ingredients = new List<Ingredient>();

    // 사용자가 접시위에 올려 놓은 재료 이름들을 저장하는 리스트
    private List<string> ingredientOnPan = new List<string>();

    // 접시위에 처음으로 배치할 재료의 위치를 참조할 게임오브젝트
    [SerializeField]
    private GameObject hamburgerPos;

    // 처음 또는 쌓은 햄버거를 리셋시켜야 할때, 비어있는 햄버거가 필요한데, 그때 사용할 게임 오브젝트를 참조
    [SerializeField]
    private GameObject emptyHGPrefab;

    // 스크립트에서 접시에 쌓을 햄버거를 참조할 변수
    private GameObject Hamburger;

    // 재료를 하나씩 배치할때, 매번 생성되는 재료 게임 오브젝트를 참조하는 변수
    private GameObject ingredient;

    [SerializeField]
    private Text debug;

    // 오더를 완성했을때, 재료를 제대로 쌓았는지 확인한는 변수
    private bool isCorrect ;

    // collider가 재료에 다가갔을때, 해당 재료가 무엇인지 알려주는 변수
    private static string onIngredient;

    [SerializeField]
    private SoundManager soundManager;

    // 씬이 시작되고 변수 초기화
    private void Start()
    {
        ingredient = null;
        isCorrect = false;
        Hamburger = Instantiate(emptyHGPrefab, hamburgerPos.transform.position, hamburgerPos.transform.rotation, hamburgerPos.transform.parent);
    }

    // 손에 재료가 닿으면 재료에 알맞은 조리도구로 바뀌고, 해당 재료의 이름을 가져옴
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

    // 게임이 시작되고 하단의 접시를 클릭하면 손에 닿은 재료에 따라 알맞은 작업 수행
    public void OnTouch()
    {
        // 손을 쓰레기통에 가져간후, 접시를 터치하면 해당 쌓은 재료를 모두 버리고 리셋시키며 해당 효과 실행
        if (onIngredient == "TrashCan")
        {
            debug.text = "버림";

            ResetHamburger(0f);

            gameManager.foodTrashCnt++;

            effectPlayer.playAbandonEffect();

            soundManager.WrongOrder();

            return;
        }

        // 재료데이터가 담긴 ingredients에서 하나씩 비교하여 현재 쌓으려고 하는 재료가 무엇인지 확인하고 쌓는다
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

        // 만약 마지막에 쌓은 것은 덮는 빵이면 오더를 완성한것으로 보고, 오더를 제대로 완료했는지, 만족 상태인지 판단한다
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

    // 오더 완료후 결과에 따라 UIManager를 통해 효과 재생하는 코루틴 메소드들
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

    // 쌓은 햄버거 리셋
    private void ResetHamburger(float delay = 2f)
    {
        ingredientOnPan.Clear();

        Destroy(Hamburger, delay);

        Hamburger = Instantiate(emptyHGPrefab, hamburgerPos.transform.position, hamburgerPos.transform.rotation, hamburgerPos.transform.parent);
    }

    // 선택한 재료에 따라 손을 해당 재료에 어울리는 조리도구로 바꿈
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