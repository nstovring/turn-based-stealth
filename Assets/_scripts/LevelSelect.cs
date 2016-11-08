using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour { 

    public bool _showGui;
    public float FadeTime;

    public Text NodeName;

	// Use this for initialization
	void Start ()
	{

	    _showGui = false;
	    NodeName.color = Color.clear;

	}
	
	// Update is called once per frame
	void Update ()
	{
	    
	    HoverSelect();

	    FadeText();

	}

    

    void HoverSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {

            if (hit.collider.tag == "MapNode")
            {
                _showGui = true;
            }
            else
            {
                _showGui = false;
            }

        }
    }


    void FadeText()
    {

        if (_showGui)
        {
            NodeName.color = Color.Lerp(NodeName.color, Color.black, FadeTime*Time.deltaTime);
        }
        else
        {
            NodeName.color = Color.Lerp(NodeName.color, Color.clear, FadeTime*Time.deltaTime);
        }
    }

}
