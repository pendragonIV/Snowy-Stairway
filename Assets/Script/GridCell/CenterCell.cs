using UnityEngine;

public class CenterCell : MonoBehaviour
{
    private void Start()
    {
        PutObjectToCenterCell();
    }
    public void PutObjectToCenterCell()
    {
        Vector3Int cellPos = GridCellManager.instance.GetCellPositionOfGivenPosition(transform.position);
        this.transform.position = GridCellManager.instance.GetWordPositionOfGivenCellPosition(cellPos);
    }
}
