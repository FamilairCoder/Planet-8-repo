using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WreckSpriteChange : MonoBehaviour
{
    private bool under;
    private SpriteRenderer spr;
    private Color targetCol = new Color(1, 1, 1);
    private float vignette;
    public PostProcessVolume postProcessingVolume;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spr.color = Color.Lerp(spr.color, targetCol, .07f);
        if (postProcessingVolume != null) { postProcessingVolume.profile.GetSetting<Vignette>().intensity.value = Mathf.Lerp(postProcessingVolume.profile.GetSetting<Vignette>().intensity, vignette, .04f); }
        if (spr.color.r <= .65f) spr.sortingOrder = -1;
        else spr.sortingOrder = 20;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            targetCol = new Color(.6f, .6f, .6f);
            vignette = .5f;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            targetCol = Color.white;
            vignette = 0f;
        }
    }
}
