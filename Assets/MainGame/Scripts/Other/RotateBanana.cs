using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBanana : MonoBehaviour
{
    public float RotSpeed;
    void Update()
    {
        transform.Rotate(RotSpeed * Time.deltaTime * Vector3.forward);
    }
}
