using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class HandleTurn
{
    public string Attacker; //Name of attacker
    public string Type; //Hero or Enemy
    public GameObject AttackersGameObject; //who attacks
    public GameObject AttackersTarget; //who is going to be attacked

    //witch attack is performed
}
