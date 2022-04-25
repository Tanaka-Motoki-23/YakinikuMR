using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Predict : MonoBehaviour
{
    public Detector detector; // ���̌��o
    private IList<BoundingBox> boxes; // ���o�����o�E���f�B���O�{�b�N�X
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

    // ���̌��o
    public IEnumerator TFDetect(Texture2D input)
    {
        if (this.isWorking == true)
        {
            Debug.Log("isWorking");
            yield return null;
        }

        this.isWorking = true;
        this.boxes = new List<BoundingBox>();

        // ���_�̎��s
        yield return StartCoroutine(this.detector.Predict(input.GetPixels32(), boxes =>
        {
            if (boxes.Count == 0)
            {
                this.isWorking = false;
                return;
            }
            this.boxes = boxes;

            // ���g�p�̃A�Z�b�g���A�����[�h
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
