using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisableCloude());
        }
    }

    private IEnumerator DisableCloude()
    {
        yield return new WaitForSeconds(.5f);
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            transform.DOKill();
            Destroy(gameObject);
        });
    }
}
