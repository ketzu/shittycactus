using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElementSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject left_end;
    [SerializeField]
    private GameObject right_end;

    [SerializeField]
    private GameObject coinPrefab;

    private float _coinProbability;

    [SerializeField]
    private GameObject safeCloudPrefab;

    private float _safeCloudRate;
    private float _safeCloudCounter = 0f;

    [SerializeField]
    private GameObject shortBoxPrefab;

    [SerializeField]
    private float safetyMargin = 50f;

    [SerializeField, Range(0.05f, 2.5f)]
    private float boxSafetyMargin = 0.1f;
    [SerializeField, Range(0.1f, 2f)]
    private float minHeightDiff = 0.5f;

    private int _max_spawnable_places = 40;

    private GameObject _player;

    private float _default_box_width;

    private float _jumpheight;
    private float _jumpwidth;
    private float _jumphighwidth;

    private int _entry_level;

    private System.Random _rnd = new System.Random();

    [SerializeField]
    private UpgradeObject safecloud_upgrade;
    [SerializeField]
    private UpgradeObject entrylevel_upgrade;
    [SerializeField]
    private UpgradeObject coinprobability_upgrade;

    private List<Rect> _spawnable_rects;
#if UNITY_EDITOR
    private List<Rect> _debug_rects = new List<Rect>();
#endif
    // Start is called before the first frame update
    void Start()
    {
        _safeCloudRate = safecloud_upgrade.getValue();
        _entry_level = Mathf.RoundToInt(entrylevel_upgrade.getValue());
        _coinProbability = coinprobability_upgrade.getValue();

        _default_box_width = shortBoxPrefab.GetComponent<BoxCollider2D>().size.x;

        _player = GameObject.FindGameObjectWithTag("Player");

        var info = _player.GetComponent<PlayerController>();

        _jumpheight = info.JumpHeight;
        var playerSpeed = info.Speed;
        _jumpwidth = playerSpeed * info.JumplengthAfterDip + playerSpeed * info.JumplengthWithButton;
        _jumphighwidth = playerSpeed * info.JumplengthWithButton;

        _spawnable_rects = new List<Rect>(_max_spawnable_places);

        spawnEntryLayer();
    }

    void removeRects(Rect rect)
    {
        _spawnable_rects.RemoveAll(_spawnable_rect => rect.Overlaps(_spawnable_rect));
    }

    bool isSpaceClear(Rect rect)
    {
        float bottom_check = 0.8f;
        var up = new Vector2(0, minHeightDiff - 0.01f + bottom_check);
        var right = new Vector2(2 * _default_box_width, 0);
        var upRight = up + right;

        var basepoint = new Vector2(rect.center.x - _default_box_width, rect.y - bottom_check);
#if UNITY_EDITOR
        _debug_rects.Add(new Rect(basepoint.x, basepoint.y, right.x, up.y));
#endif
        if(Physics2D.OverlapBox(basepoint + 0.5f * upRight, upRight, 0f) != null) 
            return false;

        return true;
    }

    void addRect(Rect rect)
    {
        if (!isSpaceClear(rect))
            return;

        var default_deviation = 0.6f * _default_box_width;
        if(rect.x < left_end.transform.position.x + default_deviation)
        {
            rect.width = rect.x + rect.width - left_end.transform.position.x - default_deviation;
            rect.x = left_end.transform.position.x + default_deviation;
        }else if (rect.xMax > right_end.transform.position.x - default_deviation)
        {
            rect.width = right_end.transform.position.x - default_deviation - rect.x;
        }

        if (rect.width < 0.01)
            return;

        bool hit = false;
        for(int i=0;i<_spawnable_rects.Count;i+=1)
        {
            if(rect.Overlaps(_spawnable_rects[i]))
            {
                float top_x = Mathf.Min(rect.x + rect.width, _spawnable_rects[i].x + _spawnable_rects[i].width);
                float top_y = Mathf.Min(rect.y + rect.height, _spawnable_rects[i].y + _spawnable_rects[i].height);
              
                float new_x = Mathf.Max(_spawnable_rects[i].x, rect.x);
                float new_y = Mathf.Max(_spawnable_rects[i].y, rect.y);

                _spawnable_rects[i] = new Rect(new_x, new_y, top_x-new_x, top_y-new_y);

                hit = true;
            }
        }

        if(!hit)
        {
            _spawnable_rects.Add(rect);
            if (_spawnable_rects.Count > _max_spawnable_places)
            {
                _spawnable_rects.RemoveAt(0);
            }
        }
    }

    void spawnBox(Vector3 pos, bool skipcheck = false)
    {
        var clearspace = new Rect(pos.x - _default_box_width, pos.y, 2 * _default_box_width, minHeightDiff - 0.01f);
        removeRects(clearspace);
        if (!skipcheck && !isSpaceClear(clearspace))
            return;

        _safeCloudCounter += (float)_rnd.NextDouble() * _safeCloudRate;
        if (_safeCloudCounter > 1f)
        {
            GameObject.Instantiate(safeCloudPrefab, pos, Quaternion.identity, transform);
            _safeCloudCounter = 0f;
        }
        else
        {
            GameObject.Instantiate(shortBoxPrefab, pos, Quaternion.identity, transform);
        }

        // Spawn Coins on top of box
        if (_rnd.NextDouble() < _coinProbability && pos.y > 10)
        {
            GameObject.Instantiate(coinPrefab, new Vector3(pos.x, pos.y + 1, 0), Quaternion.identity, transform);
        }

        var width = _jumphighwidth + _default_box_width - boxSafetyMargin;
        addRect(new Rect(pos.x + boxSafetyMargin, pos.y + minHeightDiff, width, _jumpheight - minHeightDiff));
        addRect(new Rect(pos.x - boxSafetyMargin - width, pos.y + minHeightDiff, width, _jumpheight - minHeightDiff));
    }

    void spawnEntryLayer()
    {
        float base_height = -3.7f;
        var _levelwidth = right_end.transform.position.x - left_end.transform.position.x;
        var _freespace = _levelwidth - _entry_level * _default_box_width;
        
        double[] _randoms = new double[2+_entry_level];

        double accum = 0d;
        for (int i = 0; i < _randoms.Length; i += 1)
        {
            var tmp = _rnd.NextDouble()+0.25;
            _randoms[i] = tmp;
            accum += tmp;
        }
        for (int i = 0; i < _randoms.Length; i += 1)
        {
            _randoms[i] /= accum;
        }

        float x = left_end.transform.position.x;

        for (int i = 0; i < _entry_level; i += 1)
        {
            x += (float)(_randoms[i]) * _freespace + _default_box_width / 2;
            spawnBox(new Vector3(x, base_height + _jumpheight * (float)(_rnd.NextDouble() * 0.2f + 0.8), 0), true);
            x += _default_box_width / 2;
        }
    }

    private Vector3 randomPos(Rect rect)
    {
        var x = (float)_rnd.NextDouble() * rect.width + rect.x;
        var y = (float)_rnd.NextDouble() * rect.height + rect.y;
        return new Vector3(x,y,0);
    }

