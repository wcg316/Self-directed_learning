using UnityEditorInternal;
using UnityEngine;

namespace Enemy2
{
	public class AttackState : IState<State>
	{
		Transform self;
		Transform player;

		AttackState(Transform self, Transform player)
		{
			this.self = self;
			this.player = player;
		}

		public void Enter()
		{

		}

		public void Execute(FSM<State> EnemyAI)
		{

		}

		public void Exit()
		{

		}
	}
}