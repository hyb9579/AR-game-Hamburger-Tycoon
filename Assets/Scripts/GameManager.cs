using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

/*
�ϳ��� '�ֹ�'�� ��� ���� ��Ÿ���� Ŭ����
�ֹ� ���, �ð� ����, ����� ���� ����, ���� ���� �Ǻ��� ��� �߰� �� ���Ÿ� �� �� �ִ� �Ӽ��� �޼ҵ���� �ִ�
*/
public class Order
{
    // ������ ���ѻ��� (���ѽð�, �˸��� ���) �� ��� ���� �ذ������� default�� true�� �����ǰ� �ƴ϶�� false�� �ٲ��
    public bool isSat;

    // ����� ������� true
    public bool isSpecialOrder;

    // �ֹ��� ���ѽð� ����
    public float timer;
    public string name;
    public List<string> ingredients = new List<string>();

    // ����� ������ ��Ḧ �߰�, �����Ҷ� ������ ����
    public string addedIngredient;
    public string subedIngredient;

    // ������ ���޹޾� �������� ��Ḧ �߰� ���� �ϴ� �Լ�
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

    // ������ ���� UI �����ϴ� RecipeDisplay ��ũ��Ʈ
    [SerializeField]
    private RecipeDisplay recipeDisplay;

    public float orderTime;

    // ������ �ѹ��� �ϳ��� ���� �� �ִ�
    // ��, ���ÿ� 2���̻� ������ �� ����, �پ��� ��ġ���� ���� �����ؾ� �ϱ� ������ �������� order�� �̿��Ѵ�
    public static Order order = new Order();

    // Start()���� ������ �����Ϳ��� �޾ƿ� �޴� ������ ���� List
    List<string> menu = new List<string>();

    // �Ϲݰ� ����� ���� ���� ����
    public int normalOrderCnt;
    public int specialOrederCnt;

    // ����� ������ ȭ�鿡 ǥ���� ����
    [SerializeField]
    private Text DebugLog;

    // ���� ��� �ð��� ǥ��
    public Text gameTime;

    // ��Ḧ ���� Ƚ��
    public int foodTrashCnt;

    // �Ҹ��� ��
    public int unsatisfiedCunstomerCnt;

    // ����� ���� �Ϸ� ��
    public int specialOrderClearCnt;

    // �Ϲ� ���� �Ϸ� ��
    public int normalOrderClearCnt;

    // ������ ���۵Ǳ� ���� GameManager�� ���� �ʱ�ȭ
    private void Awake()
    {
        // ����� ��ȯ ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        unsatisfiedCunstomerCnt = 0;

        specialOrderClearCnt = 0;

        normalOrderClearCnt = 0;

        foodTrashCnt = 0;
    }

    // ������ ���۵��ڸ��� �����ͷ� ���� �޴� �̸����� ������
    void Start()
    {
        foreach (string name in DataLoader.recipes.Keys)
        {
            menu.Add(name);
        }
    }
    
    // ������ ���� �ð��� ����ϰ� ���ѽð����� �ذ����� ���ϸ� �Ҹ� ������ �ٲ�
    private void Update()
    {
        if (isEnd)
        {
            return;
        }

        //�մ� �� �� ���ѽð�
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

    // ���ο� ������ ����
    public void NewOrder()
    {
        // �����ִ� �ֹ��� �ִ��� Ȯ��
        if (normalOrderCnt + specialOrederCnt <= 0)
        {
            DebugLog.text = "��� �ֹ� ��!";

            isEnd = true;

            return;
        }

        // �ֹ� �ʱ�ȭ�� �ʿ��� ���� ����
        order.timer = orderTime;
        order.isSat = true;

        // �Ϲ� ������ ����� ������ �������ִ� �б�
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

            // ����� ������ ������ ��Ḧ �ϳ��� �߰��ϰ� �ϳ��� �����Ѵ�
            order.subedIngredient = order.ingredients[Random.Range(1, order.ingredients.Count - 1)];
            order.subIngredient(order.subedIngredient);

            order.addedIngredient = order.ingredients[Random.Range(1, order.ingredients.Count - 1)];
            order.addIngredient(order.addedIngredient);

            specialOrederCnt--;

            DebugLog.text = "SpecialOrder!";
        }
        
        // ������ ������ ���� �����Ǹ� ǥ��
        recipeDisplay.GenRecipe();
        
        // ������ ������ ���� �� ��ǳ���� ���� ǥ��
        UIManager.OrderInCol();
    }

    // ���� �̸��� ���� ��� ���ϱ�
    private void SelectRecipe()
    {
        order.name = menu[Random.Range(0, menu.Count)];

        order.subedIngredient = null;
        order.addedIngredient = null;
        order.ingredients.Clear();

        foreach (string str in DataLoader.recipes[order.name])
        {
            order.ingredients.Add(str);
            Debug.Log(str);
        }
    }
}