using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Operate sprite order depend on y position.
/// </summary>
public class SpriteSorting : MonoBehaviour
{
    // Static will no change order on update, only on start
    public bool isStatic;
    // Multiplier for accuracy inreasing
    public float rangeFactor = 100f;

    // Sprites list for this object in clildren
    private Dictionary<SpriteRenderer, int> sprites = new Dictionary<SpriteRenderer, int>();

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprites.Add(sprite, sprite.sortingOrder);
        }
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        UpdateSortingOrder();
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        if (isStatic == false)
        {
            UpdateSortingOrder();
        }
    }

    /// <summary>
    /// Update sprites sorting order.
    /// </summary>
    private void UpdateSortingOrder()
    {
        foreach (KeyValuePair<SpriteRenderer, int> sprite in sprites)
        {
            sprite.Key.sortingOrder = sprite.Value - (int)(transform.position.y * rangeFactor);
        }
    }
}
