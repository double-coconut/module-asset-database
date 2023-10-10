using UnityEngine;

namespace Services.AssetDatabaseService
{ 
    public abstract class ScriptableAsset : ScriptableObject
    {
        [SerializeField] private string id;

        public string Id => id;
    }
}