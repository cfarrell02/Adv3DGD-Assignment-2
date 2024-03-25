using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class DialogueManager : MonoBehaviour
{
    private TextAsset dialogueXml; // Drag your XML file here in Unity inspector
    private Dictionary<string, List<Dialogue>> dialogues = new Dictionary<string, List<Dialogue>>();


    private Dialogue currentDialogue = null;
    private int currentDialogueIndex = 0;

    void Start()
    {
        dialogueXml = Resources.Load<TextAsset>("Dialogues");
        ParseDialogueXML();
    }

    private void OnGUI()
    {
        if (currentDialogue != null)
        {
            GUI.Box(new Rect(400, 400, 900, 200), currentDialogue.characterName + ": " + currentDialogue.dialogueContent);
            for (int i = 0; i < currentDialogue.responses.Count; i++)
            {
                string response = currentDialogue.responses[i].response;
                if(currentDialogueIndex == i)
                    response = response+ " <---";
                GUI.Label(new Rect(420, 450 + i * 20, 900, 20), response);
            }
        }
    }


    void ParseDialogueXML()
    {
        if (dialogueXml == null)
        {
            Debug.LogError("Dialogue XML file not found!");
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(dialogueXml.text);

        XmlNodeList characterNodes = xmlDoc.SelectNodes("//character");

        foreach (XmlNode characterNode in characterNodes)
        {
            string characterName = characterNode.Attributes["name"].Value;
            List<Dialogue> dialoguesList = new List<Dialogue>();
            
            XmlNodeList dialogueNodes = characterNode.SelectNodes("dialogue");
            foreach (XmlNode dialogueNode in dialogueNodes)
            {
                Dialogue dialogue = new Dialogue();
                dialogue.characterName = characterName;
                dialogue.dialogueId = Convert.ToInt32(dialogueNode.Attributes["id"].Value);
                dialogue.dialogueContent = dialogueNode.Attributes["content"].Value;
                dialogue.responses = new List<(string response, int target)>();

                XmlNodeList responseNodes = dialogueNode.SelectNodes("choice");
                foreach (XmlNode responseNode in responseNodes)
                {
                    dialogue.responses.Add((responseNode.Attributes["content"].Value, int.Parse(responseNode.Attributes["target"].Value)));
                }

                dialoguesList.Add(dialogue);
            }
            dialogues.Add(characterName, dialoguesList);
        }
    }
    
    
    public void StartDialogue(string characterName, int dialogueId = 0)
    {
        currentDialogue = dialogues[characterName].Find(d => d.dialogueId == dialogueId);
        
    }

    private void Update()
    {
        
        if (currentDialogue != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentDialogueIndex++;
                if (currentDialogueIndex >= currentDialogue.responses.Count)
                {
                    currentDialogueIndex = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                int nextDialogueId = currentDialogue.responses[currentDialogueIndex].target;
                if(nextDialogueId == -1)
                {
                    currentDialogue = null;
                    return;
                }
                if(nextDialogueId == -2)
                {
                    GameObject.Find(currentDialogue.characterName).GetComponent<DialogueNPC>().hasBeenTalkedTo = true;
                    currentDialogue = null;
                    return;
                }
                
                currentDialogue = dialogues[currentDialogue.characterName].Find(d => d.dialogueId == nextDialogueId);
                currentDialogueIndex = 0;
            }
        }
        
        
    }
}




public class Dialogue
{
    public string characterName;
    public int dialogueId;
    public string dialogueContent;
    public List<(string response, int target)> responses;
}
