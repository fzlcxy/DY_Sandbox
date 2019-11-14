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
    public float offset = 0f;
    public float amp = 10f;

    // Start is called before the first frame update
    void Awake()
    {
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


        float rdx = Random.Range(1, freq);
        float rdy = Random.Range(1, freq);

        for (int x = -halfCol; x < halfCol; ++x)
            for(int z = -halfRow; z < halfRow; ++z)
            {
                process_cnt++;


                float a = Mathf.PerlinNoise(x / freq + offset, z / freq + offset) * amp;
                int y = (int)a;

                int min = Mathf.Max(y - yMax, 0);

                for(int i = y; i >= min; --i)
                {
                    GameObject newObj = (i == y) ? Surface : Inside;

                    GameObject newBlock = GameObject.Instantiate(newObj, parentNode.transform);
                    newBlock.transform.position = new Vector3(x, i, z);
                    
                    block_cnt++;
                    EditorUtility.DisplayProgressBar("Generating Blocks", process_cnt + " / " + sum, ((float)process_cnt / (float)sum));
                }
            }
        EditorUtility.ClearProgressBar();
        Debug.Log("block_cnt：" + block_cnt);
    }
}
