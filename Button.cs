using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public int changeAmount = -10;
    public FillBarScript bar;
    private void OnMouseDown()
    {
        bar.changeBarAmount(changeAmount);
    }
}
