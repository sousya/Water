using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeceneUnLockConfig", menuName = "SeceneUnLock/SceneCofig")]
public class SceneUnLockSO : ScriptableObject
{
    [SerializeField] private List<Sprite> sceneSprites;
    [SerializeField] private List<string> scenePartName;
    [SerializeField] private List<int> sceneNeedStarCount;
    [SerializeField] private List<int> scenePartNeedStar;

    public IReadOnlyList<Sprite> SceneSprites => sceneSprites;
    public IReadOnlyList<string> ScenePartName => scenePartName;
    public IReadOnlyList<int> SceneNeedStarCount => sceneNeedStarCount;
    public IReadOnlyList<int> ScenePartNeedStar => scenePartNeedStar;

}
