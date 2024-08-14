using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public abstract class BaseModel
    {
        public int id { get; set; }

        public abstract UniTask GetDependancy();
        public abstract void Copy(BaseModel other);
    }
}
