using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class CharacterStatsObj : ScriptableObject
{
    [System.Serializable]
    public struct StatsContainer
    {
        public float power;
        public float hitChance;
        public float health;
    }

    [SerializeField] 
    private StatsContainer defaultStats = new StatsContainer() {power = 100f, hitChance = 50f, health = 80f };

    public float Power {
        get {
            return defaultStats.power;
        }
    }

    public float HitChance {
        get {
            return defaultStats.hitChance;
        }
    }

    protected float _health = 0f;
    public float Health {
        get {
            return _health; 
        }
        set {
            _health = value;
        }
    }

    public void Init()
    {
        _health = defaultStats.health;
        buffs.Clear();
    }

    public float MaxHealth {
        get {
            var healthBuffs = from b in buffs where b.passive && Mathf.Abs(b.healthUp) > float.Epsilon select b;
            float maxHP = defaultStats.health;
            foreach (var buff in healthBuffs)
            {
                maxHP += buff.healthUp;
            }
            return maxHP;
        }
    }

    public IEnumerable<BuffItem> PassiveBuffs {
        get {
            return from b in buffs where b.passive select b;
        }
    }

    public IEnumerable<BuffItem> ConsumableBuffs {
        get {
            return from b in buffs where b.consumable select b;
        }
    }

    List<BuffItem> buffs = new List<BuffItem>();
}
