using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Game.Utility.Runtime.MenuSM
{
    public class StackStateMachine
    {
        private readonly Stack<IState> m_MenuStates;

        private event Action<IState> m_PushAction;
        private event Action<IState> m_PopAction;

        public StackStateMachine(
            Action<IState> pushAction = null, 
            Action<IState> popAction = null)
        {
            m_PushAction = pushAction;
            m_PopAction = popAction;
            m_MenuStates = new Stack<IState>();
        }

        public void PushState(IState state)
        {
            if (m_MenuStates.Count > 0)
            {
                m_MenuStates.Peek().Exit();
            }
            
            m_MenuStates.Push(state);
            state.Enter();
            m_PushAction?.Invoke(state);
        }
        
        public bool PopState()
        {
            if (m_MenuStates.Count <= 0)
            {
                return false;
            }
            
            
            var state = m_MenuStates.Pop();
            state.Exit();
            
            if (m_MenuStates.Count > 0)
            {
                m_MenuStates.Peek().Enter();
            }
            
            m_PopAction?.Invoke(state);
            return true;
        }

        public int GetCountInStack()
        {
            return m_MenuStates.Count;
        }

        public IState GetCurrentState()
        {
            return m_MenuStates.LastOrDefault();
        }
    }
}