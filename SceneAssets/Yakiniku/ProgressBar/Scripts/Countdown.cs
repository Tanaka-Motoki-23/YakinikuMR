using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject progressBar;
    public GameObject particle;
    public GameObject limitParticle;


    public float time;
    public float limit;
    public bool isActive = true;
    public bool isPlay = false;
    public bool isBurn = false;

    private float nowTime = 0.0f;
    private float statTime;
    private Vector3 defaultProgressBarScale;
    private Material material;
    private AudioSource audioSource;

    public void initStats()
    {
        this.material = this.progressBar.GetComponent<Renderer>().material;
        this.audioSource = this.progressBar.GetComponent<AudioSource>();
        this.audioSource.PlayOneShot(this.audioSource.clip);
        this.defaultProgressBarScale = this.progressBar.transform.localScale;

    }   
 

    public void StartCountDown()
    {
        StartCoroutine("CountDownCoroutine");
    }

    IEnumerator CountDownCoroutine()
    {
        this.statTime = Time.time;
        while (this.isActive)
        {
            this.nowTime = Time.time - this.statTime;
            if (this.nowTime >= this.limit)
            {
                if (!this.isBurn)
                {
                    Debug.Log("burn up");
                    this.limitParticle.GetComponent<ParticleSystem>().Play();
                    this.material.color = new Color(0, 0, 0);
                    this.audioSource.PlayOneShot(this.audioSource.clip);
                    this.isBurn = true;
                    yield return new WaitForSeconds(5.0f);
                    this.isActive = false;
                }
            }

            else if (this.nowTime >= this.time)
            {
                if (!this.isPlay)
                {
                    Debug.Log("count up");
                    this.particle.GetComponent<ParticleSystem>().Play();
                    this.material.color = new Color(255, 0, 0);
                    this.audioSource.PlayOneShot(this.audioSource.clip);
                    this.isPlay = true;
                }
                this.progressBar.transform.localScale = new Vector3(this.defaultProgressBarScale.x, this.defaultProgressBarScale.y * ((this.nowTime - this.time) / this.time), this.defaultProgressBarScale.z);
            }
            
            else
            {
                this.material.color = new Color(0,0,(int)(255 * ((this.time - this.nowTime) / this.time)));
                this.progressBar.transform.localScale = new Vector3(this.defaultProgressBarScale.x, this.defaultProgressBarScale.y * ((this.time - this.nowTime) / this.time), this.defaultProgressBarScale.z);
            }

            yield return new WaitForSeconds(.1f);
        }    
    }
}
