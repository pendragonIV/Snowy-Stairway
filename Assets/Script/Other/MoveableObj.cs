using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObj : MonoBehaviour
{
    [SerializeField] private bool _isMoveable;
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;

    private void FixedUpdate()
    {
        if(_isMoveable)
        {
            Move();
        }
    }

    private void Move()
    {
        if(transform.position.y >= _distance)
        {
            _speed = -_speed;
        }
        else if(transform.position.y <= 0)
        {
            _speed = Mathf.Abs(_speed);
        }

        transform.position += Vector3.up * _speed * Time.deltaTime;
    }
}
