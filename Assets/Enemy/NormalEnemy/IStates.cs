using UnityEngine;

namespace NormalEnemy
{
	public interface IState
	{
		void Enter();
		void Execute();
		void Exit();
	}
}
