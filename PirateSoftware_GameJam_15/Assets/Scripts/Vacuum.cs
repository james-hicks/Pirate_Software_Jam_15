using System.Collections;
using UnityEngine;

public class Vacuum : MonoBehaviour{
    [Header("Inverse Potion Effect")]
    public Color potionColor;
    
    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;

    [Space]
    [SerializeField] Transform rayPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _turnSpeed = 30f;

    private bool obstructedPath = false;
    private bool grounded = false;

    private IEnumerator _currentState;
    private Rigidbody _rb;
    private RaycastHit hit;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        ChangeState(MoveState());
    }

    private void Update()
    {
        grounded = CheckGrounded();
        obstructedPath = CheckObstacle();
    }

    private bool CheckGrounded()
    {
        if (Physics.Raycast(new Vector3(rayPoint.position.x, rayPoint.position.y, rayPoint.position.z), -rayPoint.up, out hit, 1f, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckObstacle()
    {
        RaycastHit wallHit;
        if (Physics.Raycast(new Vector3(rayPoint.position.x, rayPoint.position.y, rayPoint.position.z), rayPoint.forward, out wallHit, 0.3f, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChangeState(IEnumerator newState)
    {
        // end current state
        if (_currentState != null) StopCoroutine(_currentState);

        // assign a new current state, and start it
        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private IEnumerator MoveState() 
    {
        while(grounded && !obstructedPath)
        {
            _rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);


            yield return new WaitForEndOfFrame();
            int rand = Random.Range(0, 1000);
            if (rand == 0)
            {
                gameObject.transform.Rotate(0, _turnSpeed, 0);
            }


            if (hit.collider != null)
            {
                Paintable p = hit.collider.GetComponent<Paintable>();
                if (p != null)
                {
                    PaintManager.instance.paint(p, hit.point, radius, hardness, strength, potionColor);
                }
            }

        }
        ChangeState(TurnState());
    }

    private IEnumerator TurnState()
    {
        while(!grounded || obstructedPath)
        {
            int rand = Random.Range(0, 4);
            if(rand == 0)
            {
                gameObject.transform.Rotate(0, _turnSpeed, 0);
            }
            else if(rand == 1)
            {
                gameObject.transform.Rotate(0, -_turnSpeed, 0);
            }
            else if(rand == 2)
            {
                gameObject.transform.Rotate(0, 180, 0);
            }
            else
            {
                gameObject.transform.Rotate(0, 135, 0);
            }


            yield return new WaitForEndOfFrame();
        }
        ChangeState(MoveState());
    }
}
