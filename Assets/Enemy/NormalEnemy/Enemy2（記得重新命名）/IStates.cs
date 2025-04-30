using UnityEngine;

// 記得重新命名不要給我叫 Enemy_

namespace Enemy2
{
	public interface IState
	{
		void Enter();
		void Execute();
		void Exit();
	}
}
