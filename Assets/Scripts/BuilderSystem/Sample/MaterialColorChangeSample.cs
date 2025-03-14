using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorChangeSample : MonoBehaviour
{
    [SerializeField] private Color changedColor;

    private MaterialPropertyBlock _mpb;
    private MeshRenderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _mpb = new MaterialPropertyBlock();

        ChangeColor(changedColor);
    }

    void OnValidate()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _mpb = new MaterialPropertyBlock();

        ChangeColor(changedColor);
    }

    public void ChangeColor(Color color)
    {
        _mpb.SetColor("_Color", color);

        _meshRenderer.SetPropertyBlock(_mpb);
    }
}
