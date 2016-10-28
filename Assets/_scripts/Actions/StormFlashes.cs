using UnityEngine;
using System.Collections;

public class StormFlashes : MonoBehaviour
{
    public bool StartStorm = true;
    public Light DirectionalLight;



    public float longDelay = 5;
    public float shortDelay = 0.5f;


    // Use this for initialization
    void Start ()
    {
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
                DirectionalLight.intensity = Mathf.Lerp(DirectionalLight.intensity, 0, 0.01f);
                if (DirectionalLight.intensity < 0.01f)
                    DirectionalLight.intensity = 0;
                yield return new WaitForEndOfFrame();
            }
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
