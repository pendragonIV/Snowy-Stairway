using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveType
{
    Horizontal,
    Vertical
}

public class Enemy : MonoBehaviour
{
    private int _cellDistance = 3;
    [SerializeField] private MoveType _moveType;
    [SerializeField] private int _moveRange;
    private Vector3Int _startCellPosition;
    private Vector3Int _targetNegCellPosition;
    private Vector3Int _targetPosCellPosition;
    private bool _isMoving = false;


    private void Start()
    {
        _startCellPosition = GridCellManager.instance.GetCellPositionOfGivenPosition(transform.position);
        if(_moveType == MoveType.Vertical)
        {
            _targetNegCellPosition = _startCellPosition - new Vector3Int(0, _moveRange * _cellDistance, 0);
            _targetPosCellPosition = _startCellPosition + new Vector3Int(0, _moveRange * _cellDistance, 0);
            transform.DORotate(new Vector3(0, 45, 0), .3f);
        }
        else
        {
            _targetNegCellPosition = _startCellPosition - new Vector3Int(_moveRange * _cellDistance, 0, 0);
            _targetPosCellPosition = _startCellPosition + new Vector3Int(_moveRange * _cellDistance, 0, 0);
            transform.DORotate(new Vector3(0, 135, 0), .3f);
        }
        StartCoroutine(MoveEnemy());
    }

    private IEnumerator MoveEnemy()
    {
        while (true)
        {
            if (!_isMoving)
            {
                Move();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player>().Dead();
        }
    }

    private void Move()
    {
        Vector3Int cellPosition = GridCellManager.instance.GetCellPositionOfGivenPosition(transform.position);
        Vector3Int nextCell = cellPosition;
        Vector3 nextPosition = transform.position;
        if (_moveType == MoveType.Horizontal)
        {
            if (cellPosition.x <= _targetNegCellPosition.x || cellPosition.x >= _targetPosCellPosition.x)
            {
                _cellDistance *= -1;
                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 180, 0), .3f);
            }
            nextCell = cellPosition + new Vector3Int(_cellDistance, 0, 0);
            nextPosition = GridCellManager.instance.GetWordPositionOfGivenCellPosition(nextCell);
        }
        else
        {
            if (cellPosition.y <= _targetNegCellPosition.y || cellPosition.y >= _targetPosCellPosition.y)
            {
                _cellDistance *= -1;
                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 180, 0), .3f);
            }
            nextCell = cellPosition + new Vector3Int(0, _cellDistance, 0);
            nextPosition = GridCellManager.instance.GetWordPositionOfGivenCellPosition(nextCell);
        }

        if (!PlatformAt(nextCell)) return;

        _isMoving = true;
        transform.DOJump(nextPosition, 0.5f, 1, .5f).OnComplete(() =>
        {
            _isMoving = false;
        });
    }

    private GameObject PlatformAt(Vector3Int cellPosition)
    {
        return GridCellManager.instance.GetPlatformAt(cellPosition);
    }
}
