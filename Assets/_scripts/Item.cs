using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour, IClickable, IStealable
{
    public int value;
    public void Clicked()
    {
        //If player next to object getstolen

        //else queue movement to nearest grid
        throw new System.NotImplementedException();
    }

    public IEnumerator GetStolen()
    {
        //Run animation
        while (Vector3.Distance(transform.position, PlayerCharacter.playerVector3)> 0.1)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerCharacter.playerVector3, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        //Add value to score
        GameManager.instance.ValueStolen += value;
        //Destroy Prefab
        Destroy(gameObject);
    }
}
