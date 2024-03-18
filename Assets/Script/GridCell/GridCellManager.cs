using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void SetMapForThisManager(Tilemap tilemap)
    {
        if (tilemap != null)
        {
            this.tileMap = tilemap;
        }
    }

    public Vector3Int GetCellPositionOfGivenPosition(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 GetWordPositionOfGivenCellPosition(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }

    public GameObject GetPlatformAt(Vector3Int cellPosition)
    {
        Vector3 worldPosition = GetWordPositionOfGivenCellPosition(cellPosition);
        worldPosition.y -= 2f;
        RaycastHit hit;
        Physics.Raycast(worldPosition,Vector2.up, out hit, 2f);
        Debug.DrawRay(worldPosition, Vector2.up * 2, Color.red, Mathf.Infinity);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
