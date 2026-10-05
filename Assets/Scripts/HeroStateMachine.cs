using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
    public BaseHero hero;

    private BattleStateMachine BSM;
    public enum TurnState
    {
        PROSSESING,
        ADDTOLIST,
        WAITING,
        SELECCTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;

    public GameObject Selector;

    //IEnumerator
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 10f;

    void Start()
    {
        startPosition = transform.position;
        cur_cooldown = Random.Range(0, 2.5f);
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.PROSSESING;
    }

    void Update()
    {
        //Debug.Log(currentState);
        switch (currentState)
        {
            case (TurnState.PROSSESING):
                UpgradeProgressBar();
                break;
            case (TurnState.ADDTOLIST):
                BSM.HerosToManage.Add (this.gameObject);
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING):
                //Idle
                break;
            //case (TurnState.SELECCTING):
            //  break;
            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.DEAD):
                break;
        }
    }

    void UpgradeProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;

        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0 , 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.ADDTOLIST;
        }
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //animate the enemy near the hero to attack
        Vector3 heroPosition = new Vector3(EnemyToAttack.transform.position.x - 1.5f, EnemyToAttack.transform.position.y - 0.5f, EnemyToAttack.transform.position.z);
        while (MoveTowardsEnemy(heroPosition))
        {
            yield return null;
        }
        //wait abit
        yield return new WaitForSeconds(0.5f);
        //do damage

        //animate back to start position
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition))
        {
            yield return null;
        }
        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset BSM -> WAIT
        BSM.BattleStates = BattleStateMachine.PerformAction.WAIT;
        //end coroutine
        actionStarted = false;
        //reset this enemy state
        cur_cooldown = 0f;
        currentState = TurnState.PROSSESING;
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
}
