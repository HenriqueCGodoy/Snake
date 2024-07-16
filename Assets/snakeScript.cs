using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class snakeScript : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();

    [SerializeField]
    private GameObject _segmentPrefab;

    [SerializeField]
    private int _initialSize = 4;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        //Get the direction of movement
        if(Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        //starting from the last segment, move each to the previous segment's position
        for (int i = _segments.Count-1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        //Move the head
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + _direction.x,
            Mathf.Round(transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        //Instantiate and add a new segment to the segment list
        Transform newSegment = Instantiate(_segmentPrefab).transform;
        newSegment.position = _segments[_segments.Count - 1].position;

        _segments.Add(newSegment);
    }

    private void ResetState()
    {
        //Destroy all segments
        for(int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        //Clear the segments list and add the head
        _segments.Clear();
        _segments.Add(transform);

        //Add the initial size 
        for(int i = 1; i < _initialSize; i++)
        {
            _segments.Add(Instantiate(_segmentPrefab).transform);
        }

        //Reset head position to the start (0,0)
        transform.position = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        }
        else if(other.tag == "Obstacle")
        {
            ResetState();
        }
    }
}
