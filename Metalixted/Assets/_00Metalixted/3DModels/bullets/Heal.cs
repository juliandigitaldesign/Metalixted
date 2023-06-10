using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        try {
            if (other.gameObject.tag == "Bullet")
            {
                playerController.DamageServerRpc();
                Destroy(other.gameObject);
            }
        } catch { return; }        
    }

}
