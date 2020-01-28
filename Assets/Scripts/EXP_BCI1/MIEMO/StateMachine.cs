
namespace EXP_StateMachine
{

    public class StateMachine<T>
    {
        public State<T> currentState { get; private set;}

        public StateLogic StateLogic
        {
            get => default;
            set
            {
            }
        }

        public T Owner;


        public StateMachine(T _o) 
        {
            Owner = _o;
            currentState = null;
        }

        public void ChangeState(State<T> _newstate) 
        {
            if (currentState == _newstate) 
            {
                currentState.InState(Owner);
            }
            else 
            {
                if (currentState != null)
                    currentState.ExitState(Owner);
                currentState = _newstate;
                currentState.EnterState(Owner);
            }
        }

        public void Update() 
        {
            if (currentState != null)
                currentState.UpdateState(Owner);
        }


    }

    public abstract class State<T> {
        public StateLogic StateLogic
        {
            get => default;
            set
            {
            }
        }

        public abstract void EnterState(T _owner);
        public abstract void InState(T _owner);
        public abstract void ExitState(T _owner);
        public abstract void UpdateState(T _owner);
    }

}
