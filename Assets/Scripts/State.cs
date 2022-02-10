using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : IState
{
    private string name;
    uint id;

    void Initialize()
    {

    }

    public void SetName(string Name)
    {
        name = Name;
    }

    public string GetName()
    {
        return name;
    }
}
