using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;


public class ButtonManager : MonoBehaviour
{

    public enum AnimationType { Flames, Aurora };

    public AnimationType AnimType;

    public float scrollSpeed = 0.03F;
    public Image ImageToAnimate;

    public Color mainColor = Color.white;
    public Color specularColor = Color.white;

    private float scaleDir = -1f;


    public void OnClickTest()
    {
        Debug.Log("pressed");
    }


    void Awake()
    {

       

     

        ImageToAnimate.material.SetTextureOffset("_BumpMap", new Vector2(0, 0));
        ImageToAnimate.material.SetTextureScale("_BumpMap", new Vector2(1f, 0.2f));
    }
    void Update()
    {
         ImageToAnimate.material.SetColor("_Color", mainColor);

        ImageToAnimate.material.SetColor("_SpecColor", specularColor);


        float offset = (Time.time * scrollSpeed) * -1;

        float scaleX = Mathf.Cos(Time.time) * 0.5F + 1;
        float scaleY = Mathf.Sin(Time.time) * (0.02F * scaleDir);

        switch (AnimType)
        { 
            case AnimationType.Aurora:
                
                ImageToAnimate.material.SetTextureOffset("_BumpMap", new Vector2(0, offset));
                ImageToAnimate.material.SetTextureScale("_BumpMap", new Vector2(1f, scaleY));

                break;

            case AnimationType.Flames:
                
                ImageToAnimate.material.SetTextureOffset("_BumpMap", new Vector2(0, offset));
             
                break;
        }
       

     

    }
}