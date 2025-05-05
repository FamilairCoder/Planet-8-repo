using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
//using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{

    public PostProcessVolume postProcessingVolume; // Assign in Inspector    

    public static float abberation;
    private Bloom bloomSetting;   
    // Start is called before the first frame update
    void Start()
    {
        bloomSetting = postProcessingVolume.profile.GetSetting<Bloom>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (postProcessingVolume.profile.TryGetSettings(out bloom))
        //{
        bloomSetting.intensity.value = PlayerPrefs.GetFloat("bloom intensity", 0.4f);
        bloomSetting.threshold.value = PlayerPrefs.GetFloat("bloom threshold", 0.7f);

        //postProcessingVolume.profile.GetSetting<ChromaticAberration>().intensity.value = abberation;
            //bloom.intensity.value += 1 * Time.deltaTime;
        //}


        //Volume pp = GetComponent<Volume>();
        //pp.profile.TryGet(out bloom).int = SaveManager.GetFloat("bloom intensity", .4f);
        //bloomThreshold = SaveManager.GetFloat("bloom threshold", .7f);
    }
}
