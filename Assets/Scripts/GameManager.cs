using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Order
{
    public bool isSat;
    public bool isSpecialOrder;
    public float timer;
    public string name;
    public List<string> ingredients = new List<string>();
    public string addedIngredient;
    public string subedIngredient;

    public void addIngredient(string ingredient)
    {
        ingredients.Insert(Random.Range(1, ingredients.Count - 1), ingredient);
    }

    public void subIngredient(string ingredient)
    {
        ingredients.Remove(ingredient);
    }
}

public class GameManager : MonoBehaviour
{
    public bool isEnd = false;

    [SerializeField]
    private PlayUIManager UIManager;

    [SerializeField]
    private RecipeDisplay recipeDisplay;

    public float orderTime;

    public static Order order = new Order();

    List<string> menu = new List<string>();

    public int normalOrderCnt;

    public int specialOrederCnt;

    [SerializeField]
    private Text me;

    [SerializeField]
    private Text ingredi;

    [SerializeField]
    private Text DebugLog;

    public Text gameTime;

    private float time;

    public int foodTrashCnt;

    public int unsatisfiedCunstomerCnt;

    public int specialOrderClearCnt;

    public int normalOrderClearCnt;

    public float endTime = 0;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        unsatisfiedCunstomerCnt = 0;

        specialOrderClearCnt = 0;

        normalOrderClearCnt = 0;

        foodTrashCnt = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (string name in DataLoader.recipes.Keys)
        {
            menu.Add(name);
        }
    }
    
    private void Update()
    {
        if (isEnd)
        {
            return;
        }

        //손님 한 명 제한시간
        order.timer -= Time.deltaTime;

        if (order.timer < 0)
        {
            order.timer = 0;

            if (order.isSat)
            {
                unsatisfiedCunstomerCnt++;
                order.isSat = false;
            }
        }
    }

    
    public void NewOrder()
    {
        DebugLog.text = "1";

        if (normalOrderCnt + specialOrederCnt <= 0) //모든 주문 횟수가 끝나면!! 
        {
            DebugLog.text = "모든 주문 끝!";

            isEnd = true;

            return;
        }

        order.timer = orderTime;
        order.isSat = true;
        DebugLog.text = "2";

        if (Random.Range(0, normalOrderCnt + specialOrederCnt) > specialOrederCnt - 1)
        {
            order.isSpecialOrder = false;

            SelectRecipe();

            normalOrderCnt--;
        }
        else
        {
            order.isSpecialOrder = true;

            SelectRecipe();

            order.subedIngredient = order.ingredients[Random.Range(1, order.ingredients.Count - 1)];
            order.subIngredient(order.subedIngredient);

            order.addedIngredient = order.ingredients[Random.Range(1, order.ingredients.Count - 1)];
            order.addIngredient(order.addedIngredient);

            specialOrederCnt--;

            DebugLog.text = "SpecialOrder!";
        }

        DebugLog.text = "3";
        
        recipeDisplay.GenRecipe();
        DebugLog.text = "4";
        
        UIManager.OrderInCol();
        DebugLog.text = "5";
    }

    private void SelectRecipe()
    {
        Debug.Log("1");

        order.name = menu[Random.Range(0, menu.Count)];

        Debug.Log("2");
        Debug.Log(order.name);
        order.subedIngredient = null;
        order.addedIngredient = null;
        order.ingredients.Clear();

        Debug.Log("3");
        foreach (string str in DataLoader.recipes[order.name])
        {
            order.ingredients.Add(str);
            Debug.Log(str);
        }





    }
}