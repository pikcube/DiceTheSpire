using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DiceTheSpire.Shared.Extensions;


public static class CountdownExtensions
{
    extension<T>(T instance) where T : ICountdown
    {
        public void UpgradeCountdownBy(int addend)
        {
            instance.BaseCount += addend;
        }

        public int Count
        {
            get
            {
                int modCost = instance.BaseCount;
                foreach (LocalCostModifier modifier in instance.CountdownCostModifiers)
                {
                    modCost = modifier.Modify(modCost);
                }

                modCost = DiceyHooks.ModifCountdownCost(instance.AsCardModel().CombatState, instance, instance.AsCardModel(), modCost);

                return modCost;
            }
        }

        public void SetCountUntilPlayed(int cost, bool reduceOnly = false)
        {
            if (cost == 0)
            {
                return;
            }
            instance.CountdownCostModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.WhenPlayed, reduceOnly));
        }

        public void SetCountThisTurnOrUntilPlayed(int cost, bool reduceOnly = false)
        {
            if (cost == 0)
            {
                return;
            }
            instance.CountdownCostModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.WhenPlayed | LocalCostModifierExpiration.EndOfTurn, reduceOnly));
        }

        public void SetCountThisTurn(int cost, bool reduceOnly = false)
        {
            if (cost == 0)
            {
                return;
            }
            instance.CountdownCostModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.EndOfTurn, reduceOnly));
        }

        public void SetCountThisCombat(int cost, bool reduceOnly = false)
        {
            if (cost == 0)
            {
                return;
            }
            instance.CountdownCostModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.EndOfCombat, reduceOnly));
        }

        public void AddCountUntilPlayed(int amount, bool reduceOnly = false)
        {
            if (amount == 0)
            {
                return;
            }
            instance.CountdownCostModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.WhenPlayed, reduceOnly));
        }

        public void AddCountThisTurnOrUntilPlayed(int amount, bool reduceOnly = false)
        {
            if (amount == 0)
            {
                return;
            }
            instance.CountdownCostModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfTurn | LocalCostModifierExpiration.WhenPlayed, reduceOnly));
        }

        public void AddCountThisTurn(int amount, bool reduceOnly = false)
        {
            if (amount == 0)
            {
                return;
            }
            instance.CountdownCostModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfTurn, reduceOnly));
        }

        public void AddCountThisCombat(int amount, bool reduceOnly = false)
        {
            if (amount == 0)
            {
                return;
            }
            instance.CountdownCostModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfCombat, reduceOnly));
        }
    }
}