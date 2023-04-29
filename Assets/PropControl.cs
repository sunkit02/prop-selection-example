using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PropControl : MonoBehaviour
{
    public float displayCountDown = 0.1f;
    public GameObject marker = null;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (displayCountDown <= 0)
        {   
            if (marker != null) marker.SetActive(false);
        } else
        {
            displayCountDown -= Time.deltaTime;
        }

        if (marker != null) marker.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }

    public void DisplayMarker()
    {
        if (marker == null)
        {
            marker = Instantiate(Resources.Load<GameObject>("PropMarker"));
        }

        marker.SetActive(true);
        displayCountDown = 0.1f;
    }
}
