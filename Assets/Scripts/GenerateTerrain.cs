using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject Surface;
    public GameObject Inside;
    public GameObject parentNode;
    public int cols = 100;
    public int rows = 100;
    public int yMax = 7;
    public float freq = 10f;
    public float amp = 10f;

    private Dictionary<Vector3, GameObject> blockData;

    // Start is called before the first frame update
    void Awake()
    {
        blockData = new Dictionary<Vector3, GameObject>();
        generate();
    }

    // Update is called once per frame
    void generate()
    {
        int process_cnt = 0;
        int block_cnt = 0;

        int sum = cols * rows;
        int halfCol = cols / 2;
        int halfRow = rows / 2;


        float ox = Random.Range(0f, 1f);
        float oy = Random.Range(0f, 1f);

        for (int x = -halfCol; x < halfCol; ++x)
            for(int z = -halfRow; z < halfRow; ++z)
            {
                process_cnt++;


                float a = Mathf.PerlinNoise(x / freq + ox, z / freq + oy) * amp;
                int y = (int)a;

                int min = Mathf.Max(y - yMax, 0);

                for(int i = y; i >= min; --i)
                {
                    GameObject newObj = (i == y) ? Surface : Inside;

                    GameObject newBlock = GameObject.Instantiate(newObj, parentNode.transform);
                    var pos = new Vector3(Mathf.Floor(x), Mathf.Floor(i), Mathf.Floor(z));
                    newBlock.transform.position = pos;
                    blockData.Add(pos, newBlock);
                    block_cnt++;
                    EditorUtility.DisplayProgressBar("Generating Blocks", process_cnt + " / " + sum, ((float)process_cnt / (float)sum));
                }
            }
        EditorUtility.ClearProgressBar();
        Debug.Log("block_cnt：" + block_cnt);
    }

    public void OnHandleBlock(Vector3 pos)
    {
        Debug.Log(pos);
        pos.x = Mathf.Floor(pos.x);
        pos.y = Mathf.Floor(pos.y);
        pos.z = Mathf.Floor(pos.z);
        if (blockData.ContainsKey(pos))
        {
            Debug.Log("Add Block");
            Destroy(blockData[pos]);
            blockData.Remove(pos);
        }
        else
        {
            Debug.Log("Del Block");
            GameObject newBlock = GameObject.Instantiate(Surface, parentNode.transform);
            newBlock.transform.position = pos;
            blockData[pos] = newBlock;
        }
            
    }
}
