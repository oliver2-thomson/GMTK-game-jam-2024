using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseBlock))]
public class BaseBlockEditor : Editor
{
    public bool showLevels = true;
    BaseBlock targetScript;

    void OnEnable()
    {
        targetScript = target as BaseBlock;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("BlockWidth");
        targetScript.BlockHeight = EditorGUILayout.IntField(targetScript.BlockHeight);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("BlockHeight");
        targetScript.BlockWidth = EditorGUILayout.IntField(targetScript.BlockWidth);
        EditorGUILayout.EndHorizontal();

        showLevels = EditorGUILayout.Foldout(showLevels, "Attachment Points");
        if (showLevels)
        {
            int middleX = targetScript.blockSlots.GetLength(0) / 2;
            int middleY = targetScript.blockSlots.GetLength(1) / 2;

            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < targetScript.BlockHeight + 2; y++) 
            {
                EditorGUILayout.BeginVertical();
                for (int x = 0; x < targetScript.BlockWidth + 2; x++)
                {

                    if (x == middleX && y == middleY)
                    {
                        EditorGUILayout.LabelField("Block");
                    }
                    else
                    {
                        bool oldValue = targetScript.blockSlots[x, y];
                        targetScript.blockSlots[x, y] = EditorGUILayout.Toggle(targetScript.blockSlots[x, y]);

                        if (oldValue != targetScript.blockSlots[x, y])
                        {
                            EditorUtility.SetDirty(targetScript);
                        }
                    }
                    /*
                    bool isMiddle = (x == middleX && y == middleY) ||
                                (x == middleX && y == middleY + targetScript.BlockWidth - 1) ||
                                (x == middleX + targetScript.BlockHeight - 1 && y == middleY) ||
                                (x == middleX + targetScript.BlockHeight - 1 && y == middleY + targetScript.BlockWidth - 1);
                    bool isCorner = (x == 0 || x == targetScript.blockSlots.GetLength(0)) && (y == 0 || y == targetScript.blockSlots.GetLength(1));

                    if (!isCorner)
                    {
                        if (x == middleX && y == middleX  && targetScript.BlockHeight > 2 && targetScript.BlockWidth > 2)
                        {
                            EditorGUILayout.LabelField("BLOCK");
                        }
                        else
                        {
                            
                        }
                    }*/
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}

