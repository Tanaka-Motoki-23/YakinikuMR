using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakinikuObjectManager : MonoBehaviour
{
    public List<YakinikuObject> yakinikuObjects;
    // Start is called before the first frame update
    void Start()
    {
        this.yakinikuObjects = new List<YakinikuObject>();
    }

    // Update is called once per frame
    void Update()
    {
        List<YakinikuObject> removeList = new List<YakinikuObject>();

        foreach (YakinikuObject yakinikuObject in this.yakinikuObjects)
        {
            if (yakinikuObject.getProgressBar().GetComponent<Countdown>().isActive == false)
            {
                removeList.Add(yakinikuObject);
            }
        }
        this.yakinikuObjects.RemoveAll(yakinikuObject => yakinikuObject.getProgressBar().GetComponent<Countdown>().isActive == false);

        foreach (YakinikuObject yakinikuObject in removeList)
        {
            Destroy(yakinikuObject.getPrefab());   
        }

    }

    public void AddYakinikuObject(DetectObject detectObject)
    {
        bool isExist = false;
        foreach (YakinikuObject yakinikuObject in this.yakinikuObjects)
        {
            if (Vector3.Distance(detectObject.getPosition(), yakinikuObject.getPosition()) <= 0.1f){
                isExist = true;
            }
        }

        if (isExist == false){

            yakinikuObjects.Add(new YakinikuObject(detectObject));
        }
    }

    private IEnumerator DeleteCoroutine(GameObject targetObj)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(targetObj);
        yield return null;
    }
}
