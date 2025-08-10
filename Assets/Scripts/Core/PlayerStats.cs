using System;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    Health,
    Food,
    Water,
    Strength,
    Agility,
    Intelligence
}

public enum StatModType
{
    Flat,       // Add or subtract fixed number
    PercentAdd, // Additive percentage (10% + 5% = +15%)
    PercentMult // Multiplicative percentage (x1.1 then x1.2)
}

[Serializable]
public class StatModifier
{
    public Stat stat;
    public float value;
    public StatModType modifierType;
    public object source; // Optional: to track where it came from (equipment, buff, etc.)

    public StatModifier(Stat type, float val, StatModType modType, object src = null)
    {
        stat = type;
        value = val;
        modifierType = modType;
        source = src;
    }
}

[Serializable]
public class PlayerStats
{
    private Dictionary<Stat, float> baseStats = new Dictionary<Stat, float>();

    private List<StatModifier> modifiers = new List<StatModifier>();

    public void InitializeBaseStats(Dictionary<Stat, float> initialStats)
    {
        baseStats = new Dictionary<Stat, float>(initialStats);
        // Don't need to raise StatsChanged event here since its initialized only
    }

    public void SetBaseStat(Stat type, float value)
    {
        baseStats[type] = value;
        EventManager.Raise(GameEvent.StatsChanged);
    }

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        EventManager.Raise(GameEvent.StatsChanged);
    }

    public void RemoveModifier(StatModifier mod)
    {
        modifiers.Remove(mod);
        EventManager.Raise(GameEvent.StatsChanged);
    }

    public float GetFinalStat(Stat type)
    {
        float baseValue = baseStats[type];
        float flatAdd = 0f;
        float percentAdd = 0f;
        float percentMult = 1f;

        foreach (var mod in modifiers)
        {
            if (mod.stat != type) continue;

            switch (mod.modifierType)
            {
                case StatModType.Flat:
                    flatAdd += mod.value;
                    break;

                case StatModType.PercentAdd:
                    percentAdd += mod.value;
                    break;

                case StatModType.PercentMult:
                    percentMult *= 1 + mod.value;
                    break;
            }
        }

        float result = (baseValue + flatAdd) * (1 + percentAdd) * percentMult;
        return Mathf.Round(result * 100f) / 100f; // Rounded to 2 decimals
    }

    public override string ToString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine($"<b><color=#8B4513>[Player Stats]</color></b>");

        foreach (var stat in baseStats)
        {
            float finalValue = GetFinalStat(stat.Key);
            sb.AppendLine($"  • {stat.Key}: {finalValue}");
        }

        return sb.ToString();
    }
}
