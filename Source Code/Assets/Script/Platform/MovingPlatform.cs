using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public GameObject platform;
    public float moveSpeed;
    private float initial_moveSpeed;
    private Transform currentPoint;
    private Vector3 initial_pos;
    public Transform[] points;
    public int pointSelection;
    public bool waitPlateform = false;
    public bool speedUpReturn = false;
    private bool move_plateform = false;

    void Start()
    {
        initial_moveSpeed = moveSpeed;
        initial_pos = platform.transform.position;
        currentPoint = points[pointSelection];
    }

    void Update()
    {
        if (platform.transform.position != initial_pos || move_plateform == true || waitPlateform == false)
            move();
    }

    private void move()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
        if (platform.transform.position == currentPoint.position)
        {
            pointSelection++;
            if (pointSelection == points.Length)
            {
                if (speedUpReturn)
                    moveSpeed = 10;
                pointSelection = 0;
            }
            else
            {
                if (speedUpReturn)
                    moveSpeed = initial_moveSpeed;
            }
        }
        currentPoint = points[pointSelection];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Player") && (platform.transform.position == initial_pos))
            move_plateform = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Player") && (platform.transform.position == initial_pos))
            move_plateform = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Player"))
            move_plateform = false;
    }
}

