using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]

    private Transform targetToFollow;

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(targetToFollow.position.x, -2.25f, 38.20f),
                                         Mathf.Clamp(targetToFollow.position.y, 2.30f, 6.00f),
                                         transform.position.z);
    }
}
