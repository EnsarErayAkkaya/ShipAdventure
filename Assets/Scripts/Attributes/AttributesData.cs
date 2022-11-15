using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Attributes
{
    public enum AttributeType
    {
        MOVE_SPEED_INCREASE_PERCENT,
        ATTACK_POWER_INCREASE_PERCENT,
        MAX_HEALTH_INCREASE_PERCENT
    }
    [System.Serializable]
    public struct AttributeData
    {
        public AttributeType AttributeType;
        public float value;
    }

    [CreateAssetMenu(fileName = "AttributeData", menuName = "Scriptable Objects/AttributeData")]
    public class AttributesData : ScriptableObject
    {
        public List<AttributeData> data;
    }
}