using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField]

    private Transform targetToFollow;

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(targetToFollow.position.x, -1.60f, 65.40f),
                                         Mathf.Clamp(targetToFollow.position.y, 1.75f, 3.00f),
                                         transform.position.z);
    }
}
