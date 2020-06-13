using UnityEngine;

public class MoveCloud : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = this.GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(-speed, 0);
    }

    void Update()
    {
        if (transform.position.x < -16)
            Destroy(this.gameObject);
    }
}
