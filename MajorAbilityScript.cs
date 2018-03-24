using UnityEngine;

public class MajorAbilityScript : MonoBehaviour {
    bool unlocked;
    public Material unlockedMaterial;
    // Color of skill when unlocked
    public int cost;
    public GameObject[] minorAbilities;
    // Array of minor abilities that are connected to this ability
    public LineRenderer lineObj;
    // The lineobject that will be drawn between skills
    private LineRenderer[] Lines;
    // Array of prefabs that are GameObjects with a Line Renderer
    private void Start()
    {
        Lines = new LineRenderer[minorAbilities.Length];
        drawToMinorAbilites();
    }
    void drawToMinorAbilites()
    {
        for(int i = 0; i < minorAbilities.Length; i++)
        {
            Lines[i] = Instantiate(lineObj);
            Lines[i].transform.SetParent(transform);
            Lines[i].SetPosition(0, transform.position);
            Lines[i].SetPosition(1, minorAbilities[i].transform.position);
            Lines[i].startWidth = 0.05f;
        }
    }
    private void OnMouseDown()
    {
        ValueScript val = gameObject.GetComponentInParent(typeof(ValueScript)) as ValueScript;
        int amount = val.getValue();
        if(amount >= cost)
        {
            amount -= cost;
            abilityUnlocked();
        }
        val.setValue(amount);
    }
    private void abilityUnlocked()
    {
        unlocked = true;
        GetComponent<Renderer>().material = unlockedMaterial;
    }
}
