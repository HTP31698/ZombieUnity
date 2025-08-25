using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    private State currentState = State.Ready;

    public State CurrentState
    {
        get { return currentState; }
        private set
        {
            currentState = value;
            switch (currentState)
            {
                case State.Ready:
                    break;
                case State.Empty:
                    break;
                case State.Reloading:
                    break;
            }
        }
    }

    public GunData gunData;

    public ParticleSystem muzzleEffect;
    public ParticleSystem shellEffect;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public Transform fireposition;

    public int ammoRemain;
    public int magAmmo;

    private float lastFireTime;
    private float reloadTime;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }
    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        lastFireTime = 0f;

        CurrentState = State.Ready;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Ready:
                UpdateReady();
                break;
            case State.Empty:
                UpdateEmpty();
                break;
            case State.Reloading:
                UpdateReloading();
                break;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        audioSource.PlayOneShot(gunData.shootClip);

        muzzleEffect.Play();
        shellEffect.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, fireposition.position);
        Vector3 endPos = hitPosition;
        lineRenderer.SetPosition(1, endPos);

        yield return new WaitForSeconds(1f);

        lineRenderer.enabled = false;
    }

    private void UpdateReady()
    {

    }

    private void UpdateEmpty()
    {

    }

    private void UpdateReloading()
    {

    }

    public void Fire()
    {
        if (currentState == State.Ready && Time.time > (lastFireTime + gunData.timeBetFire))
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    public void Shoot()
    {
        Vector3 hitPosition = Vector3.zero;

        RaycastHit hit;

        if (Physics.Raycast(fireposition.position, fireposition.forward, out hit
, gunData.fireDistance))
        {
            hitPosition = hit.point;
            var target = hit.collider.GetComponent<IDamagable>();
            //hit.collider.CompareTag("");
            //hit.collider.GetComponent<Monster>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = fireposition.position + fireposition.forward * 10f;
        }

        StartCoroutine(ShotEffect(hitPosition));

        --magAmmo;
        if (magAmmo == 0)
        {
            CurrentState = State.Empty;
        }


    }
    public bool ReLoad()
    {
        //if (currentState == State.Empty
        //    && ammoRemain >= gunData.magCapacity
        //    && Time.time > (reloadTime + gunData.reloadTime))
        //{
        //    reloadTime = Time.time;
        //    ammoRemain -= gunData.magCapacity;
        //    magAmmo = gunData.magCapacity;
        //    currentState = State.Ready;
        //    Debug.Log($"³²Àº ÅºÃ¢: {ammoRemain}");
        //}
        //else if (currentState == State.Ready
        //    && ammoRemain >= gunData.magCapacity - magAmmo
        //    && gunData.magCapacity > magAmmo
        //    && Time.time > (reloadTime + gunData.reloadTime))
        //{
        //    reloadTime = Time.time;
        //    ammoRemain -= gunData.magCapacity - magAmmo;
        //    magAmmo = gunData.magCapacity;
        //    currentState = State.Ready;
        //    Debug.Log($"³²Àº ÅºÃ¢: {ammoRemain}");
        //}
        //else { return false; }
        if (CurrentState == State.Reloading
            || ammoRemain == 0
            || magAmmo == gunData.magCapacity)
        { return false; }

        StartCoroutine(CoReload());


        return true;
    }

    IEnumerator CoReload()
    {
        CurrentState = State.Reloading;
        audioSource.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        magAmmo += ammoRemain;
        if (magAmmo >= gunData.magCapacity)
        {
            magAmmo = gunData.magCapacity;
            ammoRemain -= magAmmo;
            Debug.Log($"³²Àº ÅºÃ¢: {ammoRemain}");
        }
        else
        {
            ammoRemain = 0;
            Debug.Log($"³²Àº ÅºÃ¢: {ammoRemain}");
        }

        CurrentState = State.Ready;
    }
}
