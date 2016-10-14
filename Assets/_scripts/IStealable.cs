using UnityEngine;
using System.Collections;

public interface IStealable
{
    IEnumerator GetStolen(Transform playerTransform);

}
