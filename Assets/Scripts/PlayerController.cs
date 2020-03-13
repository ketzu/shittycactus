using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent die;
    public UnityEvent jump;
    public UnityEvent landing;
    public UnityEvent headhitting;
    public UnityEvent FinalReached;

    private float _terminal_distance;

    public float JumplengthAfterDip { get { return jumplength_after_dip; } }
    public float JumplengthWithButton { get { return jumplength_with_button; } }

    public float YVelocity { get { return vertical_velocity; } }
        
    [SerializeField]
    private float jumplength_with_button = 1f;
    [SerializeField]
    private float jumplength_without_button = 1f;
    [SerializeField]
    private float jumplength_after_dip = 1f;

    private float horizontal_velocity = 0f;
    private float vertical_velocity = 0f;

    private bool was_suspended = false;
    private bool is_grounded = false;
    private bool is_hitceiling = false;
    private Vector2 _ground_normal;

    private Animator _animator;

    private Rigidbody2D _rb;
    private ContactFilter2D _contact_filter;

    private const float _min_move_distance = 0.001f;
    private const float _shell_radius = 0.01f;

    [SerializeField]
    private float min_ground_normal_y = 0.65f;

    private RaycastHit2D[] _hitbuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> _hitbuffer_list = new List<RaycastHit2D>(16);

    private Camera _camera;

    private bool _do_jump = false;
    private bool _is_dead = false;

    private float _top_height = -10f;

    private float _maxspeed;
    private float _inair_damping;
    private float _jumpheight;

    [SerializeField]
    private LayerMask passthrough_upwards;

    [SerializeField]
    private UpgradeObject jumpheight_upgrade;
    [SerializeField]
    private UpgradeObject speed_upgrade;
    [SerializeField]
    private UpgradeObject airdamping_upgrade;
    [SerializeField]
    private UpgradeObject deathdepth_upgrade;

    [SerializeField]
    private int final_unlock;

    public float GoalHeight { get { return final_unlock; } }
    public float Speed { get { return _maxspeed; } }
    public float JumpHeight { get { return _jumpheight; } }

    private void Start()
    {
        Cursor.visible = false;

        _jumpheight = jumpheight_upgrade.getValue();
        _maxspeed = speed_upgrade.getValue();
        _inair_damping = airdamping_upgrade.getValue();
        _terminal_distance = deathdepth_upgrade.getValue();

        _contact_filter.useTriggers = false;
        _contact_filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contact_filter.useLayerMask = true;

        _rb = GetComponent<Rigidbody2D>();

        _animator = GetComponentInChildren<Animator>();

        _camera = Camera.main;

        landing.AddListener(isDieOnLanding);
        die.AddListener(onDeath);
    }

    private void isDieOnLanding()
    {
        if (_top_height-transform.position.y > _terminal_distance)
        {
            die.Invoke();
        }
        _top_height = transform.position.y;
    }

    private void onDeath()
    {
        _is_dead = true;
        _animator.SetBool("dead", true);
        _animator.SetTrigger("death");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            _do_jump = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y > final_unlock && PlayerPrefs.GetInt("Final",0) == 0)
        {
            PlayerPrefs.SetInt("Final", 1);
            FinalReached.Invoke();
        }
        if (!_is_dead)
        {
            _top_height = Mathf.Max(_top_height, transform.position.y);

            float h_move = horizontal_movement();
            float v_move = vertical_movement();

            _animator.SetFloat("speed", Mathf.Abs(h_move));
            easteregg();

            is_grounded = false;


            Vector2 ground_aligned_move = new Vector2(_ground_normal.y, -_ground_normal.x) * h_move;
            Move(ground_aligned_move * Time.deltaTime, false);

            Move(new Vector2(0, v_move) * Time.deltaTime, true);
        }
    }

    private void easteregg()
    {
        if (Input.GetButton("Wave"))
        {
            _animator.SetTrigger("wave");
        }
    }

    private void Move(Vector2 movement, bool move_y)
    {
        float distance = movement.magnitude;

        if (distance > _min_move_distance)
        {
            int count = _rb.Cast(movement, _contact_filter, _hitbuffer, distance+_shell_radius);

            _hitbuffer_list.Clear();
            for(int i=0; i<count; i+=1)
            {
                if (passthrough_upwards != (passthrough_upwards | (1 << _hitbuffer[i].transform.gameObject.layer)))
                {
                    _hitbuffer_list.Add(_hitbuffer[i]);
                }
                else
                {
                    if (movement.y < 0f && move_y && _hitbuffer[i].normal == Vector2.up)
                    {
                        var ssc = _hitbuffer[i].transform.gameObject.GetComponent<SafeStopController>();
                        if (ssc != null)
                            ssc.hit.Invoke();
                        _hitbuffer_list.Add(_hitbuffer[i]);
                    }
                }
            }

            foreach (var hit in _hitbuffer_list)
            {
                var current_normal = hit.normal;

                if (hit.normal.y > min_ground_normal_y)
                {
                    is_grounded = true;
                    if (was_suspended)
                    {
                        was_suspended = false;

                        landing.Invoke();
                    }
                    if (move_y)
                    {
                        _ground_normal = hit.normal;
                        current_normal.x = 0;
                    }
                }else if(hit.normal.y < -min_ground_normal_y && move_y)
                {
                    headhitting.Invoke();
                    vertical_velocity = 0f;
                }

                float projection = Vector2.Dot(movement, current_normal);
                if (projection < 0)
                {
                    vertical_velocity -= projection * current_normal.y;
                }

                float modified_distance = hit.distance - _shell_radius;
                distance = Mathf.Min(distance, modified_distance);
            }

            if (!is_grounded && move_y)
            {
                was_suspended = true;
            }
        }

        var tmp_pos = _rb.position + movement.normalized * distance;
        
        // Camera correction
        var lower_end = _camera.ViewportToWorldPoint(Vector3.zero);
        var upper_end = _camera.ViewportToWorldPoint(Vector3.one);

        tmp_pos.x = Mathf.Clamp(tmp_pos.x,lower_end.x,upper_end.x);
        //tmp_pos.y = Mathf.Clamp(tmp_pos.y, lower_end.y, upper_end.y);

        _rb.position = tmp_pos;
    }

    float vertical_movement()
    {
        if (is_hitceiling)
        {
            vertical_velocity = 0f;
        }
        
        if (is_grounded && !is_hitceiling)
        {
            vertical_velocity = 0f;
            
            if (_do_jump)
            {
                var gravity_with_button = _jumpheight * 2 / (jumplength_with_button * jumplength_with_button);
                vertical_velocity = gravity_with_button * jumplength_with_button;

                _do_jump = false;
                jump.Invoke();
            }
        }
        else
        {

            if (vertical_velocity < 0)
            {
                var gravity_after_dip = _jumpheight * 2 / (jumplength_after_dip * jumplength_after_dip);
                vertical_velocity -= gravity_after_dip * Time.deltaTime;
            }else if (Input.GetButton("Jump"))
            {
                var gravity_with_button = _jumpheight * 2 / (jumplength_with_button * jumplength_with_button);
                vertical_velocity -= gravity_with_button * Time.deltaTime;
            }
            else
            {
                var gravity_without_button = _jumpheight * 2 / (jumplength_without_button * jumplength_without_button);
                vertical_velocity -= gravity_without_button * Time.deltaTime;
            }
        }

        return vertical_velocity;
    }

    float horizontal_movement()
    {
        float x_axis = Input.GetAxis("Horizontal");
        if (!is_grounded)
        {
            x_axis *= _inair_damping;
            horizontal_velocity += x_axis * _maxspeed;
            horizontal_velocity = System.Math.Max(System.Math.Min(_maxspeed, horizontal_velocity),-_maxspeed);
        }
        else
            horizontal_velocity = (x_axis*_maxspeed);
                
        return horizontal_velocity;
    }
}
