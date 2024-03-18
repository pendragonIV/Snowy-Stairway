using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _bulletLifeTime;

    private float _nextFireTime;

    private void Start()
    {
    }

    private void Update()
    {
        if (GameManager.instance.IsThisGameFinalOrLose() || GameManager.instance.IsThisGameFinalOrWin()) return;
        if (!transform.GetChild(0).GetComponent<Renderer>().isVisible) return;
        if (Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + _fireRate;
            Fire(2);
        }
    }

    private Vector3 GetShootPosition()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3Int playerCell = GridCellManager.instance.GetCellPositionOfGivenPosition(playerPos);
        int randomDir = Random.Range(0, 3);
        int cellDistance = GameManager.instance.player.GetComponent<Player>().GetCellDistance();
        Vector3 bulletPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(playerCell);

        if (randomDir == 0)
        {
            playerCell.x -= cellDistance;
            bulletPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(playerCell);
            if (CheckGround(bulletPos) == null)
            {
                playerCell.x += cellDistance;
                playerCell.y += cellDistance;
                bulletPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(playerCell);
            }
        }
        else if(randomDir == 1)
        {
            playerCell.y += cellDistance;
            bulletPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(playerCell);
            if (CheckGround(bulletPos) == null)
            {
                playerCell.y -= cellDistance;
                playerCell.x -= cellDistance;
                bulletPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(playerCell);
            }
        }
        return bulletPos;
    }

    private void Fire(int numb)
    {
        for (int i = 0; i < numb; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            Vector3 shootPos = GetShootPosition();
            bullet.transform.DOJump(shootPos, 10, 1, 3).OnStart(() =>
            {
                Vector3 aimRotation = new Vector3(270, 0, 0);
                bullet.transform.DORotate(aimRotation, 2f, RotateMode.FastBeyond360);
            });
        }
    }

    private GameObject CheckGround(Vector3 pos)
    {
        Collider[] intersecting = Physics.OverlapSphere(pos, 0.01f, LayerMask.GetMask("Platform"));
        if (intersecting.Length != 0)
        {
            return intersecting[0].gameObject;
        }
        return null;
    }

}
