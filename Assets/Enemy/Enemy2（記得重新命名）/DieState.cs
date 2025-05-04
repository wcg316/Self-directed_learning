using UnityEditorInternal;
using UnityEngine;

namespace Enemy2
{
	public class DieState : IState<State>
	{
		Transform self;
		Transform player;

		DieState(Transform self, Transform player)
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