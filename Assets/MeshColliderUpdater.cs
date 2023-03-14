using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// mlc March 14, 2023

public class MeshColliderUpdater : MonoBehaviour
{
    // MLC Was here:
    public List<GameObject> _crackGOs;
    public bool _beginCheckingAtStart = true;
    public float _intervalToCheck = 0.5f; // Update collider of skin every 1/2s
    public float _stopUpdatingAfter = 20f; // Stop updating after 20 seconds.

    private void Start()
    {
        //MeshFilter mf = new MeshFilter();
        Debug.Assert(_crackGOs.Count == 0,
            "crackGOs need to be set by level designer");
        if (_beginCheckingAtStart)
            StartCoroutine(UpdateColliders());
    }

    public void BeginUpdatingCollider()
    {
        StartCoroutine(UpdateColliders());
    }

    IEnumerator UpdateColliders()
    {
        float timeLeft = _stopUpdatingAfter;
        do
        {
            yield return new WaitForSeconds(_intervalToCheck);
            foreach (GameObject g in _crackGOs)
            {
                UpdateColliderForSkinnedMeshRendererGO(g);
            }
            timeLeft -= _intervalToCheck;
        } while (timeLeft > 0);
        yield return null;
    }


    void UpdateColliderForSkinnedMeshRendererGO(GameObject go)
    {
        MeshCollider  meshCollider = go.GetComponent<MeshCollider>();
        SkinnedMeshRenderer skinnedMeshRenderer = go.GetComponent<SkinnedMeshRenderer>();
        Mesh mesh  = new Mesh();
        // See JoeStrout's Comment here:https://forum.unity.com/threads/how-to-update-a-mesh-collider.32467/
        skinnedMeshRenderer.BakeMesh(mesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;

        // If you "borrow" this code and need it for regular MeshRenderer,
        // you might get away with just updating the meshFilter instead of backing.
        // So, something like this, and menioned in above link, only works for regular MeshRenderers:
        //_meshFilter = go.GetComponent<MeshFilter>();
        //_meshCollider.sharedMesh =  _meshFilter.mesh;
    }

}
