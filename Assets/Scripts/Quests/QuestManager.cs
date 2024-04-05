using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class QuestManager : MonoBehaviour
{
    // XML file name
    private const string questFileName = "Quests";
    private List<Quest> quests = new List<Quest>();
    private Quest activeQuest;
    private QuestObjective currentObjective;
    private GameObject player, target;
    private float timer = 0;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Load XML file from Resources folder
        TextAsset questXml = Resources.Load<TextAsset>(questFileName);

        if (questXml == null)
        {
            Debug.LogError("Quest XML file not found!");
            return;
        }

        // Parse XML data
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(questXml.text);

        // Get all quest nodes
        XmlNodeList questNodes = xmlDoc.SelectNodes("//Quest");

        // Iterate through each quest
        foreach (XmlNode questNode in questNodes)
        {
            string questName = questNode.SelectSingleNode("Name").InnerText;
            string questDescription = questNode.SelectSingleNode("Description").InnerText;
            int questLevel = Convert.ToInt32(questNode.Attributes["level"].Value);

            var quest = new Quest(questName, questDescription, questLevel, new List<QuestObjective>());

            // Get all objectives for the current quest
            XmlNodeList objectiveNodes = questNode.SelectNodes("Objectives/Objective");

            // Iterate through each objective
            foreach (XmlNode objectiveNode in objectiveNodes)
            {
                string objectiveDescription = objectiveNode.SelectSingleNode("Description").InnerText;
                string action = objectiveNode.SelectSingleNode("Action").InnerText;
                string target = objectiveNode.SelectSingleNode("Target").InnerText;
                int xp = int.Parse(objectiveNode.SelectSingleNode("XP").InnerText);

                var objective = new QuestObjective(objectiveDescription, action, target, xp);
                quest.objectives.Add(objective);
            }
            quests.Add(quest);
        }
    }

    private void OnGUI()
    {
        if (activeQuest != null)
        {
            GUI.Label(new Rect(Screen.width / 2-50, 40, 500, 20), "Quest: " + activeQuest.name);
            GUI.Label(new Rect(Screen.width / 2-100, 60, 500, 20), "Objective: " + currentObjective.description);
        }
    }

    void Update()
    {
        if (activeQuest != null)
        {
            ManageObjectives();
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > 5)
            {
                var validQuests = quests.Where(q => q.level == SceneManager.GetActiveScene().buildIndex).ToList();
                if (validQuests.Count <= 0)
                {
                    GameManager.Instance.NextLevel();
                    return;
                }
                
                StartQuest();
                timer = 0;
            }
        }
    }
    
    public string GetCurrentTarget()
    {
        return currentObjective != null ? currentObjective.target : "";
    }
    
    public void ManageObjectives()
    {
        target = GameObject.Find(currentObjective.target); // Assuming the targets are in the world
        var inventory = player.GetComponent<Inventory>();

        switch (currentObjective.action)
        {
            case QuestObjective.Action.GO:
                if (Vector3.Distance(player.transform.position, target.transform.position) < 5)
                {
                    Debug.Log("Objective completed: " + currentObjective.description);
                    var index = activeQuest.objectives.IndexOf(currentObjective);
                    inventory.AddXP(currentObjective.xp);
                    if (index + 1 < activeQuest.objectives.Count)
                    {
                        currentObjective = activeQuest.objectives[index + 1];
                        Debug.Log("Objective: " + currentObjective.description);
                    }
                    else
                    {
                        Debug.Log("Quest completed: " + activeQuest.name);
                        activeQuest = null;
                        currentObjective = null;
                    }
                }

                break;
            case QuestObjective.Action.TAKE:
                if (inventory.Contains(target.name))
                {
                    Debug.Log("Objective completed: " + currentObjective.description);
                    var index = activeQuest.objectives.IndexOf(currentObjective);
                    inventory.AddXP(currentObjective.xp);
                    if (index + 1 < activeQuest.objectives.Count)
                    {
                        currentObjective = activeQuest.objectives[index + 1];
                        Debug.Log("Objective: " + currentObjective.description);
                    }
                    else
                    {
                        Debug.Log("Quest completed: " + activeQuest.name);
                        activeQuest = null;
                        currentObjective = null;
                    }
                }

                break;
            case QuestObjective.Action.TALK:
                // TODO: Implement talking to NPCs
                var npc = target.GetComponent<DialogueNPC>();
                if (npc.hasBeenTalkedTo)
                {
                    Debug.Log("Objective completed: " + currentObjective.description);
                    var index = activeQuest.objectives.IndexOf(currentObjective);
                    if (index + 1 < activeQuest.objectives.Count)
                    {
                        currentObjective = activeQuest.objectives[index + 1];
                        Debug.Log("Objective: " + currentObjective.description);
                    }
                    else
                    {
                        Debug.Log("Quest completed: " + activeQuest.name);
                        activeQuest = null;
                        currentObjective = null;
                    }
                }

                break;
        }

        
    }
  
    
    
    public void StartQuest()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex ;
        
        activeQuest = quests.Find(q => q.level == currentLevel);
        //Remove the quest from the list so it doesn't get repeated
        quests.Remove(activeQuest);
        
        
        Debug.Log("Quest started: " + activeQuest.name);
        currentObjective = activeQuest.objectives[0];
        Debug.Log("Objective: " + currentObjective.description);
        
        var targetsInQuest = activeQuest.objectives.Where(o => o.action == QuestObjective.Action.TALK).Select(o => o.target).ToList();
        
        foreach (var target in targetsInQuest)
        {
            var npc = GameObject.Find(target).GetComponent<DialogueNPC>();
            if(npc!=null)
                npc.isEligible = true;
        }
    }
}

