using UnityEditorInternal;
using UnityEngine;

namespace Slime
{
	public class AttackState : IState<State>
	{
		Transform self;
		Transform player;

		public AttackState(Transform self, Transform player)
		{
			this.self = self;
			this.player = player;
		}

		public void Enter()
		{

		}

		public void Execute(FSM<State> fsm)
		{

		}

		public void Exit()
		{

		}
	}
}