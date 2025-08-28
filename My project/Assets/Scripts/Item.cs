using UnityEngine;
using UnityEngine.AI;

public class Item : MonoBehaviour, IItem
{
    public enum Types
    {
        Coin,
        Ammo,
        Health,
    }
    public Types itemType;
    public int value = 10;

    private void Update()
    {
        
    }

    public void Use(GameObject other)
    {
        switch (itemType)
        {
            case Types.Coin:
                {
                    var gm = other.GetComponent<GameManager>();
                    gm?.AddScore(value);
                }
                break;
            case Types.Ammo:
                {
                    var shooter = other.GetComponent<PlayerShooter>();
                    shooter?.gun?.AddAmmo(value);
                }
                break;
            case Types.Health:
                {
                    var playerHealth = other.GetComponent<PlayerHealth>();
                    playerHealth?.Heal(value);
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Use(other.gameObject);
            Destroy(gameObject);
        }
    }
}
