using UnityEngine;
using System.Collections;
// Redundant class in hindsight
public class InputManager : MonoBehaviour
{
    public static Vector3 mousePositionInWorld;
    public static Rect mousePositionOnScreen;
	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(readMousePositionInWorld());
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonUp(0))
	    {
	        LeftClick();
	    }
        if (Input.GetMouseButtonUp(1))
        {
            //RightClick();
        }
    }
    //Method sends raycast and checks if object hit is Clickable if so run the clicked method
    void LeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
            var clickable = monohit as IClickable;
            if (clickable != null)
            {
                clickable.LeftClicked();
            }
        }
    }

    private IEnumerator readMousePositionInWorld()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            mousePositionInWorld = ray.GetPoint(100);
        }
    }
}
