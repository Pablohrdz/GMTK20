using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    [SerializeField]
    private Vector2 ParallaxEffectMultiplier;
    [SerializeField]
    private bool InfiniteHorizontal;
    [SerializeField]
    private bool InfiniteVertical;

    private Transform CameraTransform;
    private Vector3 LastCameraPosition;
    private float TextureUnitSizeX;
    private float TextureUnitSizeY;

    private void Start()
    {
        CameraTransform = Camera.main.transform;
        LastCameraPosition = CameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        TextureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        TextureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = CameraTransform.position - LastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * ParallaxEffectMultiplier.x, deltaMovement.y * ParallaxEffectMultiplier.y);
        LastCameraPosition = CameraTransform.position;

        if (InfiniteHorizontal)
        {
            if (Mathf.Abs(CameraTransform.position.x - transform.position.x) >= TextureUnitSizeX)
            {
                float offsetPositionX = (CameraTransform.position.x - transform.position.x) % TextureUnitSizeX;
                transform.position = new Vector3(CameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (InfiniteVertical)
        {
            if (Mathf.Abs(CameraTransform.position.y - transform.position.y) >= TextureUnitSizeY)
            {
                float offsetPositionY = (CameraTransform.position.y - transform.position.y) % TextureUnitSizeY;
                transform.position = new Vector3(transform.position.x, CameraTransform.position.y + offsetPositionY);
            }
        }
    }

}
