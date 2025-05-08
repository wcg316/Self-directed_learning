using UnityEditorInternal;
using UnityEngine;

namespace Enemy3
{
	public class ChaseState : IState<State>
	{
		Transform self;
		Transform player;

		ChaseState(Transform self, Transform player)
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