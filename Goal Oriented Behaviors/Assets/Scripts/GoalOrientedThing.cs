using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalOrientedThing : MonoBehaviour
{
    public float tickDelay;

    Goal[] goals;
    int[] values;
    Action[] actions;
    float currentDiscontent = 0;

    // Start is called before the first frame update
    void Start()
    {
        goals = new Goal[4];
        values = new int[4];
        actions = new Action[5];

        values[0] = 5;
        values[1] = 5;
        values[2] = 5;
        values[3] = 5;

        goals[0] = new Goal("Hunger", 2);
        goals[1] = new Goal("Fatigue", 2);
        goals[2] = new Goal("Work", 3);
        goals[3] = new Goal("Boredom", 3);

        List<Goal> stuff = new List<Goal> { new Goal("Work", -3)};
        actions[0] = new Action("procrastinate", stuff);
        stuff = new List<Goal> { new Goal("Hunger", 3), new Goal("Boredom", -5)};
        actions[1] = new Action("kill some zombies", stuff);
        stuff = new List<Goal> { new Goal("Work", -5), new Goal("Fatigue", 3) };
        actions[2] = new Action("do some actual work", stuff);
        stuff = new List<Goal> { new Goal("Hunger", -2)};
        actions[3] = new Action("eat", stuff);
        stuff = new List<Goal> { new Goal("Fatigue", -3) };
        actions[4] = new Action("sleep", stuff);
        InvokeRepeating("Tick", 0f, tickDelay);
    }

    void Tick()
    {
        Debug.Log("I decided not to be lazy, so I'm going to do something");
        Debug.Log("    Right now my discontent is: " + currentDiscontent);
        string output = "    My needs right now are: " + goals[0].name + ": " + values[0];
        for (int i = 1; i < goals.Length; ++i)
        {
            output += ", " + goals[i].name + ": " + values[i];
        }
        Debug.Log(output);

        float lowest = Mathf.Infinity;
        int index = -1;

        for (int i = 0; i < actions.Length; ++i)
        {
            float test = discontent(actions[i]);
            if (test < lowest)
            {
                lowest = test;
                index = i;
            }
        }

        for (int i = 0; i < goals.Length; ++i)
        {
            int number = actions[index].GetGoalChange(goals[i]);
            values[i] = Mathf.Max(values[i] + (number != 0 ? number : goals[i].value), 0);
        }
        currentDiscontent = lowest;

        Debug.Log("I decided to " + actions[index].name);
        Debug.Log("    Now my discontent is: " + currentDiscontent);
    }

    float discontent(Action a)
    {
        int discontent = 0;

        for (int i = 0; i < goals.Length; ++i)
        {
            int goalChange = a.GetGoalChange(goals[i]);
            int value = Mathf.Max(values[i] + (goalChange != 0 ? goalChange : values[i]), 0);
            discontent +=  value * value;
        }

        return discontent;
    }
}
