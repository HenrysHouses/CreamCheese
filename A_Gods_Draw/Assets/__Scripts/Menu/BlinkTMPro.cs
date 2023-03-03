using UnityEngine;
using TMPro;

public class BlinkTMPro : MonoBehaviour
{
    [SerializeField] float speed = 0.34f;
    [SerializeField] float SmoothTime = 0.47f;
    float Velocity = 0.1f;
    [SerializeField] TextMeshProUGUI text;

    float t = 1;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * speed;

        Color color = text.color;

        float target = t%2;

        if(target > 1)
            target = 0;
        else 
            target = 1;

        color.a = Mathf.SmoothDamp(color.a, target, ref Velocity, SmoothTime) ;

        text.color = color;        
    }
}
