using UnityEditorInternal;
using UnityEngine;

namespace Slime
{
	public class PatrolState : IState<State>
	{
		Transform self;
		Transform player;

		public PatrolState(Transform self, Transform player)
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