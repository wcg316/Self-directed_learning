using UnityEditorInternal;
using UnityEngine;

namespace Slime
{
	public class ChaseState : IState<State>
	{
		Transform self;
		Transform player;

		public ChaseState(Transform self, Transform player)
		{
			this.self = self;
			this.player = player;
		}

		public void Enter()
		{

		}

		public void Execute(FSM<State> EnemyAI)
		{
			// RaycastHit2D detector = Physics2D.Raycast();
		}

		public void Exit()
		{

		}
	}
}