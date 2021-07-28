using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoImageFill : MonoBehaviour
{

    public Image ImageToAnimate;
    private float randomFloat;
    private float speed;
    // Use this for initialization
    void Start()
    {

      
        speed = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {

       ImageToAnimate.fillAmount += speed * Time.deltaTime;
       Debug.Log(ImageToAnimate.fillAmount);
        if (ImageToAnimate.fillAmount == 1f)
        {

            speed *= -1;
        }
        if (ImageToAnimate.fillAmount == 0f)
        {

            speed *= -1;
        }
    }
}
