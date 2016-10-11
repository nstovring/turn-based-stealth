using UnityEngine;
using System.Collections;

public interface IStealable
{
    IEnumerator GetStolen(Vector3 playerVector3);

}
