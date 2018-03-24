using UnityEngine;

public class FocusScript : MonoBehaviour {
    private bool isFocusing;
    private float focusEfficiency;
    private float focusCost;
    private float minFocusCostModifier;
    // minFocusCostModifier - The lowest number that the focusEfficiency can reach
    private float focusEfficiencyChangeRate;
    // The rate at which the focus cost multiplier changes
    public float minimumFocus;
    public float maximumFocus;
    private float currentFocus;
    private float focusRegenRate;
    private KeyCode focusKey;

    private void Awake()
    {
        isFocusing = false;
        focusEfficiency = 1f;
        minFocusCostModifier = 0.02f;
        focusEfficiencyChangeRate = 0.001f;
        currentFocus = maximumFocus;
        focusKey = KeyCode.LeftShift;
    }
    public void setFocusCost(int cost)
    {
        focusCost = cost;
    }
    public void changeFocusCost(int amount)
    {
        focusCost += amount;
    }
    void Update () {
		if(Input.GetKey(focusKey))
        {
            isFocusing = true;
        } else
        {
            isFocusing = false;
            focusEfficiency = 1f;
        }
        if(isFocusing)
        {
            if(focusEfficiency > minFocusCostModifier)
            {
                focusEfficiency -= focusEfficiencyChangeRate;
            }
            float calculatedFocusCost = focusEfficiency * focusCost;
            if(currentFocus - calculatedFocusCost < minimumFocus)
            {
                isFocusing = false;
                currentFocus = minimumFocus;
            } else
            {
                currentFocus -= calculatedFocusCost;
            }
        } else
        {
            if(currentFocus + focusRegenRate > maximumFocus)
            {
                currentFocus = maximumFocus;
            }
            else
            {
                currentFocus += focusRegenRate;
            }
        }
	}
}
