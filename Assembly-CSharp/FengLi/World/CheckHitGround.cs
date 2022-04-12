using UnityEngine;

public class CheckHitGround : MonoBehaviour
{
    public bool isGrounded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("EnemyAABB"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("EnemyAABB"))
        {
            isGrounded = true;
        }
    }
}
