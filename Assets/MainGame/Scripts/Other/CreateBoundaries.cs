using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoundaries : MonoBehaviour
{
    public GameObject objTopRight;
    public GameObject objDownLeft;

    private void Update()
    {
        SetupBoundaries();
    }
    void SetupBoundaries()
    {
        Vector3 point = new Vector3();

        point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
        objTopRight.transform.position = point;

        point = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        objDownLeft.transform.position = point;
    }
}
