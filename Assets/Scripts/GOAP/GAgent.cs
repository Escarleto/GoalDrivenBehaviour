using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public GInventory inventory = new GInventory();
    public WorldStates beliefs = new WorldStates();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;
    int lastPlanWorldVersion = -1;

    protected void Start()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach (GAction a in acts)
            actions.Add(a);
    }

    bool invoked = false;

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, transform.position);
            if (currentAction.agent.hasPath && distanceToTarget < 2)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }

            return;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePeform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if (currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
                else
                {
                    actionQueue = null;
                    planner = null;
                    currentAction.running = false;
                    currentAction = null;
                }
            }
            else
            {
                actionQueue = null;
                planner = null;
                currentAction.running = false;
                currentAction = null;
            }

            return;
        }

        if (actionQueue != null)
        {
            if (currentGoal.remove)
                goals.Remove(currentGoal);
            actionQueue = null;
            planner = null;
            currentAction = null;
            return;
        }

        WorldStates world = GWorld.Instance.GetWorld();
        if (planner == null || world.Version != lastPlanWorldVersion)
        {
            planner = new GPlanner();
            lastPlanWorldVersion = world.Version;
            
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.Plan(actions, sg.Key.sgoals, null);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }
    }
}
