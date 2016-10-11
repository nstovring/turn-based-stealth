using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour, IClickable, IStealable
{
    public int value;
    public void LeftClicked()
    {
        //If player next to object getstolen

        //else queue movement to nearest grid
        throw new System.NotImplementedException();
    }

    public void RightClicked()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator GetStolen(Vector3 playerVector3)
    {
        //Run animation
        while (Vector3.Distance(transform.position, playerVector3)> 0.1)
        {
            transform.position = Vector3.Lerp(transform.position, playerVector3, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        //Add value to score
        GameManager.instance.ValueStolen += value;
        //Destroy Prefab
        Destroy(gameObject);
    }
}
