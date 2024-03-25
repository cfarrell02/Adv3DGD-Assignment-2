using System;
using UnityEngine;


public class DialogueNPC : MonoBehaviour
{
    DialogueManager dialogueManager;
    QuestManager questManager;
    GameObject player;
    private bool isInRange;
    public bool isEligible = true;
    public bool hasBeenTalkedTo = false;
    
    public string characterName;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        questManager = FindObjectOfType<QuestManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        

        
    }

    private void OnGUI()
    {
        if (isInRange)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 100, 100, 20), "Press E to talk");
        }
    }

    private void Update()
    {

        if(!isEligible)
            return;
        
        if(Vector3.Distance(transform.position, player.transform.position) < 4)
        {
            isInRange = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueManager.StartDialogue(characterName);
            }
        }
        else
        {
            isInRange = false;
        }
        
    }
}
