using System.Collections;
using System.Linq;
using UnityEngine;

public class Grapher : MonoBehaviour
{
    public static void GraphToPanel(GameObject panel, float[] data, GameObject prefab, float max, float offset)
    {
        //calculate scale factors
        RectTransform transform = panel.GetComponent<RectTransform>();

        float width = transform.rect.width;
        float height = transform.rect.height;

        int length = data.Length;

        float widthScaleFactor = (width * 2) / (float)length / 2;
        float heightScaleFactor = (height / 2) / (float)(max / 2 == 0 ? 1 : max / 2);

        int idx = 0;
        foreach (int dataPoint in data)
        {
            Vector2 currentPoint = new Vector2((widthScaleFactor * idx) - (width / 2F) + widthScaleFactor, (heightScaleFactor * data[idx]) - (height / 2F) + offset);
            Vector2 nextPoint = (idx + 1 < data.Length ? new Vector2((widthScaleFactor * (idx + 1)) - (width / 2F) + widthScaleFactor, (heightScaleFactor * data[idx + 1]) - (height / 2F) + offset) : currentPoint);

            GameObject dataPointObject = new GameObject();
            dataPointObject.name = "Data: " + idx;
            dataPointObject.transform.SetParent(panel.transform);

            LineRenderer renderer = dataPointObject.AddComponent<LineRenderer>();
            renderer.startWidth = 0.5F;
            renderer.endWidth = 0.5F;
            renderer.startColor = Color.black;
            renderer.endColor = Color.black;

            Vector3 newPosA = panel.transform.TransformPoint(currentPoint);
            newPosA.z -= 1;
            Vector3 newPosB = panel.transform.TransformPoint(nextPoint);
            newPosB.z -= 1;

            renderer.SetPosition(0, newPosA);
            renderer.SetPosition(1, newPosB);

            idx++;
        }
    }

    public static void ClearGraph(GameObject panel)
    {
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            if (panel.transform.GetChild(i).gameObject.name.Contains("Data"))
            {
                GameObject.Destroy(panel.transform.GetChild(i).gameObject);
            }

        }
    }
}
