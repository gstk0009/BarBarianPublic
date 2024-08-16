using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTargetSystem : MonoBehaviour
{
    public List<GameObject> TargetList;
    public List<GameObject> NPCIndex;


    private void Awake()
    {
        if (GameManager.Instance.NPCTargetSystem != null) return;

        GameManager.Instance.NPCTargetSystem = this;
        TargetList = new List<GameObject>{Player.Instance.gameObject};
        NPCIndex = new List<GameObject>();
    }

}
