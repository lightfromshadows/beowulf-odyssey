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

    protected float _power;
    public float Power {
        get {
            return _power;
        }
        set {
            _power = value;
        }
    }

    protected float _hitChance = 0f;
    public float HitChance {
        get {
            return _hitChance;
        }
        set {
            _hitChance = value;
        }
    }

    protected float _health = 0f;
    public float Health {
        get {
            return _health; 
        }
        set {
            _health = Mathf.Clamp(value, 0f, MaxHealth);
        }
    }

    public void Init()
    {
        _health = MaxHealth;
        _hitChance = defaultStats.hitChance;
        _power = defaultStats.power;
        foreach (var b in PassiveBuffs)
        {
            _hitChance += b.hitUp;
            _power += b.powerUp;
        }
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

    public void AddBuff(BuffItem item)
    {
        buffs.Add(item);

        InventoryUpdated();
    }

    public void ConsumeItem(BuffItem item)
    {
        // TODO verify and notify that item is removed
        buffs.Remove(item);

        InventoryUpdated();
    }

    public bool HasBuff(BuffItem item)
    {
        return buffs.Contains(item);
    }

    private static void Default() {}
    public System.Action InventoryUpdated = Default;
}
