using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0F;
    [SerializeField] private Transform targetTransformForward;
    [SerializeField] private Transform targetTransformBackwards;

    private bool _isForward = true;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 position = Vector3.MoveTowards(_rb.position,
            _isForward ? targetTransformForward.position : targetTransformBackwards.position,
            speed * Time.fixedDeltaTime);

        _rb.MovePosition(position);
        
        Debug.Log(Vector3.Distance(_rb.position,
            _isForward ? targetTransformForward.position : targetTransformBackwards.position));
            
        if (Vector3.Distance(_rb.position,
            _isForward ? targetTransformForward.position : targetTransformBackwards.position) <= 1.0)
        {
            _isForward = !_isForward;
        }
    }
}