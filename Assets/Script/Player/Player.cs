
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Components
    private Collider _collider;
    #endregion

    #region Movement
    [SerializeField] private Vector2 _moveDir;
    [SerializeField] private int _cellDistance;
    private bool _isMoving = false;
    #endregion

    private void Start()
    {
        _collider = GetComponent<Collider>();
        StartCoroutine(CheckLose());
    }

    private void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }

    private void Update()
    {
        if (GameManager.instance.IsThisGameFinalOrLose() || GameManager.instance.IsThisGameFinalOrWin()) return;

        if (_moveDir != Vector2.zero)
        {
            MovePlayer();
        }
    }

    //TODO: Check if player is moving
    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance.IsThisGameFinalOrLose() || GameManager.instance.IsThisGameFinalOrWin()) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Dead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.IsThisGameFinalOrLose() || GameManager.instance.IsThisGameFinalOrWin()) return;

        if (other.CompareTag("Destination"))
        {
            GameManager.instance.PlayerWinThisLevel();
        }
    }

    public void Dead()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<Renderer>().enabled = false;
        GameManager.instance.PlayerLoseThisLevelAndShowUI();
    }


    private void MovePlayer()
    {
        if (_isMoving)
        {
            _moveDir = Vector2.zero;
            return;
        }
        Vector3Int currentCell = GridCellManager.instance.GetCellPositionOfGivenPosition(transform.position);
        Vector3Int targetCell = currentCell;
        if (_moveDir.x > 0)
        {
            targetCell = currentCell + new Vector3Int(0, _cellDistance, 0);
        }
        else if (_moveDir.x < 0)
        {
            targetCell = currentCell + new Vector3Int(-_cellDistance, 0, 0);
        }
        else
        {
            _moveDir = Vector2.zero;
            return;
        }
        Vector3 targetPosition = GridCellManager.instance.GetWordPositionOfGivenCellPosition(targetCell);

        _isMoving = true;
        StartCoroutine(DisableCollider());
        transform.DOJump(targetPosition, 1.5f, 1, 0.4f).SetEase(Ease.InSine).OnComplete(() =>
        {
            _isMoving = false;
        });
        _moveDir = Vector2.zero;
    }

    private IEnumerator DisableCollider()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(0.35f);
        _collider.enabled = true;
    }

    private IEnumerator CheckLose()
    {
        yield return new WaitForSeconds(0.5f);
        while(true)
        {
            if (GameManager.instance.IsThisGameFinalOrLose() || GameManager.instance.IsThisGameFinalOrWin()) yield break;
            if (!GetComponent<Renderer>().isVisible || transform.position.y < -1)
            {
                GameManager.instance.PlayerLoseThisLevelAndShowUI();   
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public int GetCellDistance()
    {
        return _cellDistance;
    }
}
