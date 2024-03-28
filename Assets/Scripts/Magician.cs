using UnityEngine;
using UnityEngine.UI;

public class Magician : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float defense = 0.5f; // Defense modifier (0 to 1)
    public int maxHealth = 1000; // Maximum health
    private int currentHealth; // Current health
    public GameObject[] spells; // Array to store spell prefabs
    private int selectedSpell = 0; // Index of the currently selected spell
    public Text healthText; // Reference to UI text displaying health (optional)

    private GameObject area; // Reference to the are object

    private void Start()
    {
        currentHealth = maxHealth;
        // Find the cube object (assuming it has a specific tag or name)
        area = GameObject.FindGameObjectWithTag(StringConstants.Area); // Replace with your search method (e.g., by name)
        if (area == null)
        {
            Debug.Log("Cube object not found!"); // Handle the case where the cube is not found
        }

        GameManager.instance.ChangeHealth(maxHealth,maxHealth);
    }

    private void Update()
    {

        // Movement based on arrow keys with boundary checking
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;
        Vector3 desiredMovement = new Vector3(horizontal, 0f, vertical) * Time.deltaTime;
        transform.Translate(ClampMovementToBounds(desiredMovement));

        // Spell selection with Q and W keys
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedSpell = Mathf.Clamp(selectedSpell - 1, 0, spells.Length - 1); // Loop back to last spell if reached first
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedSpell = Mathf.Clamp(selectedSpell + 1, 0, spells.Length - 1);
        }

        // Spell casting with X key
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (spells[selectedSpell] != null)
            {
                Vector3 centerPosition = transform.position + transform.up ;

                // Instantiate the spell at the center position
                Instantiate(spells[selectedSpell], centerPosition, transform.rotation);

            }
        }
    }

    private Vector3 ClampMovementToBounds(Vector3 movement)
    {
        // Assuming your cube has a collider component with a specific size
        Bounds cubeBounds = area.GetComponent<Collider>().bounds; // Fix the reference
        float minX = cubeBounds.min.x + transform.localScale.x / 2f; // Offset by half magician size to avoid clipping
        float maxX = cubeBounds.max.x - transform.localScale.x / 2f;
        float minZ = cubeBounds.min.z + transform.localScale.z / 2f;
        float maxZ = cubeBounds.max.z - transform.localScale.z / 2f;

        // Clamp movement to stay within the cube's boundaries
        movement.x = Mathf.Clamp(movement.x, minX - transform.position.x, maxX - transform.position.x);
        movement.z = Mathf.Clamp(movement.z, minZ - transform.position.z, maxZ - transform.position.z);

        return movement;
    }
   
    public void TakeDamage(int damage)
    {
        Debug.Log("Magican Take Demage" + damage);
        currentHealth -= damage; // Apply defense modifier
       

        if (currentHealth <= 0)
        {
            GameManager.instance.LoseGame();
            Destroy(gameObject);
        }
        GameManager.instance.ChangeHealth(currentHealth,maxHealth);
        // Update health UI if applicable
        if (healthText != null)
        {
            healthText.text = $"{StringConstants.Health}: " + currentHealth;
        }
    }
}
