﻿
using System.Collections.Generic;

public class Quest
{
    public string name;
    public string description;
    public List<QuestObjective> objectives;
    
    public Quest(string name, string description, List<QuestObjective> objectives)
    {
        this.name = name;
        this.description = description;
        this.objectives = objectives;
    }
}


public class QuestObjective
{
    public enum Action
    {
        GO,
        TAKE,
        TALK,
        GIVE
    }
    
    public string description;
    public Action action;
    public string target;
    public int xp;
    
    public QuestObjective(string description, string action, string target, int xp)
    {
        this.description = description;
        this.action = (Action)System.Enum.Parse(typeof(Action), action);
        this.target = target;
        this.xp = xp;
    }
}
