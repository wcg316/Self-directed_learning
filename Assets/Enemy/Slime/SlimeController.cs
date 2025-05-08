using System;
using UnityEngine;

namespace Slime
{
	public class SlimeController : MonoBehaviour
	{
		FSM<State> SlimeAI = new FSM<State>();

		void Start()
		{
			LoadStates();
			SlimeAI.ChangeStateTo(State.Born);
		}

		void LoadStates()
		{
			Transform playerTransform = PlayerController.Instance.transform;

			SlimeAI.AddState(State.Born, new BornState(transform, playerTransform));
			SlimeAI.AddState(State.Partol, new PatrolState(transform, playerTransform));
			SlimeAI.AddState(State.Chase, new ChaseState(transform, playerTransform));
			SlimeAI.AddState(State.Attack, new AttackState(transform, playerTransform));
			SlimeAI.AddState(State.Die, new DieState(transform, playerTransform));
		}

		void Update()
		{
			SlimeAI.Execute();
		}
	}
}