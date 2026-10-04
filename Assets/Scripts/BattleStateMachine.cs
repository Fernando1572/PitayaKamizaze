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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleStates = PerformAction.WAIT;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));

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

                }

                BattleStates = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION):

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

            // Obtiene el componente TextMeshProUGUI en lugar de Text
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
}
