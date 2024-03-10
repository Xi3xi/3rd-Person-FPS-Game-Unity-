using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsController: MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerManager player;
    private SkillTree skillTree;
    private BreadthManager breadthManager;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    public void SetSkillTree(SkillTree tree, BreadthManager breadth){
        skillTree = tree;
        breadthManager = breadth;
        skillTree.SetSkills(player.GetSkills());
        breadthManager.SetSkills(player.GetSkills());
    }
}
