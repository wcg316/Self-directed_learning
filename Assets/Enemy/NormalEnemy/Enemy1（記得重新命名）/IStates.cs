using UnityEngine;

// 記得重新命名不要給我叫 Enemy_

namespace Enemy1
{
	public interface IState
	{
		void Enter();
		void Execute();
		void Exit();
	}
}
