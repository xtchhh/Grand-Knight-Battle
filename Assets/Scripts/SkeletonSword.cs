using Unity.VisualScripting;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    public float damage = 20f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CrusaderFaction>())
        {
            collision.gameObject.GetComponent<CrusaderFaction>().health -= damage;
        }
    }
}
