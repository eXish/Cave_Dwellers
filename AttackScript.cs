using UnityEngine;

public class AttackScript : MonoBehaviour {
    public GameObject spawnObj;
    private GameObject swing;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (swing == null)
            {
                getDirectionOfMouse();
            }
            else
            {
                Destroy(swing);
                getDirectionOfMouse();
            }
        }
    }
    private void getDirectionOfMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Get x
        float x_difference = ray.direction.x - ray.origin.x;
        // Get y
        float y_difference = ray.direction.y - ray.origin.y;
        // Compare the absolute values to find the greater one
        if(Mathf.Abs(x_difference) > Mathf.Abs(y_difference))
        {
            if(x_difference < 0)
            {
                // Right swing
                swing = Instantiate(spawnObj, transform.position, transform.rotation, transform);
                swing.transform.Translate(transform.localScale.x*3/4, 0f, 0f);
            } else if(x_difference > 0)
            {
                // Left swing
                swing = Instantiate(spawnObj, transform.position, transform.rotation, transform);
                swing.transform.Translate(transform.localScale.x * -3 / 4, 0f, 0f);
            }
        } else if(Mathf.Abs(x_difference) < Mathf.Abs(y_difference))
        {
            if (y_difference < 0)
            {
                // Up swing
                swing = Instantiate(spawnObj, transform.position, transform.rotation, transform);
                swing.transform.Translate(0f, transform.localScale.y * 3 / 4, 0f);
            }
            else if (y_difference > 0)
            {
                // Down swing
                swing = Instantiate(spawnObj, transform.position, transform.rotation, transform);
                swing.transform.Translate(0f, transform.localScale.y * -3 / 4, 0f);
            }
        } else if(x_difference > 0 && y_difference > 0)
        {
            //Lower Left - make it a left swing
            swing = Instantiate(spawnObj, transform.position, transform.rotation, transform);
            swing.transform.Translate(transform.localScale.x * -3 / 4, 0f, 0f);
        } else if(x_difference > 0 && y_difference < 0)
        {
            //Upper Left - make it a up swing
            swing = Instantiate(spawnObj, transform.position, transform.rotation, transform);
            swing.transform.Translate(0f, transform.localScale.y * 3 / 4, 0f);
        } else if(x_difference < 0 && y_difference > 0)
        {
            //Lower Right - make it a down swing
            swing = Instantiate(spawnObj, transform.position, transform.rotation, transform);
            swing.transform.Translate(0f, transform.localScale.y * -3 / 4, 0f);
        } else if(x_difference < 0 && y_difference < 0)
        {
            //Upper Right - make it a right swing
            swing = Instantiate(spawnObj, transform.position, transform.rotation, transform);
            swing.transform.Translate(transform.localScale.x * 3 / 4, 0f, 0f);
        }
        else
        {
            // Do nothing, because the click occured on the player object
        }
    }
}
