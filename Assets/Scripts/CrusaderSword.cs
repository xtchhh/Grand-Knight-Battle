using Unity.VisualScripting;
using UnityEngine;

public class CrusaderSword : MonoBehaviour
{
    public float damage = 20f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SkeletonTeam>())
        {
            collision.gameObject.GetComponent<SkeletonTeam>().health -= damage;
        }
    }
}
