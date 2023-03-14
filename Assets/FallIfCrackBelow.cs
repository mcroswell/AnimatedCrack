using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallIfCrackBelow : MonoBehaviour
{
    public LayerMask _layerMask;
    public Rigidbody _rigidbody;
    public int _layerNumber;
    public float _upOffset = 10f;
    public float _distanceOfCast = 100f;
    public float _checkInterval = 0.25f;
    public bool _beginAtStart = true;
    public bool _destroyAfterFallTime = true;
    public float _timeToDestroy = 5f;

    public enum FallerState { inPlace, falling, gone }
    public FallerState _state = FallerState.inPlace;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _layerNumber = _layerMask.value;
        if (_beginAtStart)
        {
           Invoke("CheckForCrackFall", 1f);
        }
    }

    public void CheckForCrackFall()
    {
        StartCoroutine(CheckToFall());
    }

    IEnumerator CheckToFall()
    {
        do
        {
            Vector3 rayOrigin = transform.position + (Vector3.up * _upOffset);
            bool hit = Physics.Raycast(new Ray(rayOrigin, -Vector3.up), _distanceOfCast, _layerMask);
            if (hit)
            {
                print("Hit");
                _state = FallerState.falling;
                _rigidbody.isKinematic = false;
                break;
            }
            else
            {
                print("Checking");
            }
            yield return new WaitForSeconds(_checkInterval);
        } while (true);

        if (_state == FallerState.falling && _destroyAfterFallTime)
        {
            Destroy(gameObject, _timeToDestroy);
            print("Destroying");
            _state = FallerState.gone;
        }

        yield return null;
    }

}
