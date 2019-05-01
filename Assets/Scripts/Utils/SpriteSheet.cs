using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteSheet", menuName = "RPG/Sprite Sheet", order = 101)]
[Serializable]
public class SpriteSheet : ScriptableObject {
    public Texture2D texture = null;
    public int rowCount = 1;
    public int columnCount = 1;
    public int pixelPerUnit = 100;
    public Vector2 pivot = new Vector2(0.5f, 0.5f);

    private Sprite[][] m_sprites;

    public Sprite this[int row, int column]
    {
        get { return m_sprites[row][column]; }
    }

    private void OnEnable()
    {
        m_sprites = GenerateSprites();
        //Debug.Log("Sprite sheet enabled");
    }

    private Sprite[][] GenerateSprites()
    {
        Sprite[][] sprites = null;
        if (texture != null)
        {
            int spriteWidth = texture.width / columnCount;
            int spriteHeight = texture.height / rowCount;

            // Create sprites from texture sheet
            sprites = new Sprite[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                sprites[i] = new Sprite[columnCount];
                for (int j = 0; j < columnCount; j++)
                {
                    Rect rect = new Rect(j * spriteWidth, texture.height - (i+1) * spriteHeight, spriteWidth, spriteHeight);
                    sprites[i][j] = Sprite.Create(texture, rect, pivot, pixelPerUnit, 0, SpriteMeshType.FullRect);
                }
            }
        }
        return sprites;
    }
}
