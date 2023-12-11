using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour
{
    public float fps = 30.0f;
    public Texture2D[] frames;

    private int frameIndex;
    private MeshRenderer rendererMy;

    void Start()
    {
        rendererMy = GetComponent<MeshRenderer>();
        rendererMy.enabled = false;
        frameIndex = 0;
    }

    

    public void StartAnimation()
    {
        StopAllCoroutines(); // Stop any ongoing animation
        StartCoroutine(PlayAnimationOnce());
    }

    IEnumerator PlayAnimationOnce()
    {
        rendererMy.enabled = true;
        for (int i = 0; i < frames.Length; i++)
        {
            rendererMy.sharedMaterial.SetTexture("_MainTex", frames[i]);
            yield return new WaitForSeconds(1 / fps);
        }

        rendererMy.enabled = false;
    }
}