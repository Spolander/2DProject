﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBossBehaviour : StateMachineBehaviour {

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("spawnBoulders"))
        {
            Debug.Log("storm");
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (stateInfo.IsTag("punch"))
        {
           

            RockBoss boss = animator.GetComponent<RockBoss>();
            boss.LastAttackTime = Time.time;
            if (animator.GetBool("rockstorm"))
                return;

            if (boss.Cycle == RockBoss.AttackCycle.OnePunch)
                boss.Cycle = RockBoss.AttackCycle.ThrowDebris;
            else if (boss.Cycle == RockBoss.AttackCycle.MultiplePunches && animator.GetBool("punching") == false)
                boss.Cycle = RockBoss.AttackCycle.OnePunch;
        }
        else if (stateInfo.IsName("throwDebris"))
        {
            RockBoss boss = animator.GetComponent<RockBoss>();
            boss.LastAttackTime = Time.time;

            if (animator.GetBool("rockstorm"))
                return;

            boss.Cycle = RockBoss.AttackCycle.MultiplePunches;
            boss.ChangePhase(RockBoss.AttackPhase.Punch);
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMachineEnter is called when entering a statemachine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
    //
    //}

    // OnStateMachineExit is called when exiting a statemachine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
    //
    //}
}
