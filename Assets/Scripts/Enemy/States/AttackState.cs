using System.Collections;
using UnityEngine;

public class AttackState : BaseState
{
    private float timeElapsed;
    private float attackCooldown;
    private bool isAttacking = false;
    private bool isMovingBack = false;

    public override void Enter()
    {
        npc.Agent.SetDestination(npc.transform.position);
        npc.Agent.speed = 0f;
        attackCooldown = npc.parameters.behaviour.attackCooldown;
        timeElapsed = attackCooldown;
    }

    public override void Perform()
    {
        timeElapsed += Time.deltaTime;

        ModelRotation();

        if (isMovingBack)
            MoveBack();
        else
            AttackLogic();
    }

    public override void Exit()
    {

    }

    private void ModelRotation()
    {
        Vector3 playerDirection = (npc.Player.position - npc.transform.position).normalized;
        float targetRotation = Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg;

        npc.transform.rotation = Quaternion.Euler(new Vector3(0, targetRotation, 0));
    }

    private void AttackLogic()
    {
        if (isAttacking) return;

        if (npc.PlayerDistance > 1.1f)
        {
            stateMachine.ChangeState(new ChaseState());
        }
        else if (timeElapsed > attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        timeElapsed = 0;
        isAttacking = true;
        npc.CharacterAnimator.SetTrigger("OnAttack");

        Debug.Log("I am attacking you!"); //Put damage logick here

        npc.StartCoroutine(ResetAttack());
    }

    private void MoveBack()
    {
        if (npc.Agent.remainingDistance < 0.1f || npc.PlayerDistance > 2f)
        {
            stateMachine.ChangeState(new ChaseState());
        }
    }

    private void MoveBackChance()
    {
        if (isMovingBack || npc.PlayerDistance > 1.1f) return;

        float notAttackChance = Random.value;

        if (notAttackChance < 0.35f)
        {
            isMovingBack = true;
            
            Vector3 newDestination = npc.transform.forward * -1f;
            Vector3 randomSpread = npc.transform.right * Random.Range(-1f, 1f);
            npc.Agent.SetDestination(npc.transform.position + newDestination + randomSpread);
            npc.Agent.speed = 1.5f;
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        MoveBackChance();
    }
}