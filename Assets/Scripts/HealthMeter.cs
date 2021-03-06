﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMeter : MonoBehaviour
{
    [System.Serializable]
    public class FaceSpec
    {
        [Range(0, 1)]
        public float Health;
        public Sprite Face;
    }

    [Range(0, 1)]
    public float Health;
    public int AngleRange;
    public int AngleOffset;
    public UnityEngine.UI.Image Needle;
    public UnityEngine.UI.Image Face;
    public List<FaceSpec> FaceSpecs;
    public float CurrentTarget;
    public float HealthBarSpeed = 0.5f;
    public Sprite SuccessFace;
    public Sprite RejectFace; 
    private Sprite overrideFace;
    private float overrideFaceDuration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (overrideFaceDuration > 0)
        {
            overrideFaceDuration -= Time.deltaTime;
            Face.sprite = overrideFace;
        }
        else
        {
            UpdateSprites();
        }
    }

    void OnValidate()
    {
        UpdateSprites();
    }

    public void OverrideFace(Sprite sprite, float duration)
    {
        overrideFaceDuration = duration;
        overrideFace = sprite;
    }

    void UpdateSprites()
    {
        var target = Mathf.Lerp(AngleRange, -AngleRange, Health) - AngleOffset;
        CurrentTarget = Mathf.Lerp(CurrentTarget, target, HealthBarSpeed);
        Needle.rectTransform.rotation = Quaternion.AngleAxis(CurrentTarget, Vector3.forward);

        FaceSpec activeSpec = null;
        foreach (var spec in FaceSpecs)
        {
            if (Health <= spec.Health)
            {
                activeSpec = spec;
            }
        }
        if (activeSpec != null)
            Face.sprite = activeSpec.Face;
        else
            Debug.Log($"No face spec for health {Health}");
    }
}
