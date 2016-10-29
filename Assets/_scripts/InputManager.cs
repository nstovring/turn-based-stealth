using UnityEngine;
using System.Collections;
// Redundant class in hindsight
public class InputManager : MonoBehaviour
{
    public static Vector3 mousePositionInWorld;
    public static Rect mousePositionOnScreen;
    public Transform Selector;

    public LayerMask ClickableLayer;
	// Use this for initialization
	void Start ()
	{
	    Selector = Instantiate(Selector, Vector3.zero,Quaternion.identity) as Transform;
	    Selector.parent = transform;
	    StartCoroutine(ReadMousePositionInWorld());
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
        if (Physics.Raycast(ray, out hit, 100, ClickableLayer))
        {
            MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
            var clickable = monohit as IClickable;
            if (clickable != null)
            {
                clickable.LeftClicked();
            }
        }
    }

    private IEnumerator ReadMousePositionInWorld()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Ground")))
            {
                MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
                var cell = monohit as Cell;
                if (cell)
                {
                    GameManager.Instance.PlayerCharacters[0].mySeeker.ResetPosition();
                    if (GameManager.Instance.PlayerTurn)
                        GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer].mySeeker.SetPathToDestination(cell.transform);

                  

                    mousePositionInWorld = cell.transform.position + new Vector3(0, 1, 0);
                }
            }
            Selector.transform.position = mousePositionInWorld;
        }
    }
}
