using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WebGraphController : MonoBehaviour
{

    public SpriteShapeController shapeController;

    public float radius = 4.5f;
    public List<float> mockValue = new List<float>();


    [ContextMenu("SetShapeValue")]
    public void SetShapeValue()
    {
        SetShapeValue(mockValue);
    }
    //[ContextMenu("SetShapeValue")]
    public void SetShapeValue(List<float> val01s)
    {
        var centerPos = transform.position;
        for (int i = 0; i < 6; i++)
        {
            var _radius = Mathf.Clamp(radius * val01s[i], 0.1f, radius);

            var angle = ((-(Mathf.PI * 2f) / 6f) * i) + Mathf.PI / 2f;

            float x = Mathf.Cos(angle) * _radius; //r cos;
            float y = Mathf.Sin(angle) * _radius; // r sin;

            var pos = new Vector3(x, y);
            shapeController.spline.SetPosition(i, pos);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
