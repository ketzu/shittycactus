using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float damping_time = 0.25f; 
    private Vector3 velocity = Vector3.zero;
    
    [SerializeField]
    private GameObject left_end;
    [SerializeField]
    private GameObject right_end;

    [SerializeField]
    private Vector2 bottom_left = Vector2.zero;
    [SerializeField]
    private Vector2 top_right = Vector2.one;

    private GameObject _player;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player == null)
        {
            Debug.LogError("CameraController: Could not find player!");
        }
        _camera = GetComponent<Camera>();
        if(_camera == null)
        {
            Debug.LogError("CameraController: Could not find camera!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 player_point = _camera.WorldToViewportPoint(_player.transform.position);

        // Is Player in Box? Yes -> Do nothing.
        if (player_point.x > bottom_left.x &&
            player_point.x < top_right.x &&
            player_point.y > bottom_left.y &&
            player_point.y < top_right.y)
            return;

        float clamp_x = Mathf.Clamp(player_point.x, bottom_left.x, top_right.x);
        float clamp_y = Mathf.Clamp(player_point.y, bottom_left.y, top_right.y);

        Vector3 delta = _player.transform.position - _camera.ViewportToWorldPoint(new Vector3(clamp_x, clamp_y, player_point.z));

        Vector3 left_check = _camera.WorldToViewportPoint(left_end.transform.position);
        Vector3 right_check = _camera.WorldToViewportPoint(right_end.transform.position);

        // limit x-axis to level borders
        if (delta.x < 0 && left_check.x >= 0)
            delta.x = 0;
        if (delta.x > 0 && right_check.x <= 1)
            delta.x = 0;

        Vector3 target_pos = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, target_pos, ref velocity, damping_time);
    }
}
