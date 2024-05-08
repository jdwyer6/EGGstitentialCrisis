using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainParticles : MonoBehaviour
{
    private ParticleSystem rain;
    public GameObject rainSplashPrefab;
    public int poolSize = 100;

    private Queue<GameObject> splashPool;

    public float rainInterval = 30f;
    public float timeToSetupRain = 30f;
    public float timeToRain = 30f;
    public float timeBetweenThunderMin = 10f;
    public float timeBetweenThunderMax = 20f;
    public float timeToStopRaining = 15f;
    
    private float normalSunIntensity = 2f;
    private float thunderSunIntensity = 25f;

    public bool isRaining = false;
    private float initialGroundSmoothness = 0;

    public Material groundMaterial;

    public Light sun;
    private float fogEndValue;

    private AudioManager am;

    void Start()
    {
        rain = GetComponent<ParticleSystem>();
        am = FindObjectOfType<AudioManager>();
        fogEndValue = RenderSettings.fogEndDistance;

        // Initialize the object pool
        splashPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject splash = Instantiate(rainSplashPrefab);
            splash.SetActive(false);
            splashPool.Enqueue(splash);
        }

        StartCoroutine(InitiateRain());
    }

    private void Update() {
        transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int numCollisionEvents = rain.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            if (splashPool.Count > 0)
            {
                Vector3 hitPos = collisionEvents[i].intersection;

                // Reuse a splash from the pool
                GameObject splash = splashPool.Dequeue();
                splash.transform.position = hitPos;
                splash.SetActive(true);

                // Return the splash to the pool after some time
                StartCoroutine(ReturnToPool(splash, 1f));
     
            }
        }
    }

    IEnumerator ReturnToPool(GameObject splash, float delay)
    {
        yield return new WaitForSeconds(delay);
        splash.SetActive(false);
        splashPool.Enqueue(splash);
    }

    IEnumerator InitiateRain() 
    {
        while (true)
        {
            yield return new WaitForSeconds(rainInterval);
            am.Play("PreRain");
            yield return new WaitForSeconds(timeToSetupRain);
            Debug.Log("Rain");
            isRaining = true;
            StartCoroutine(HandleWetGround());
            // StartCoroutine(InitWeatherChange());
            StartCoroutine(InitThunder());
            am.Play("Rain");
            // add fog
            // change ground texture
            rain.Play();
            
            yield return new WaitForSeconds(timeToRain);
            rain.Stop();
            isRaining = false;
            StartCoroutine(HandleWetGround());
            // StartCoroutine(EndWeatherChange());
            yield return new WaitForSeconds(timeToStopRaining);
        }
    }

    IEnumerator InitThunder() 
    {
        while (isRaining)
        {
            yield return new WaitForSeconds(Random.Range(timeBetweenThunderMin, timeBetweenThunderMax));
            am.Play("Thunder");
            while (sun.intensity < thunderSunIntensity)
            {
                sun.intensity += 5f;
                yield return new WaitForSeconds(0.1f);
            }

            while (sun.intensity > normalSunIntensity)
            {
                sun.intensity -= 1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator InitWeatherChange() {
        while (RenderSettings.fogEndDistance > 15)
        {
            RenderSettings.fogEndDistance -= 1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator EndWeatherChange() {
            while (RenderSettings.fogEndDistance < fogEndValue)
            {
                RenderSettings.fogEndDistance += 1f;
                yield return new WaitForSeconds(0.1f);
            }
    }

    IEnumerator HandleWetGround() {
        if(isRaining) {
            while (groundMaterial.GetFloat("_Smoothness") < 1f)
            {
                groundMaterial.SetFloat("_Smoothness", groundMaterial.GetFloat("_Smoothness") + 0.05f);
                yield return new WaitForSeconds(0.1f);
            }
        } else {
            while (groundMaterial.GetFloat("_Smoothness") < initialGroundSmoothness)
            {
                groundMaterial.SetFloat("_Smoothness", groundMaterial.GetFloat("_Smoothness") - 0.05f);
                yield return new WaitForSeconds(0.1f);
            }
        }

    }
}