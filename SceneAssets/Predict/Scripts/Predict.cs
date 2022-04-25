using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Predict : MonoBehaviour
{
    public Detector detector; // 物体検出
    private IList<BoundingBox> boxes; // 検出したバウンディングボックス
    private bool isWorking = false;

    // Start is called before the first frame update
    void Start()
    {
        this.boxes = new List<BoundingBox>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 物体検出
    public IEnumerator TFDetect(Texture2D input)
    {
        if (this.isWorking == true)
        {
            Debug.Log("isWorking");
            yield return null;
        }

        this.isWorking = true;
        this.boxes = new List<BoundingBox>();

        // 推論の実行
        yield return StartCoroutine(this.detector.Predict(input.GetPixels32(), boxes =>
        {
            if (boxes.Count == 0)
            {
                this.isWorking = false;
                return;
            }
            this.boxes = boxes;

            // 未使用のアセットをアンロード
            Resources.UnloadUnusedAssets();
            this.isWorking = false;
        }));
        Debug.Log(this.isWorking);
        yield return null;

    }

    public IList<BoundingBox> getBoxes()
    {
        return this.boxes;
    }
}
