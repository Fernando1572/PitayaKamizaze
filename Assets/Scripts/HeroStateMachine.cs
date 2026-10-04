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

    void Start()
    {
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
}
