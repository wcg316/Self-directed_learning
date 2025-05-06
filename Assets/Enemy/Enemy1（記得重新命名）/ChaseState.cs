using UnityEditorInternal;
using UnityEngine;

namespace Enemy1
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

		public void Execute(FSM<State> fsm)
		{

		}

		public void Exit()
		{

		}
	}
}