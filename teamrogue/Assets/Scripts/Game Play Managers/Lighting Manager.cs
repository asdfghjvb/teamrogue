using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] List<GameObject> lights;
    [SerializeField] float activationRadius;

    [Tooltip("How often the player radius is checked for lights")]
    [SerializeField] float checkFrequency;

    [SerializeField] float lightIntensity;

    bool cooldownActive = false;

    // Start is called before the first frame update
    void Start()
    {
        //lights = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cooldownActive)
            StartCoroutine(ActivateLightSources());
    }

    void CheckRadius()
    {
        if (GameManager.instance.player == null)
            return;

        foreach (GameObject light in lights)
        {
            float distToPlayer = Vector3.Distance(GameManager.instance.player.transform.position, light.transform.position);

            if (distToPlayer <= activationRadius)
                light.SetActive(true);
            else
                light.SetActive(false);
        }
    }

    public void RegisterLightSource(GameObject light)
    {
        if (lights == null)
            lights = new List<GameObject>();

        lights.Add(light);
    }

    IEnumerator ActivateLightSources()
    {
        cooldownActive = true;

        CheckRadius();
        yield return new WaitForSeconds(checkFrequency);

        cooldownActive = false;
    }

    public void UpdateLightSources()
    {
        foreach (GameObject light in lights)
        {
            Light lightComp = light.GetComponentInChildren<Light>();

            if (lightComp != null)
            {
                lightComp.intensity = lightIntensity;
            }
        }
    }
}
