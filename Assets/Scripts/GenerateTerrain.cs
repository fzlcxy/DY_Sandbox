using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject Surface;
    public GameObject Inside;
    public GameObject Water;
    public GameObject Tree;
    public GameObject parentNode;
    public int cols = 100;
    public int rows = 100;
    public float treeRates = 0.1f;
    public int yWater = 4;
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
                float a = Mathf.PerlinNoise(x / freq + ox, z / freq + oy) * amp;
                int y = (int)a;

                int min = Mathf.Max(y - yMax, 0);

                for(int i = y; i >= min; --i)
                {
                    GameObject newObj = (i == y) ? Surface : Inside;

                    if(i == y && y > yWater)
                    {
                        newObj = Surface;
                        {
                            float haveTree = Random.Range(0f, 1f);
                            if (haveTree < treeRates)
                            {
                                GameObject newTree = GameObject.Instantiate(Tree, parentNode.transform);
                                var treePos = new Vector3(Mathf.Floor(x), Mathf.Floor(i+1), Mathf.Floor(z));
                                newTree.transform.position = treePos;
                                dealWithTreeData(newTree, ref block_cnt);
                            }
                        }
                    }
                    else{
                        newObj = Inside;
                    }

                    GameObject newBlock = GameObject.Instantiate(newObj, parentNode.transform);
                    var pos = new Vector3(Mathf.Floor(x), Mathf.Floor(i), Mathf.Floor(z));
                    newBlock.transform.position = pos;
                    blockData.Add(pos, newBlock);
                    block_cnt++;
                }

                if (y < yWater)
                {
                    for (int i = y + 1; i <= yWater; ++i)
                    {
                        GameObject newWater = GameObject.Instantiate(Water, parentNode.transform);
                        var posWater = new Vector3(Mathf.Floor(x), Mathf.Floor(i), Mathf.Floor(z));
                        newWater.transform.position = posWater;
                        block_cnt++;
                    }
                }
                process_cnt++;
                EditorUtility.DisplayProgressBar("Generating Blocks", process_cnt + " / " + sum, ((float)process_cnt / (float)sum));
            }
        EditorUtility.ClearProgressBar();
        Debug.Log("block_cnt：" + block_cnt);
    }

    public void dealWithTreeData(GameObject Tree, ref int block_cnt)
    {
        for(int i = 0; i < Tree.transform.childCount; ++i)
        {
            GameObject child = Tree.transform.GetChild(i).gameObject;
            blockData.Add(child.transform.position, child);
            block_cnt++;
        }
    }

    public void OnHandleBlock(Vector3 pos)
    {
        Debug.Log(pos);
        pos.x = Mathf.Floor(pos.x);
        pos.y = Mathf.Floor(pos.y);
        pos.z = Mathf.Floor(pos.z);
        if (blockData.ContainsKey(pos))
        {
            Debug.Log("Del Block " + blockData[pos]);
            Destroy(blockData[pos]);
            blockData.Remove(pos);
        }
        else
        {
            Debug.Log("Add Block");
            GameObject newBlock = GameObject.Instantiate(Surface, parentNode.transform);
            newBlock.transform.position = pos;
            blockData[pos] = newBlock;
        }
            
    }
}
