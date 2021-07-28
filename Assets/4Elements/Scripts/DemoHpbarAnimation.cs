using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoHpbarAnimation : MonoBehaviour {

   public Slider barToAnimate;
   private float randomFloat;
   private float speed;
	// Use this for initialization
	void Start () {

        randomFloat = Random.RandomRange(0.3f, 0.7f);
        speed = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
       
        barToAnimate.value -= speed * Time.deltaTime;
        if (barToAnimate.value < randomFloat || barToAnimate.value == 1f)
        {
            
           speed *= -1;
        }
	}
}
