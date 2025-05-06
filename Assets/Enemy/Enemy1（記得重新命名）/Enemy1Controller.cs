using System;
using UnityEngine;

namespace Enemy1
{
	public class Enemy1Controller : MonoBehaviour
	{
		FSM<State> Enemy1AI = new FSM<State>();

		void Start()
		{
			LoadStates();
			Enemy1AI.ChangeStateTo(State.Born);
		}

		void LoadStates()
		{
			Transform playerTransform = PlayerController.Instance.transform;

			Enemy1AI.AddState(State.Born, new BornState(transform, playerTransform));
			Enemy1AI.AddState(State.Partol, new PatrolState(transform, playerTransform));
			Enemy1AI.AddState(State.Chase, new ChaseState(transform, playerTransform));
			Enemy1AI.AddState(State.Attack, new AttackState(transform, playerTransform));
			Enemy1AI.AddState(State.Die, new DieState(transform, playerTransform));
		}

		void Update()
		{
			Enemy1AI.Execute();
		}
	}
}