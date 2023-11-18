using System.Collections.Generic;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine;

namespace ReadyPlayerMe
{
    [CreateAssetMenu(fileName = "AvatarCreatorData", menuName = "Ready Player Me/Avatar Creator Data", order = 1)]
    public class AvatarCreatorData : ScriptableObject
    {
        public AvatarProperties AvatarProperties;
        public bool IsExistingAvatar;

        public void Awake()
        {
            AvatarProperties = new AvatarProperties();
        }

        // [ContextMenu("StartSerialization")]
        // public void StartSerialization()
        // {
        //     string json = JsonUtility.ToJson(AvatarProperties, true);
        //     Debug.Log(json);
        //     if (AvatarProperties.Assets.Count >= 1)
        //     {
        //         foreach (KeyValuePair<Category, object> item in AvatarProperties.Assets)
        //         {
        //             Debug.Log($"{item.Key} {item.Value}");
        //         }
        //     }
        // }
    }
}
