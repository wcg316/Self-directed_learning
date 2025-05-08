using UnityEngine;

public interface IState<T>
{
	void Enter();
	void Execute(FSM<T> EnemyAI);
	void Exit();
}
