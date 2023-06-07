using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WebGraphController : MonoBehaviour
{

    public RawImage image;
    public SpriteShapeController shapeController;

    public float radius = 4.5f;
    public List<float> mockValue = new List<float>();

    public void SetColor(Color color)
    {

        System.Action<ITween<Color>> tweenUpdate = (t) =>
        {
            image.color = t.CurrentValue;
        };
        System.Action<ITween<Color>> tweenComplete = (t) =>
        {
            image.color = color;
        };


        // completion defaults to null if not passed in
        gameObject.Tween(null, image.color, color, 1f, TweenScaleFunctions.CubicEaseInOut, tweenUpdate, tweenComplete);
    }

    public void SetShapeValue(List<float> vals)
    {

        var val01s = new List<float>();

        for (int i = 0; i < vals.Count; i++)
        {
            //Debug.Log("raw val: " + vals[i]);
            var _01val = (int)vals[i] / 6f;
            //Debug.Log("clamp: " + _01val);
            val01s.Add(_01val);
        }

        var targetPos = val01s.ToList();

        System.Action<ITween<float>> tweenUpdate = (t) =>
        {
            var tweenVal = new List<Vector3>();
            for (int i = 0; i < targetPos.Count; i++)
            {
                var currentPos = shapeController.spline.GetPosition(i);
                var _radius = Mathf.Clamp(radius * val01s[i], 0.1f, radius);

                var angle = ((-(Mathf.PI * 2f) / 6f) * i) + Mathf.PI / 2f;

                float x = Mathf.Cos(angle) * _radius; //r cos;
                float y = Mathf.Sin(angle) * _radius; // r sin;

                var pos = new Vector3(x, y);

                tweenVal.Add(Vector3.Lerp(currentPos, pos, t.CurrentProgress));
            }

            _SetShapeValue(tweenVal);
        };
        System.Action<ITween<float>> tweenComplete = (t) =>
        {
            _SetShapeValue(val01s);
        };


        // completion defaults to null if not passed in
        gameObject.Tween(null, 0f, 1f, 1f, TweenScaleFunctions.CubicEaseInOut, tweenUpdate, tweenComplete);

    }

    public void _SetShapeValue(List<float> val01s)
    {
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
    public void _SetShapeValue(List<Vector3> val01s)
    {
        for (int i = 0; i < 6; i++)
        {
            shapeController.spline.SetPosition(i, val01s[i]);
        }
    }

    public void ResetGraph()
    {
        float[] val01s = { 0f, 0f, 0f, 0f, 0f, 0f };
        _SetShapeValue(val01s.ToList());
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetGraph();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
