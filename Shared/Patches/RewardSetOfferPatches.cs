using System.Data;
using DiceTheSpire.Inventor;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System.Reflection;
using System.Runtime.CompilerServices;
using BaseLib.Extensions;
using TempoLib.TempoLibCode;

namespace DiceTheSpire.Shared.Patches;

[TempoPatch(typeof(RewardsSet), nameof(RewardsSet.Offer))]
public static class RewardSetOfferPatches
{
    [TempoPrefixAsync]
    public static async Task BeforeRewardsAsync(RewardsSet __instance)
    {
        if (__instance.Player.Character is not TheInventor ||
            __instance.Player.RunState.CurrentRoom is not CombatRoom cr ||
            RewardSetOfferMoveNextPatches.IsEndOfRun(__instance.Player.RunState, cr) ||
            ScrapManager.GadgetIsFromThisFloor(__instance.Player)||
            ScrapManager.DebugMode == ScrapManager.ScrapDebugMode.Off)
        {
            return;
        }

        await ScrapManager.DoScrapForAsync(__instance.Player, __instance.Rewards);
    }
}





[HarmonyPatch]
public static class RewardSetOfferMoveNextPatches
{

    public static MethodBase TargetMethod()
    {
        Type stateMachine = AccessTools.DeclaredMethod(typeof(RewardsSet), nameof(RewardsSet.Offer)).StateMachineType();
        return stateMachine.DeclaredMethod("MoveNext");
    }

    public static bool Prefix(object __instance)
    {
        //State machine has a value of -1 on first execution, we'll be taking advantge of that to detect the first pass through MoveNext.
        FieldInfo state = AccessTools.DeclaredField(__instance.GetType(), "<>1__state");
        if ((int)(state.GetValue(__instance) ?? throw new NoNullAllowedException()) == -1)
        {
            FieldInfo thisField = AccessTools.DeclaredField(__instance.GetType(), "<>4__this");
            RewardsSet instance = thisField.GetValue(__instance) as RewardsSet ?? throw new NoNullAllowedException();

            if (instance.Player.Character is not TheInventor ||
                instance.Player.RunState.CurrentRoom is not CombatRoom cr ||
                IsEndOfRun(instance.Player.RunState, cr) ||
                ScrapManager.GadgetIsFromThisFloor(instance.Player) ||
                ScrapManager.DebugMode == ScrapManager.ScrapDebugMode.Off)
            {
                //Returning true in a state of -1 will start the state machine like normal
                return true;
            }

            TaskAwaiter awaiter = ScrapManager.DoScrapForAsync(instance.Player, instance.Rewards).GetAwaiter();
            if (!awaiter.IsCompleted) //We only suspend if we need to suspend
            {
                state.SetValue(__instance, -11); //We'll use -11 as our added prefix state
                AccessTools.DeclaredField(__instance.GetType(), "<>u__1").SetValue(__instance, awaiter);
                AsyncTaskMethodBuilder builder = (AsyncTaskMethodBuilder?)AccessTools.DeclaredField(__instance.GetType(), "<>t__builder").GetValue(__instance) ?? throw new NoNullAllowedException();
                IAsyncStateMachine stateMachine = (IAsyncStateMachine)__instance;
                builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
                return false; //Return false since we don't want to resume until the next time mvoe next is called
            }
            //Returning true in a state of -1 will start the state machine like normal
            return true;
        }

        //If the state is -11, we are resuming after our awaiter finished
        if ((int)(state.GetValue(__instance) ?? throw new NoNullAllowedException()) == -11)
        {
            //Ensure execution actually finished by getting the result.
            TaskAwaiter awaiter = (TaskAwaiter?)AccessTools.DeclaredField(__instance.GetType(), "<>u__1").GetValue(__instance) ?? throw new NoNullAllowedException();
            awaiter.GetResult();
            //Returning true in a state of -1 will start the state machine like normal
            state.SetValue(__instance, -1);
        }

        return true;
    }

    internal static bool IsEndOfRun(IRunState runState, CombatRoom room)
    {
        return room.RoomType == RoomType.Boss && runState.CurrentActIndex >= runState.Acts.Count - 1;
    }
}