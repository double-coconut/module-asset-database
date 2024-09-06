using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Logger = DCLogger.Runtime.Logger;


namespace Services.AssetDatabaseService
{
    public class ScriptablesDataBase<T> : ScriptableObject, IInitializable where T : ScriptableAsset
    {
        [SerializeField] private List<T> assets;

        private Dictionary<string, T> _assets;

        public List<T> Assets => assets;

        public virtual void Initialize()
        {
            if (assets.Count == 0) return;
            _assets = new Dictionary<string, T>(assets.Count);
            foreach (T asset in assets)
            {
                if (_assets == null) continue;
                if (_assets.ContainsKey(asset.Id)) continue;
                _assets.Add(asset.Id, asset);
            }
        }

        public virtual T GetResource(string id)
        {
            if (_assets == null || !_assets.ContainsKey(id))
            {
#if DC_LOGGING
                Logger.Log($"<color=red>Resource with ID : {id} not found inside of {name} DataBase!</color>",
                    AssetDatabaseLogChannels.Default);
#else
                Debug.Log($"<color=red>Resource with ID : {id} not found inside of {name} DataBase!</color>");
#endif
                return null;
            }

            return _assets[id];
        }

        public virtual bool Contains(string id)
        {
            return _assets.ContainsKey(id);
        }

        public virtual bool TryGetResource(string id, out T resource)
        {
            resource = null;
            if (_assets == null || _assets.Count == 0)
            {
                return false;
            }

            return _assets.TryGetValue(id, out resource);
        }

#if UNITY_EDITOR

        private int _assetsCount = 0;
        private void OnValidate()
        {
            if (_assetsCount == 0)
                _assetsCount = assets.Count;

            if (assets.Count == _assetsCount) return;

            Dictionary<string, T> assetsDictionary = new Dictionary<string, T>();

            foreach (T asset in assets)
            {
                if (assetsDictionary.ContainsKey(asset.Id))
                {
#if DC_LOGGING
                    Logger.LogError($"You have tried to add asset with ID : {asset.Id} which is already exist!",
                        AssetDatabaseLogChannels.Error);
#else
                    Debug.LogError($"You have tried to add asset with ID : {asset.Id} which is already exist!");
#endif
                    continue;
                }

                assetsDictionary.Add(asset.Id, asset);
            }

            List<T> assetsList = new List<T>();
            foreach (KeyValuePair<string, T> asset in assetsDictionary)
            {
                assetsList.Add(asset.Value);
            }

            assets = assetsList;
            _assetsCount = assets.Count;
        }
#endif
    }
}