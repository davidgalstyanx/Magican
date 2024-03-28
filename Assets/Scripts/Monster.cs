using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum MonsterType { Melee, Ranged }; // Define monster types (optional)
    public MonsterType type; // Assign type to each monster prefab (optional)
    public float speed = 2f; // Movement speed
    public int attackDamage = 1; // Damage dealt on collision (melee)
    public int maxHealth = 50; // Maximum health
    private int currentHealth; // Current health
    public Transform target; // Reference to the magician (assuming one)
    public GameObject projectilePrefab; // Prefab for ranged attacks (optional)
    Bounds cubeBounds;
    public GameObject area;

    public bool activated = false;

    void Start()
    {
        currentHealth = maxHealth;
        // Find the magician object (assuming there's only one)
        target = GameObject.FindGameObjectWithTag(StringConstants.Magician).transform;
        area = GameObject.FindGameObjectWithTag(StringConstants.Area);
        cubeBounds = area.GetComponent<Collider>().bounds;
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Contains(StringConstants.Magician)) // Check for collision with monster tag
        {
            collision.gameObject.GetComponent<Magician>().TakeDamage(attackDamage); // Deal damage to the monster
        }
        else if (collision.gameObject.tag.Contains(StringConstants.Spell)) // Check for collision with monster tag
        {
            TakeDamage(collision.gameObject.GetComponent<Spell>().GetDemageOnHitAndDestroySpell()); // Deal damage to the monster
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag.Contains(StringConstants.Magician)) // Check for collision with monster tag
        {
            collision.gameObject.GetComponent<Magician>().TakeDamage(attackDamage); // Deal damage to the monster
        }
    }

    private void Update()
    {

        if (type == MonsterType.Melee) // Melee behavior
        {
            MoveTowardsTarget();
        }

        // Check for collision with spells (assuming spells have colliders)
        //if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 10f)) // Adjust detection range as needed
        //{
        //    if (hit.collider.gameObject.tag == StringConstants.Spell) // Check for spell tag
        //    {
        //        TakeDamage(hit.collider.gameObject.GetComponent<Spell>().damage); // Get damage from SpellScript
        //        Destroy(hit.collider.gameObject); // Destroy the spell on collision

        //    }

        //}
    }

    void MoveTowardsTarget()
    {
        if(!target|| !transform)
        {
            return;
        }

        Vector3 moveDirection = target.position - transform.position;
        float distance = moveDirection.magnitude;

        // If within a close distance, don't move
        if (distance < 0.1f) return;

        // Normalize direction and apply speed
        moveDirection.Normalize();
        moveDirection *= speed * Time.deltaTime;

        // Clamp movement to stay within cube bounds
        Vector3 clampedMovement = ClampMovementToBounds(moveDirection, cubeBounds);

        // Move the monster
        transform.Translate(clampedMovement);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Monster Take Demage" + damage);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Destroy the monster
            Destroy(gameObject);
            if (!GameManager.instance.MonsterDefeated())
            {
                GameController.instance.SpawnMonsters();
            }
        }
    }


    Vector3 ClampMovementToBounds(Vector3 movement, Bounds cubeBounds)
    {
        float minX = cubeBounds.min.x + transform.localScale.x / 2f; // Offset by half monster size to avoid clipping
        float maxX = cubeBounds.max.x - transform.localScale.x / 2f;
        float minZ = cubeBounds.min.z + transform.localScale.z / 2f;
        float maxZ = cubeBounds.max.z - transform.localScale.z / 2f;

        // Clamp movement to stay within the cube's boundaries
        movement.x = Mathf.Clamp(movement.x, minX - transform.position.x, maxX - transform.position.x);
        movement.z = Mathf.Clamp(movement.z, minZ - transform.position.z, maxZ - transform.position.z);

        return movement;
    }
}
