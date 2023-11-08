using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingObject : MonoBehaviour
{
    private string resourceMaterialName ="Material/Stun"; // Resources 폴더 내의 Material 이름
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material originalMaterial; // 원래 Material을 저장할 변수
    public float swapDuration = 5f; // Material을 변경할 시간 (초)
    public float totalDuration = 30f; // Coroutine이 실행될 총 시간 (초)
    Material[] materials;
    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        materials = skinnedMeshRenderer.materials;
        originalMaterial = materials[0];


    }
    public void StartSwapMaterialCoroutine()
    {
        StartCoroutine(SwapMaterialCoroutine());
    }
    IEnumerator SwapMaterialCoroutine()
    {
        // Resources 폴더에서 Material을 로드합니다.
        Material newMaterial = Resources.Load<Material>(resourceMaterialName);

        if (newMaterial == null)
        {
            Debug.LogError("Material not found in Resources folder.");
            yield break;
        }

        float startTime = Time.time; // Coroutine 시작 시간

        // 설정한 총 시간이 지날 때까지 반복합니다.
        while (Time.time - startTime < totalDuration)
        {
            Debug.Log("변");
            // 새 Material로 변경합니다.
            materials[0] = newMaterial;
            skinnedMeshRenderer.materials = materials;

            // 지정된 시간 동안 기다립니다.
            yield return new WaitForSeconds(swapDuration);

            // 총 실행 시간이 지났는지 다시 확인합니다.
            if (Time.time - startTime >= totalDuration) break;

            // 원래의 Material로 되돌립니다.
            // 새 Material로 변경합니다.
            materials[0] = originalMaterial;
            skinnedMeshRenderer.materials = materials;

            // 같은 시간만큼 다시 기다립니다.
            yield return new WaitForSeconds(swapDuration);
        }

        // Coroutine이 끝나면 원래 Material로 되돌립니다.
        materials[0] = originalMaterial;
        skinnedMeshRenderer.materials = materials;
    }

}
