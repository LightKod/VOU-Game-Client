using Owlet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VOU
{
    public class ModelCacheManager : Singleton<ModelCacheManager>
    {
        public Dictionary<string, List<BaseModel>> modelCache = new();

        public void Add(string type, List<BaseModel> models)
        {
            if (modelCache.ContainsKey(type))
            {
                List<BaseModel> currentCached = modelCache[type];
                List<BaseModel> newModel = new();
                foreach (BaseModel model in models)
                {
                    BaseModel cachedModel = currentCached.FirstOrDefault(x => x.id == model.id);
                    if (cachedModel != null)
                    {
                        //cachedModel.Copy(model);
                    }
                    else
                    {
                        newModel.Add(model);
                    }
                }
                currentCached.AddRange(newModel);
            }
            else
            {
                modelCache.Add(type, new List<BaseModel>());
            }
        }

        public void Remove(string type)
        {
            if(modelCache.ContainsKey((type)))
            {
                modelCache.Remove(type);
            }
        }
    }
}
