using UnityEngine;

public class Spell : MonoBehaviour
{
    public float speed = 10f; // Movement speed of the fireball
    public int damage = 20; // Damage dealt on collision

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Move forward
    }
    private void Start()
    {
        Destroy(gameObject, 5f); // Destroys the spell after 5 seconds
    }

    public int GetDemageOnHitAndDestroySpell()
    {
        Destroy(gameObject);
        return damage;
    }
}