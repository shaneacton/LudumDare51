using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileArt : MonoBehaviour
{
    public List<Sprite> altSprites;
    public static float altChance = 0.2f;

    private SpriteRenderer _renderer;
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (altSprites.Count > 0)
        {
            if (Random.Range(0f, 1f) < altChance)
            {
                int altIndex = Mathf.RoundToInt(Random.Range(0f, altSprites.Count-1));
                _renderer.sprite = altSprites[altIndex];
            }
        }
    }
}
