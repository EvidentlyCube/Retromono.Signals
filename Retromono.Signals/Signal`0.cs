using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Retromono.Signals {
    /// <summary>
    /// Signals are like C# events on steroids and without having to check for null before calling them.
    /// 
    /// A basic signal class which supports adding listeners with:
    ///  - Optional context, which allows removing all of the listeners for a given context
    ///  - Priority, the higher the earlier the signal is fired 
    /// </summary>
    public class Signal {
        private const int DefaultPriority = 0;
        private readonly List<CallbackContainer> _callbackContainers;

        public Signal() {
            _callbackContainers = new List<CallbackContainer>();
        }

        /// <summary>
        /// Adds an action as a listener to this signal
        /// </summary>
        /// <param name="callback">Action to invoke when the signal is called</param>
        /// <param name="context">Optional context to assign to that action to allow easy removing all of the listeners from the same context</param>
        /// <param name="priority">Priority, actions are called in the order of priority, higher being first. Ties are broken by age, younger
        /// are called earlier.</param>
        public void AddListener(Action callback, object context = null, int priority = DefaultPriority) {
            Contract.Requires(callback != null);
            _callbackContainers.Add(new CallbackContainer(callback, context, priority));
            _callbackContainers.Sort((left, right) => (int) (left.Priority == right.Priority ? left.Age - right.Age : right.Priority - left.Priority));
        }

        /// <summary>
        /// Removes the specific action from the listening list
        /// </summary>
        /// <param name="callback">Action to be removed</param>
        public void RemoveListener(Action callback) {
            Contract.Requires(callback != null);
            _callbackContainers.RemoveAll(x => x.EmptyCallback == callback);
        }

        /// <summary>
        /// Removes all the actions attached to the specific context
        /// </summary>
        /// <param name="context">Context by which to remove actions</param>
        public void RemoveListenerByContext(object context) {
            _callbackContainers.RemoveAll(x => x.Context == context);
        }

        /// <summary>
        /// Calls all of the actions listening to the signal
        /// </summary>
        public void Call() {
            foreach (var callback in _callbackContainers.ToArray()) {
                callback.Invoke();
            }
        }

        /// <summary>
        /// Removes all of the actions listening to the signal
        /// </summary>
        public void Clear() {
            _callbackContainers.Clear();
        }

        private struct CallbackContainer {
            public readonly object Context;
            public readonly Action EmptyCallback;
            public readonly int Priority;
            public readonly float Age;

            public CallbackContainer(Action emptyCallback, object context, int priority) {
                Contract.Requires(emptyCallback != null);

                EmptyCallback = emptyCallback;
                Context = context;
                Priority = priority;
                Age = DateTime.Now.Ticks;
            }

            public void Invoke() {
                Contract.Requires(EmptyCallback != null);

                EmptyCallback.Invoke();
            }
        }
    }
}