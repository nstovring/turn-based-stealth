using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Camera))]

public class GlobalShaderVariables : MonoBehaviour
{
    private void OnPreRender()
    {
        //Shader.SetGlobalFloat("_AspectRatio", (float)Screen.width / (float)Screen.height);
        //Shader.SetGlobalFloat("_FieldOfView", Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad * 0.5f) * 2f);
        //Shader.SetGlobalVector("_CamPos", this.transform.position);
        Shader.SetGlobalVector("_PlayerPosition", GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer].myTransform.position);
        //Shader.SetGlobalVector("_CamUp", this.transform.up);
        //Shader.SetGlobalVector("_CamForward", this.transform.forward);
        //Shader.SetGlobalTexture("_NoiseOffsets", noiseTexture2D);
    }
  
}
