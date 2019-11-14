using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GenerateTerrain : MonoBehaviour
{
    public GameObject root;
    public GameObject currentBlockType;
    public int cols = 100;
    public int rows = 100;
    public float freq = 10f;
    public float amp = 10f;

    // Start is called before the first frame update
    void Start()
    {
        generate();
    }

    // Update is called once per frame
    void generate()
    {
        int process_cnt = 0;
        int block_cnt = 0;

        int sum = cols * rows;

        for (int x = 0; x < cols; ++x)
            for(int z = 0; z < rows; ++z)
            {
                process_cnt++;

                float a = Mathf.PerlinNoise(x / freq, z / freq) * amp;
                int y = (int)a;

                for(int i = 0; i <= y; ++i)
                {
                    GameObject newBlock = GameObject.Instantiate(currentBlockType, root.transform);
                    newBlock.transform.position = new Vector3(x, i, z);
                    
                    block_cnt++;
                    EditorUtility.DisplayProgressBar("Generating Block", process_cnt + " / " + sum, ((float)process_cnt / (float)sum));
                }
            }
        EditorUtility.ClearProgressBar();
        Debug.Log("block_cnt：" + block_cnt);
    }
}
