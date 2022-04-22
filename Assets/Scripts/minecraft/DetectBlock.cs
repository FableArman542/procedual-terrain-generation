using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectBlock : MonoBehaviour {

    public TMP_Text castedBlockText;
    public TMP_Text selectedBlockText;
    public GameObject highlight;
    private Camera camera;
    private BlockType[] blockTypes;
    private int selectedIndex = 4;

    private void Start() {
        blockTypes = new BlockType[] { BlockType.DIRT, BlockType.GRASS, BlockType.SAND, BlockType.STONE, BlockType.TNT, BlockType.WOODEN_PLANK };
        camera = GetComponent<Camera>();
    }

    private void SelectIndex() {
        if (Input.GetKeyUp(KeyCode.E)) {
            if (selectedIndex >= blockTypes.Length-1)
                selectedIndex = 0;
            else
                selectedIndex ++;
        } else if (Input.GetKeyUp(KeyCode.Q)) {
            if (selectedIndex <= 0)
                selectedIndex = blockTypes.Length-1;
            else
                selectedIndex --;
        }
    }

    private void Update() {

        SelectIndex();
        
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100000, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            
            Vector3 point = hit.point;
            Transform objectHit = hit.transform;
            Debug.Log(objectHit.name);

            try {
                ChunkGameObject chunk = objectHit.GetComponent<ChunkGameObject>();

                if (Input.GetMouseButtonDown(0)) {
                    chunk.PlaceBlock(point, blockTypes[selectedIndex]);
                } else if (Input.GetMouseButtonDown(1)) {
                    chunk.RemoveBlock(point);
                }

                string text = chunk.GetBlockType(point);
                castedBlockText.text = (text != null) ? text : "";
                selectedBlockText.text = "Selected Block: " + chunk.BlockTypeToString(blockTypes[selectedIndex]);

                chunk.HighLightBlock(point, highlight);

            } catch (UnityException e) {

            }

        }
        
    }

}
