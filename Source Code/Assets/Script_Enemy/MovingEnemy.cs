using UnityEngine;

public class MovingEnemy : MonoBehaviour
{

    public GameObject platform;
    public float moveSpeed;
    private Transform currentPoint;
    public Transform[] points;
    public int pointSelection;

    void Start()
    {
        currentPoint = points[pointSelection];
    }

    void Update()
    {
        if (tag == "Enemy" || tag == "Trap")
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
            if (platform.transform.position == currentPoint.position)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                pointSelection++;
                if (pointSelection == points.Length)
                {
                    pointSelection = 0;
                }
            }
            currentPoint = points[pointSelection];
        }
        
    }
}

