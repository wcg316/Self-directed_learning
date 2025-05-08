using UnityEditorInternal;
using UnityEngine;

namespace Slime
{
	public class BornState : IState<State>
	{
		Transform self;
		Transform player;

		public BornState(Transform self, Transform player)
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