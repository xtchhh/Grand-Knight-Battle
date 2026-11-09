using System.Runtime.CompilerServices;
using UnityEngine;

public class HitRadiusObject : MonoBehaviour
{
    private MeshRenderer mesh;
    private float disableTimer;
    private float disableTime = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.orangeRed;
    }

    // Update is called once per frame
    void Update()
    {
        HandleObjectInstance();
    }

    void HandleObjectInstance()
    {
        disableTimer += Time.deltaTime;

        if (disableTimer > disableTime)
        {
            Destroy(this.gameObject);
        }
    }
}
