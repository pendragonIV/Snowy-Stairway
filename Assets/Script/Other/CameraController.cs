using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void FixedUpdate()
    {
        transform.position += Vector3.forward * _speed * Time.fixedDeltaTime;
    }


}
