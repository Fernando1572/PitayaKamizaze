using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,

    }

    public PerformAction BattleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();
    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();

    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public HeroGUI HeroInput;

    public List<GameObject> HerosToManage = new List<GameObject>();
    private HandleTurn HeroChoice;

    public GameObject enemyButton;
    public Transform Spacer;
    
    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleStates = PerformAction.WAIT;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        HeroInput = HeroGUI.ACTIVATE;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);

        EnemyButtons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (BattleStates)
        {
            case (PerformAction.WAIT):
                if (PerformList.Count > 0)
                {
                    BattleStates = PerformAction.TAKEACTION;
                }
                break;
            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }

                if (PerformList[0].Type == "Hero")
                {
                    //Debug.Log("Hero is here to perform");
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.ACTION;
                }

                BattleStates = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION):
                //idle
                break;
        }

        switch (HeroInput)
        {
            case (HeroGUI.ACTIVATE):
                if(HerosToManage.Count > 0)
                {
                    HerosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    HeroChoice = new HandleTurn();
                    
                    AttackPanel.SetActive(true);
                    HeroInput = HeroGUI.WAITING;
                    
                }
            break;

            case (HeroGUI.WAITING):
                //idle
            break;

            case (HeroGUI.DONE):
                HeroInputDone();
            break;
        }
    }

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }

    public void EnemyButtons()
    {
        foreach (GameObject enemy in EnemiesInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            // Obtiene el componente TextMeshProUGUI en lugar de Text (esto lo corrigio gemini por que el tutorial estaba desactualizado)
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null && cur_enemy != null && cur_enemy.enemy != null)
            {
                buttonText.text = cur_enemy.enemy.name;
            }
            else if (buttonText == null)
            {
                Debug.LogError("No se encontró TextMeshProUGUI en el Prefab del botón.");
            }

            button.EnemyPrefab = enemy;
            newButton.transform.SetParent(Spacer, false);
        }
    }

    public void Input1 () //attack button
    {
        HeroChoice.Attacker = HerosToManage[0].name;
        HeroChoice.AttackersGameObject = HerosToManage[0];
        HeroChoice.Type = "Hero";

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }

    public void Input2 (GameObject ChoosenEnemy) //enemy selection
    {
        HeroChoice.AttackersTarget = ChoosenEnemy;
        HeroInput = HeroGUI.DONE;
    }

    public void HeroInputDone()
    {
        PerformList.Add(HeroChoice);
        EnemySelectPanel.SetActive(false);
        HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HerosToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }
}
