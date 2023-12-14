using System;
using System.Collections.Generic;
using UnityEngine;


    public class CraftScriptableObject : ScriptableObject
    {
        public enum CraftType
        {
            Common, Tools
        }

        public CraftType craftType;

        public ItemScriptableObject finalCraft;
        public int craftAmount;
        public int craftTime;

        public List<CraftResource> craftingResources;
    }

        [Serializable]
        public class CraftResource
        {
            public ItemScriptableObject craftObject;
            public int craftObjectAmount;
        }
    
