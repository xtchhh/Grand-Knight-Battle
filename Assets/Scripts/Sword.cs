using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damage = 20f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SkeletonTeam>())
        {
            collision.gameObject.GetComponent<SkeletonTeam>().health -= damage;
        }

        if (collision.gameObject.GetComponent<CrusaderFaction>())
        {
            collision.gameObject.GetComponent<CrusaderFaction>().health -= damage;
        }
    }
}
