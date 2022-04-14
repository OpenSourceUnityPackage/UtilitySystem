using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UtilitySystem : MonoBehaviour
{
    [SerializeField]
    List<Action> actions = new List<Action>();
    public List<Action> Actions
    {
        get { return actions; }
    }


    [SerializeField]
    Blackboard bb;

    Action currentAction = null;

    public Action CurrentAction
    {
        get { return currentAction; }
    }



    private void Start()
    {
        if (currentAction != null)
        {
            currentAction.StartAction();
        }
    }

    private void Update()
    {
        float highestUtility = 0f;
        Action newAction = null;
        foreach (Action action in actions)
        {
            float utility = action.EvaluateUtility(bb);
            if (utility > highestUtility || (newAction != null && utility == highestUtility && action.priority > newAction.priority))
            {
                newAction = action;
                highestUtility = utility;
            }
        }

        if (newAction == null)
        {
            newAction = actions[Random.Range(0, actions.Count)];
        }

        if (newAction != currentAction)
        {
            if (currentAction  != null)
                currentAction.EndAction();

            currentAction = newAction;
            currentAction.StartAction();
        }

        // Call update atleast 1 time, even if StartAction() has been called during the same frame
        currentAction.UpdateAction();
    }
}
