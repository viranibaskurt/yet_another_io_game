using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<Material> materials = new List<Material>();

    public void ChangeMaterial()
    {
        int lastUsedMatIndex = PlayerPrefs.GetInt("LastPlayedMat", 0);
        lastUsedMatIndex++;
        lastUsedMatIndex %= materials.Count;

        meshRenderer.material = materials[lastUsedMatIndex];

        PlayerPrefs.SetInt("LastPlayedMat", lastUsedMatIndex);
        PlayerPrefs.Save();
    }

    public string GetMaterialName()
    {
        return meshRenderer.material.name;
    }

    public void LoadLastPlayedMaterial()
    {
        int lastUsedMatIndex = PlayerPrefs.GetInt("LastPlayedMat", 0);
        meshRenderer.material = materials[lastUsedMatIndex];
    }
}
