using UnityEngine;

public class FillBarScript : MonoBehaviour
{
    public int maxBarAmount;
    public int minBarAmount;
    public int currentBarAmount;
    public GameObject barPixel;
    private GameObject[] points;
    private const int SCALE_OF_BAR = 10;
    void Awake()
    {
        currentBarAmount = maxBarAmount;
        points = new GameObject[maxBarAmount / SCALE_OF_BAR];
        fillBarWithAmount();
    }
    void fillBarWithAmount()
    {
        int toFill = currentBarAmount / SCALE_OF_BAR;
        for (int i = 0; i < toFill; i++)
        {
            Destroy(points[i]);
            points[i] = Instantiate(barPixel);
            points[i].transform.localPosition = transform.right * i;
            points[i].transform.SetParent(transform);
        }
    }
    private void clearPreviousBar()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Destroy(points[i]);
        }
    }
    public void changeBarAmount(int num)
    {
        if (currentBarAmount + num >= maxBarAmount)
        {
            currentBarAmount = maxBarAmount;
        } else if(currentBarAmount + num <= minBarAmount)
        {
            currentBarAmount = minBarAmount;
        }
        else
        {
            currentBarAmount += num;
        }
        clearPreviousBar();
        fillBarWithAmount();
    }
}