#if UNITY_EDITOR
    void debugDrawRect(Rect rect, Color color)
    {
        var bl = new Vector3(rect.x, rect.y, 0);
        var br = new Vector3(rect.x + rect.width, rect.y, 0);
        var tl = new Vector3(rect.x, rect.y + rect.height, 0);
        var tr = new Vector3(rect.x + rect.width, rect.y + rect.height, 0);

        Debug.DrawLine(bl, br, color);
        Debug.DrawLine(br, tr, color);
        Debug.DrawLine(tr, tl, color);
        Debug.DrawLine(tl, bl, color);
    }
#endif
    // Update is called once per frame
    void Update()
    {
        while (_spawnable_rects.Min(rect => rect.y) < _player.transform.position.y + safetyMargin)
        //if (Input.GetKeyDown(KeyCode.P))
        {
            var next = _rnd.Next(0, _spawnable_rects.Count-1);
            var pos_rect = _spawnable_rects[next];
            _spawnable_rects.RemoveAt(next);

            spawnBox(randomPos(pos_rect));
        }

#if UNITY_EDITOR

        foreach (var rect in _spawnable_rects)
        {
            debugDrawRect(rect, Color.red);
        }
        foreach (var rect in _debug_rects)
        {
            debugDrawRect(rect, Color.green);
        }
#endif
    }
}
