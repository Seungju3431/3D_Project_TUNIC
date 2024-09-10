using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class FindMissingScripts : MonoBehaviour
{
    [MenuItem("Utility/Remove Missing Script")]
    private static void RemoveAllMissingScripts()
    {

        // 현재 활성화된 씬의 모든 루트(GameObject) 오브젝트를 가져옵니다.
        GameObject[] rootGameObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

        // 루트 오브젝트들 아래에 있는 모든 오브젝트를 수집합니다.
        Object[] allObjectsInHierarchy = EditorUtility.CollectDeepHierarchy(rootGameObjects);

        int componentCount = 0;  // 제거된 미싱 스크립트 컴포넌트의 총 개수를 기록하기 위한 변수
        int gameObjectCount = 0; // 미싱 스크립트 컴포넌트를 가지고 있던 게임 오브젝트의 수를 기록하기 위한 변수

        // 하이어라키에 있는 모든 오브젝트를 순회합니다.
        foreach (Object obj in allObjectsInHierarchy)
        {
            // 오브젝트가 게임 오브젝트인지 확인합니다.
            if (obj is GameObject go)
            {
                // 해당 게임 오브젝트에 있는 미싱 스크립트 컴포넌트의 수를 가져옵니다.
                int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);

                // 미싱 스크립트 컴포넌트가 하나 이상 있을 경우
                if (count > 0)
                {
                    // 작업을 되돌릴 수 있도록 해당 게임 오브젝트에 대한 Undo를 등록합니다.
                    Undo.RegisterCompleteObjectUndo(go, "Remove Missing Scripts");

                    // 미싱 스크립트 컴포넌트를 제거합니다.
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);

                    // 카운터를 업데이트합니다.
                    componentCount += count;
                    gameObjectCount++;
                }
            }
        }

        // 콘솔에 결과를 출력합니다.
        Debug.Log($"총 {gameObjectCount}개의 게임 오브젝트에서 {componentCount}개의 미싱 스크립트 컴포넌트를 제거.");
    }
}
