using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.General
{
    public interface IDamageable
    {
        string ID { get; }
        bool TakeDamage(int value);
    }
}