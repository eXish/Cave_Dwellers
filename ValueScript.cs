using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueScript : MonoBehaviour
{
    protected int money;

    private void Start()
    {
        money = 300;
    }

    public int getValue()
    {
        return money;
    }
    public void setValue(int amount)
    {
        money = amount;
    }
}
