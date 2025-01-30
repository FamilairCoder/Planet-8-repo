using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class RandomSprite : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<RuntimeAnimatorController> anim_sprites = new List<RuntimeAnimatorController>();
    public GameObject same_as;
    public bool animated, sprite, dont_set_polygon_collider;
    

    private bool did_same;
    // Start is called before the first frame update
    void Start()
    {
        if (sprites.Count > 0 && same_as == null) 
        {

            if (GetComponent<SpriteRenderer>() != null) GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
            else GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Count)];
            if (!dont_set_polygon_collider) gameObject.AddComponent<PolygonCollider2D>();
        }

        if (anim_sprites.Count > 0)
        {
            GetComponent<Animator>().runtimeAnimatorController = anim_sprites[Random.Range(0, anim_sprites.Count)];
        }
        if (same_as != null) { StartCoroutine(SameAsRandomize()); }
    }

    // Update is called once per frame
    void Update()
    {
        if (same_as != null && !did_same)
        {

            did_same = true;
        }
    }

    public void RandomizeSprite()
    {
        did_same = false;
        if (sprites.Count > 0 && same_as == null)
        {

            if (GetComponent<SpriteRenderer>() != null) GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
            else GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Count)];
            if (!dont_set_polygon_collider) gameObject.AddComponent<PolygonCollider2D>();
        }

        if (anim_sprites.Count > 0)
        {
            GetComponent<Animator>().runtimeAnimatorController = anim_sprites[Random.Range(0, anim_sprites.Count)];
        }
    }

    private IEnumerator SameAsRandomize()
    {
        yield return new WaitForSeconds(.07f);

        if (sprite)
        {
            GetComponent<SpriteRenderer>().sprite = same_as.GetComponent<SpriteRenderer>().sprite;
            if (GetComponent<Health>() != null) GetComponent<Health>().orig_sprite = same_as.GetComponent<SpriteRenderer>().sprite;

        }
        else if (animated)
        {
            GetComponent<Animator>().runtimeAnimatorController = same_as.GetComponent<Animator>().runtimeAnimatorController;
        }
        if (!dont_set_polygon_collider) gameObject.AddComponent<PolygonCollider2D>();
        yield return null;
    }
}
