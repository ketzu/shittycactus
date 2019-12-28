using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private float correctionfactor = 0.9f; 
    [SerializeField]
    private float safezone = 20f;

    [SerializeField]
    private float stepsize = 10f;

    [SerializeField]
    private GameObject stars;
    private Color _stars_color;

    private Camera _camera;
    private Color _sky_color;

    private GameObject _player;

    [SerializeField]
    private GameObject galaxy;
    [SerializeField]
    private float galaxy_safezone = 400f;
    private Color _galaxy_color;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _camera = Camera.main;
        _sky_color = _camera.backgroundColor;

        _stars_color = stars.GetComponent<SpriteRenderer>().color;
        _galaxy_color = galaxy.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera.transform.position.y < safezone)
            return;

        float steps = (_camera.transform.position.y - safezone) / stepsize;

        stars.transform.localPosition = new Vector3(0, Mathf.Max(0f,9.739473f - 1f*steps), stars.transform.localPosition.z);
        stars.transform.position = new Vector3(0, stars.transform.position.y, stars.transform.position.z);

        Color tmp_stars = _stars_color;
        tmp_stars.a += _player.transform.position.y / (stepsize*15f);
        stars.GetComponent<SpriteRenderer>().color = tmp_stars;

        Color tmp_sky = _sky_color;
        float change = Mathf.Pow(correctionfactor, steps);
        change = Mathf.Clamp(change, 0, 1);
        tmp_sky.r *= change;
        tmp_sky.g *= change;
        tmp_sky.b *= change;
        _camera.backgroundColor = tmp_sky;

        if (_camera.transform.position.y < galaxy_safezone)
            return;

        steps = (_camera.transform.position.y - galaxy_safezone) / stepsize;
        galaxy.transform.localPosition = new Vector3(0, Mathf.Max(0f, 9.739473f - 1f * steps), galaxy.transform.localPosition.z);
        galaxy.transform.position = new Vector3(0, galaxy.transform.position.y, galaxy.transform.position.z);
        tmp_stars = _galaxy_color;
        tmp_stars.a += (_player.transform.position.y-galaxy_safezone) / (stepsize * 15f);
        //tmp_stars.a = Mathf.Clamp(0f, 0.55f, tmp_stars.a);
        galaxy.GetComponent<SpriteRenderer>().color = tmp_stars;

    }
}
