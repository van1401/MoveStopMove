using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] Vector3 offset;

    private void Awake()
    {
        if(playerTransform == null)
        {
            playerTransform = PlayerController.Instance.transform;
        }
        else { return; }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset, Time.deltaTime * 5f);
    }
}
