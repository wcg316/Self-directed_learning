using UnityEditorInternal;
using UnityEngine;

namespace Enemy3
{
	public class BornState : IState<State>
	{
		Transform self;
		Transform player;

		BornState(Transform self, Transform player)
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