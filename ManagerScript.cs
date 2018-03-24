using UnityEngine;

public class ManagerScript : MonoBehaviour {
    public GameObject[] majorAbilites;
    public LineRenderer lineObj;
    private LineRenderer[] Lines;

    private void Start()
    {
        Lines = new LineRenderer[majorAbilites.Length];
        drawToMajorAbilites();
    }
    void drawToMajorAbilites()
    {
        for(int i = 1; i < majorAbilites.Length; i++)
        {
            Lines[i] = Instantiate(lineObj);
            Lines[i].transform.SetParent(transform);
            Lines[i].SetPosition(0, majorAbilites[i-1].transform.position);
            Lines[i].SetPosition(1, majorAbilites[i].transform.position);
            Lines[i].startWidth = 0.1f;
        }
    }

}
