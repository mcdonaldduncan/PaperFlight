using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{ 
    private List<State> states;

    private State activeState;

    // Start is called before the first frame update
    void Start()
    {
        states = new List<State>();

        /* basic default states. When we add more, add them here */
        AddState("Idle");
        AddState("Flying");
        AddState("Turning");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetActiveState(string Name)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].GetName() == Name)
            {
                activeState = states[i];
                return;
            }
        }
        Debug.LogError($"Cannot set {Name}: state does not exist");
    }

    public State GetActiveState()
    {
        return activeState;
    }

    public State GetStateAtIndex(int index)
    {
        return states[index];
    }

    public void AddState(string Name)
    {
        // check for duplicate
        for(int i = 0; i < states.Count; i++)
        {
            if(states[i].GetName() == Name)
            {
                Debug.LogError($"Cannot add {Name}: state already exists");
                return;
            }
        }

        State newState = new State();
        newState.SetName(Name);
        states.Add(newState);
    }

    public void RemoveState(string Name)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].GetName() == Name)
            {
                states.RemoveAt(i);
                return;
            }
        }
        Debug.LogError($"Cannot remove {Name}: State does not exist");
    }
}
