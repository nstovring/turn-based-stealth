using UnityEngine;
using System.Collections;

public class StormFlashes : MonoBehaviour
{
    public bool StartStorm = true;
    public Light DirectionalLight;

    public float longDelay = 5;
    public float shortDelay = 0.5f;

    public AudioClip Rain;
    public AudioClip[] ThunderClaps;
    private AudioSource rain;
    public AudioSource thunderClaps;

    // Use this for initialization
    void Start ()
    {
        rain = transform.GetComponent<AudioSource>();
        //thunderClaps = transform.GetComponentInChildren<AudioSource>();
        StartCoroutine(Flashes());
    }

    IEnumerator Flashes()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            while (DirectionalLight.intensity < 3)
            {
                DirectionalLight.intensity = Mathf.Lerp(DirectionalLight.intensity, 8, 0.8f);
                yield return new WaitForEndOfFrame();
            }

            while (DirectionalLight.intensity > 0)
            {
                DirectionalLight.intensity = Mathf.Lerp(DirectionalLight.intensity, 0, 1f);
                if (DirectionalLight.intensity < 0.01f)
                    DirectionalLight.intensity = 0;
                yield return new WaitForEndOfFrame();
            }

            while (DirectionalLight.intensity < 3)
            {
                DirectionalLight.intensity = Mathf.Lerp(DirectionalLight.intensity, 8, 0.8f);
                yield return new WaitForEndOfFrame();
            }
            while (DirectionalLight.intensity > 0)
            {
                DirectionalLight.intensity = Mathf.Lerp(DirectionalLight.intensity, 0, 0.05f);
                if (DirectionalLight.intensity < 0.1f)
                    DirectionalLight.intensity = 0;
                yield return new WaitForEndOfFrame();
            }
            int randomThunder = Random.Range(0, ThunderClaps.Length);
            AudioClip selectedClip = ThunderClaps[randomThunder];
            thunderClaps.clip = selectedClip;
            thunderClaps.Play();
            yield return new WaitForSeconds(selectedClip.length);
            yield return new WaitForSeconds(Random.Range(5,longDelay));
        }
        //StopCoroutine(Flashes());
    }

   
    // Update is called once per frame
    void Update () {
        if (StartStorm)
        {
            StartCoroutine(Flashes());
            StartStorm = false;
        }
	}
}
