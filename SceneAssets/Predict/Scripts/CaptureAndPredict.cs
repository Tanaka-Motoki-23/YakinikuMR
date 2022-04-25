using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class CaptureAndPredict : MonoBehaviour
{
    public Predict predictor;
    public YakinikuObjectManager yakinikuObjectManager;
    public TrumpObjectManager trumpObjectManager;
    public string mode = "yakiniku";

    public bool showMarker = false;
    public GameObject rectRender;
    public float confidenceThreshold = 0.5f;
    private PhotoCapture photoCaptureObject = null;
    private ImageData imageData;
    private IList<BoundingBox> boxes;

    private GameObject marker;
    private MeshFilter meshFilter;

    private Matrix4x4 viewMat;
    private Matrix4x4 projMat;
    private Vector3 cameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        this.imageData = new ImageData();
        this.imageData.setTimestamp(DateTime.Now);
        this.marker = (GameObject)Resources.Load("Marker");
        this.meshFilter = this.rectRender.GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
    }

    public void CaptureImageData()
    {
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    }

    private void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;
        this.viewMat = Camera.main.worldToCameraMatrix;
        this.projMat = Camera.main.projectionMatrix;
        this.cameraPosition = Camera.main.transform.position;
        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }


    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            photoCaptureObject.TakePhotoAsync(OnCapturedAndPredict);
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    public void OnCapturedAndPredict(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            StartCoroutine(ProcessPredict(result, photoCaptureFrame));
        }
        // Clean up
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
    private IEnumerator ProcessPredict(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        Resolution renderResolution = new Resolution();
        renderResolution.width = Camera.main.pixelWidth;
        renderResolution.height = Camera.main.pixelHeight;
        Debug.Log(renderResolution);

        Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        //Texture2D refTexture = CopyTexture(targetTexture);
        Texture2D refTexture = targetTexture;
        Texture2D saveTexture = CopyTexture(targetTexture);
        TextureScale.Scale(targetTexture, 416, 416);
        this.boxes = new List<BoundingBox>();
        yield return this.predictor.TFDetect(targetTexture);
        this.boxes = this.predictor.getBoxes();
        //Debug.Log("Captured!");
        //Debug.Log(this.boxes.Count);
        //Debug.Log(cameraResolution);
        this.boxes = this.DeleteLowConfidenceBox(this.boxes);
        this.boxes = this.DeleteDoubleBox(this.boxes);

        foreach (BoundingBox box in this.boxes)
        {
            this.DrawRect(box.Rect, refTexture);
            //this.DrawRect(AdaptRect(box.Rect, cameraResolution), refTexture);
            Rect resizedRect = this.AdaptRect(box.Rect, renderResolution);
            //Rect resizedRect = this.AdaptRect(new Rect(0, 0, 416, 416), renderResolution);
            //Rect resizedRect = box.Rect;
            Vector3 ray = this.CalcTargetRay(cameraPosition, resizedRect.center, viewMat, projMat, 1.0f, 20.0f, renderResolution);
            DetectObject detectObject = new DetectObject(box.Label, Camera.main.transform.position, ray);


            if (this.mode == "yakiniku")
            {
                this.yakinikuObjectManager.AddYakinikuObject(detectObject);
            }
            else if (this.mode == "trump")
            {
                this.trumpObjectManager.AddTrumpObject(detectObject);
            }

            if (this.showMarker == true)
            {

                Vector3 centerPositon = this.ScreenToWorld3D(resizedRect.center, viewMat, projMat, 10.0f, 1000.0f, renderResolution);
                Vector3 leftUpPosition = this.ScreenToWorld3D(new Vector2(resizedRect.xMin, resizedRect.yMin), viewMat, projMat, 10.0f, 20.0f, renderResolution);
                Vector3 leftDownPosition = this.ScreenToWorld3D(new Vector2(resizedRect.xMin, resizedRect.yMax), viewMat, projMat, 10.0f, 20.0f, renderResolution);
                Vector3 rightUpPosition = this.ScreenToWorld3D(new Vector2(resizedRect.xMax, resizedRect.yMin), viewMat, projMat, 10.0f, 20.0f, renderResolution);
                Vector3 rightDownPosition = this.ScreenToWorld3D(new Vector2(resizedRect.xMax, resizedRect.yMax), viewMat, projMat, 10.0f, 20.0f, renderResolution);


                //Instantiate(this.marker, centerPositon, Camera.main.transform.rotation);
                //Instantiate(this.marker, rightUpPosition, Camera.main.transform.rotation);
                //Instantiate(this.marker, rightDownPosition, Camera.main.transform.rotation);
                //Instantiate(this.marker, leftUpPosition, Camera.main.transform.rotation);
                //Instantiate(this.marker, leftDownPosition, Camera.main.transform.rotation);

                List<Vector3> vertices = new List<Vector3>(this.meshFilter.mesh.vertices);
                vertices.Add(centerPositon);
                vertices.Add(rightUpPosition);
                vertices.Add(rightDownPosition);
                vertices.Add(leftUpPosition);
                vertices.Add(leftDownPosition);

                List<int> triangles = new List<int>();
                for (int i = 0; i < (int)(vertices.Count / 5); i++)
                {
                    triangles.Add(1 + i * 5);
                    triangles.Add(4 + i * 5);
                    triangles.Add(2 + i * 5);

                    triangles.Add(1 + i * 5);
                    triangles.Add(3 + i * 5);
                    triangles.Add(4 + i * 5);
                }

                Mesh mesh = new Mesh();
                mesh.SetVertices(vertices);
                mesh.SetTriangles(triangles, 0);

                MeshFilter meshFilter = this.rectRender.GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;

            }

            Debug.Log(box.Label);
            Debug.Log(resizedRect.center);
            Debug.Log(detectObject.getPosition());
            Debug.DrawRay(Camera.main.transform.position, ray, Color.green, 60, false);

        }
        this.imageData.setTexture(refTexture);
        this.imageData.setPosition(Camera.main.transform.position);
        this.imageData.setTimestamp(DateTime.Now);
        yield return null;

        string filename = string.Format(@"CapturedImage{0}_n.jpg", Time.time);
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        byte[] bytes = saveTexture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
    }
    private Vector3 ScreenToWorld3D(Vector2 screenPosition, Matrix4x4 viewMat, Matrix4x4 projMat, float nearZ, float farZ, Resolution renderResolution)
    {
        //Vector3 screenPosition3D = new Vector3(screenPosition.x, screenPosition.y, 0.45f);
        //Vector3 targetPosition = Camera.main.ScreenToWorldPoint(screenPosition3D);
        Vector3 targetPosition = this.ApplyProjectionMatrix(screenPosition, viewMat, projMat, nearZ, farZ, renderResolution);
        return targetPosition;
    }

    private Vector3 CalcTargetRay(Vector3 cameraPosition, Vector2 screenPosition, Matrix4x4 viewMat, Matrix4x4 projMat, float nearZ, float farZ, Resolution renderResolution)
    {
        //Vector3 screenPosition3D = new Vector3(screenPosition.x, screenPosition.y, 10.0f);
        //Vector3 targetPosition = Camera.main.ScreenToWorldPoint(screenPosition3D);
        Vector3 targetPosition = this.ApplyProjectionMatrix(screenPosition, viewMat, projMat, nearZ, farZ, renderResolution);

        Vector3 targetRay = new Vector3(targetPosition.x - cameraPosition.x, targetPosition.y - cameraPosition.y, targetPosition.z - cameraPosition.z);
        return targetRay;
    }
    
    private Vector3 CalcTargetPosition(Vector3 cameraPosition, Vector3 targetRay)
    {
        int layerMask = LayerMask.GetMask(new string[] { "Spatial Awareness" });
        RaycastHit hitInfo;
        if (Physics.Raycast(
                cameraPosition,
                targetRay,
                out hitInfo,
                20.0f,
                layerMask)) { }

        return hitInfo.point;
    }

    private Texture2D CopyTexture(Texture2D texture)
    {
        Texture2D copiedTexture = new Texture2D(texture.width, texture.height);
        copiedTexture.SetPixels(texture.GetPixels());
        copiedTexture.Apply();
        return copiedTexture;
    }

    private void DrawRect(Rect rect, Texture2D texture)
    {
        Color color = new Color(1, 0, 0, 0.2f);
        for (int x=(int)rect.xMin; x < (int)rect.xMax; x++)
        {
            texture.SetPixel(x, (int)rect.yMin, color);
            texture.SetPixel(x, (int)rect.yMax, color);
        }
        for (int y = (int)rect.yMin; y < (int)rect.yMax; y++)
        {
            texture.SetPixel((int)rect.xMin, y, color);
            texture.SetPixel((int)rect.xMax, y, color);
        }
        texture.Apply();
    }
    private Rect AdaptRect(Rect rect, Resolution cameraResolution)
    {
        float newX = rect.x * cameraResolution.width / 416.0f;
        float newY = rect.y * cameraResolution.height / 416.0f;
        float newWidth = rect.width * cameraResolution.width / 416.0f;
        float newHeight = rect.height * cameraResolution.height / 416.0f;
        return new Rect(newX, newY, newWidth, newHeight);
    }

    public ImageData getImageData()
    {
        return this.imageData;
    }

    private Vector3 ApplyProjectionMatrix(Vector2 point, Matrix4x4 viewMat, Matrix4x4 projMat, float nearZ, float farZ, Resolution renderResolution)
    {

        Matrix4x4 viewportInv = Matrix4x4.identity;
        viewportInv.m00 = renderResolution.width / 2f;
        viewportInv.m03 = renderResolution.width / 2f;
        viewportInv.m11 = renderResolution.height / 2f;
        viewportInv.m13 = renderResolution.height / 2f;
        viewportInv.m22 = (farZ - nearZ) / 2f;
        viewportInv.m23 = (farZ + nearZ) / 2f;
        viewportInv = viewportInv.inverse;


        Matrix4x4 viewMatInv = viewMat.inverse;
        Matrix4x4 projMatInv = projMat.inverse;
        Matrix4x4 matrix = viewMatInv * projMatInv * viewportInv;

        Vector3 pos = new Vector3(point.x, point.y, nearZ);

        float x = pos.x * matrix.m00 + pos.y * matrix.m01 + pos.z * matrix.m02 + matrix.m03;
        float y = pos.x * matrix.m10 + pos.y * matrix.m11 + pos.z * matrix.m12 + matrix.m13;
        float z = pos.x * matrix.m20 + pos.y * matrix.m21 + pos.z * matrix.m22 + matrix.m23;
        float w = pos.x * matrix.m30 + pos.y * matrix.m31 + pos.z * matrix.m32 + matrix.m33;

        x /= w;
        y /= w;
        z /= w;

        return new Vector3(x, y, z);
    }

    private IList<BoundingBox> DeleteLowConfidenceBox(IList<BoundingBox> boxes)
    {
        IList<BoundingBox> activeBoxes = new List<BoundingBox>();
        foreach (BoundingBox box in boxes)
        {
            if (box.Confidence >= this.confidenceThreshold)
            {
                activeBoxes.Add(box);
            }
        }
        return activeBoxes;
    }

    private IList<BoundingBox> DeleteDoubleBox(IList<BoundingBox> boxes)
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = 0; j < boxes.Count; j++)
            {
                if (i == j) { continue; }
                if (boxes[i].isActive == false) { continue; }

                if (Vector2.Distance(boxes[i].Rect.center, boxes[j].Rect.center) <= 25.0f)
                {
                    if (boxes[i].Confidence >= boxes[j].Confidence)
                    {
                        boxes[j].isActive = false;
                    }
                    else if (boxes[i].Confidence < boxes[j].Confidence)
                    {
                        boxes[i].isActive = false;
                    }
                }
            }
        }

        IList<BoundingBox> activeBoxes = new List<BoundingBox>();
        foreach (BoundingBox box in boxes)
        {
            if (box.isActive == true)
            {
                activeBoxes.Add(box);
            }
        }
        return activeBoxes;
    }
}
