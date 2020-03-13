using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHider : MonoBehaviour
{
    [SerializeField]
    private float _reset_value = 3.0f;

    private float _remaining_delay = 3.0f;

    private Vector2 _last_position;

    // Update is called once per frame
    void Update()
    {
        _remaining_delay = Mathf.Clamp(_remaining_delay - Time.deltaTime, 0.0f, _reset_value);
        if(!Input.mousePosition.Equals(_last_position))
        {
            _remaining_delay = _reset_value;
        }
        if (_remaining_delay == 0.0f)
        {
            Cursor.visible = false;
        }
        _last_position = Input.mousePosition;
    }
}
