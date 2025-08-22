using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public ParticleSystem muzzleEffect;
    public ParticleSystem shellEffect;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public Transform fireposition;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ShotEffect());
        }
    }

    private IEnumerator ShotEffect()
    {
        muzzleEffect.Play();
        shellEffect.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, fireposition.position);

        Vector3 endPos = fireposition.position + fireposition.forward * 10f;

        lineRenderer.SetPosition(1, endPos);

        yield return new WaitForSeconds(1f);

        lineRenderer.enabled = false;
    }
}
