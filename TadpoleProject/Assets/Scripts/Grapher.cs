using System.Collections;
using System.Linq;
using UnityEngine;

public class Grapher : MonoBehaviour
{
    public static void GraphToPanel(GameObject panel, float[] data, GameObject prefab)
    {
        //calculate scale factors
        RectTransform transform = panel.GetComponent<RectTransform>();

        float width = transform.rect.width;
        float height = transform.rect.height;

        float max = data.Max();
        int length = data.Length;

        float widthScaleFactor = (width * 2) / (float)length / 2;
        float heightScaleFactor = (height * 2) / (float)max / 2;

        int idx = 0;
        foreach (int dataPoint in data)
        {
            Vector2 currentPoint = new Vector2((widthScaleFactor * idx) - (width / 2F) + widthScaleFactor / 2F, 0 - (height / 2F));
            //Vector2 nextPoint = (idx + 1 < data.Length ? new Vector2((widthScaleFactor * (idx + 1)) - (width / 2F), (heightScaleFactor * data[idx + 1]) - (height / 2F)) : currentPoint);
            Vector2 nextPoint = new Vector2((widthScaleFactor * idx) - (width / 2F) + widthScaleFactor / 2F, Mathf.Max((heightScaleFactor * dataPoint) - (height / 2F), 15 - (height / 2F)));

            GameObject dataPointObject = GameObject.Instantiate(prefab);
            dataPointObject.name = "Data: " + idx;
            dataPointObject.transform.SetParent(panel.transform);
            dataPointObject.transform.localPosition = currentPoint;
            dataPointObject.transform.localScale = new Vector3(1, 1, 1);
            dataPointObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector2.Distance(currentPoint, nextPoint), widthScaleFactor);

            Vector3 dir = currentPoint - nextPoint;

            dataPointObject.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 180);

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
