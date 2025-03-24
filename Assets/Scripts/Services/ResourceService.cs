using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace WaterGame.Services
{
    public class ResourceService : IService
    {
        private readonly ResLoader _resLoader = ResLoader.Allocate();
        private readonly Dictionary<string, UnityEngine.Object> _cache = new Dictionary<string, UnityEngine.Object>();

        public void Initialize()
        {
            ResKit.Init();
        }

        public void Cleanup()
        {
            _resLoader.Recycle2Cache();
            _cache.Clear();
        }

        public T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            if (_cache.ContainsKey(path))
            {
                return _cache[path] as T;
            }

            var resource = _resLoader.LoadSync<T>(path);
            _cache[path] = resource;
            return resource;
        }

        public void LoadResourceAsync<T>(string path, System.Action<T> onComplete) where T : UnityEngine.Object
        {
            if (_cache.ContainsKey(path))
            {
                onComplete?.Invoke(_cache[path] as T);
                return;
            }

            _resLoader.Add2Load<T>(path, (success, result) =>
            {
                if (success)
                {
                    _cache[path] = result;
                    onComplete?.Invoke(result);
                }
            });
        }

        public void ClearCache()
        {
            _cache.Clear();
        }

        public void PreloadResources(string[] paths)
        {
            foreach (var path in paths)
            {
                _resLoader.Add2Load(path);
            }
        }
    }
} 