using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HeroStartInfo
{
    public int team;
    public Vector2Int position;

    //public Vector3 position { get { return new Vector3(m_position.x, m_position.y); } }
}

[CreateAssetMenu(fileName = "MapData", menuName = "Data/Map")]
public class MapData : ScriptableObject
{
    public static int width = 26;
    public static int height = 16;

    public PaletteData palette = null;
    [SerializeField] private TextAsset m_data;
    [SerializeField] private TextAsset m_heroStarts;

    [HideInInspector] public int[,] data;
    [HideInInspector] public List<HeroStartInfo> heroStarts = new List<HeroStartInfo>();

    public bool Load()
    {
        bool success = false;

        if(m_data != null && m_heroStarts != null)
        {
            data = new int[width, height];
            // Load data from the file
            int tileIndex = 0;
            foreach (string number in m_data.text.Split(',', '\n'))
            {
                int index = -1;
                if (int.TryParse(number, out index))
                {
                    data[tileIndex % width, tileIndex / width] = index;
                }
                tileIndex++;
            }
            // Load heroStarts from the file
            tileIndex = 0;
            heroStarts.Clear();
            foreach (string number in m_heroStarts.text.Split(',', '\n'))
            {
                int index = -1;
                if (int.TryParse(number, out index))
                {
                    if (index >= 0)
                    {
                        heroStarts.Add(new HeroStartInfo() {
                            team = index,
                            position = new Vector2Int(tileIndex % width, tileIndex / width)
                        });
                    }
                }

                tileIndex++;
            }

            success = true;
        }

        return success;
    }
}
