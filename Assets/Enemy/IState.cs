using UnityEngine;

public interface IState<T>
{
	void Enter();
	void Execute();
	void Exit();
}
