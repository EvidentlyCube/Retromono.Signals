using System.Collections.Generic;
using NUnit.Framework;
using Retromono.Signals;

namespace Tests {
    [TestFixture]
    public class TestSignal_0 {
        
        [Test]
        public void AddedListenerIsCalled() {
            var wasCalled = false;
            var signal = new Signal();
            signal.AddListener(() => wasCalled = true);
            signal.Call();
            
            Assert.True(wasCalled);
        }
        
        [Test]
        public void AllAddedListenersAreCalled() {
            var wasCalled1 = false;
            var wasCalled2 = false;
            var signal = new Signal();
            signal.AddListener(() => wasCalled1 = true);
            signal.AddListener(() => wasCalled2 = true);
            signal.Call();
            
            Assert.True(wasCalled1);
            Assert.True(wasCalled2);
        }
        
        [Test]
        public void ListenersAreCalledInDescendingPriorityOrderInAscendingAgeOrder() {
            var calls = new List<int>();
            var signal = new Signal();
            signal.AddListener(() => calls.Add(1), null, 10);
            signal.AddListener(() => calls.Add(2), null, 8);
            signal.AddListener(() => calls.Add(3), null, 9);
            signal.AddListener(() => calls.Add(4), null, 10);
            signal.AddListener(() => calls.Add(5), null, 7);
            signal.AddListener(() => calls.Add(6), null, 11);
            signal.AddListener(() => calls.Add(7), null, 8);
            signal.AddListener(() => calls.Add(8), null, 9);
            signal.AddListener(() => calls.Add(9), null, 9);
            signal.Call();
            
            CollectionAssert.AreEqual(new[]{6, 1, 4, 3, 8, 9, 2, 7, 5}, calls);
        }
        
        [Test]
        public void ClearRemovesAllListeners() {
            var wasCalled = false;
            var signal = new Signal();
            signal.AddListener(() => wasCalled = true);
            signal.AddListener(() => wasCalled = true);
            signal.AddListener(() => wasCalled = true);
            signal.AddListener(() => wasCalled = true);
            signal.Clear();
            signal.Call();
            
            Assert.False(wasCalled);
        }
        
        [Test]
        public void ListenersShouldBeRemovedByContext() {
            var wasCalled = false;
            var wasFailed = false;
            var signal = new Signal();
            signal.AddListener(() => wasCalled = true, "test1");
            signal.AddListener(() => wasFailed = true, "test2");
            signal.AddListener(() => wasCalled = true, "test1");
            signal.AddListener(() => wasFailed = true, "test2");
            signal.RemoveListenerByContext("test2");
            signal.Call();
            
            Assert.True(wasCalled);
            Assert.False(wasFailed);
        }
        
    }
}