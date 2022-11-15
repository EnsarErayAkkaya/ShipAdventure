using EEA.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Managers
{
    public class AttributeManager : MonoBehaviour
    {
        [SerializeField] private AttributesData attributesData;

        private Dictionary<string, List<AttributeData>> attributes = new Dictionary<string, List<AttributeData>>();

        public Dictionary<string, List<AttributeData>> Attributes => attributes;

        public void Add(string key, AttributeType type)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes[key] = new List<AttributeData>();
            }
            attributes[key].Add(attributesData.data.Find(s => s.AttributeType == type));
        }
    }
}