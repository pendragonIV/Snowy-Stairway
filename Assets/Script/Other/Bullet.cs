using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool _isCollided = false;
    private void OnTriggerEnter(Collider other)
    {
        if (_isCollided) return;
        _isCollided = true;
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        if (other.CompareTag("Player"))
        {
            GameManager.instance.player.GetComponent<Player>().Dead();
        }
        StartCoroutine(DestroyBullet());
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.5f);
        transform.DOKill();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (_isCollided) return;
        if(transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }
}
