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


        public void ResetAllFields()
        {
            AvatarProperties.Id = string.Empty;
            AvatarProperties.Base64Image = string.Empty;
            AvatarProperties.Partner = string.Empty;
            AvatarProperties.BodyType = Core.BodyType.None;
            AvatarProperties.Gender = Core.OutfitGender.None;
            AvatarProperties.Assets = null;
        }
    }
}
