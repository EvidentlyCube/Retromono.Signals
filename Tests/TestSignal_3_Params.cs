using System.Collections.Generic;
using NUnit.Framework;
using Retromono.Signals;

namespace Tests {
    [TestFixture]
    public class TestSignal_3_Params {
        
        [Test]
        public void ParamsAreSentToListener() {
            int[] passedParams = null;
            var signal = new Signal<int, int, int>();
            signal.AddListener((p1, p2, p3) => passedParams = new []{p1, p2, p3});
            signal.Call(13, 27, 11);
            
            CollectionAssert.AreEqual(new []{13, 27, 11}, passedParams);
        }
        
        [Test]
        public void AddedListenerIsCalled_Params() {
            var wasCalled = false;
            var signal = new Signal<int, int, int>();
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled = true);
            signal.Call(0, 0, 0);
            
            Assert.True(wasCalled);
        }
        
        [Test]
        public void AllAddedListenersAreCalled_Params() {
            var wasCalled1 = false;
            var wasCalled2 = false;
            var signal = new Signal<int, int, int>();
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled1 = true);
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled2 = true);
            signal.Call(0, 0, 0);
            
            Assert.True(wasCalled1);
            Assert.True(wasCalled2);
        }
        
        [Test]
        public void ListenersAreCalledInDescendingPriorityOrderInAscendingAgeOrder_Params() {
            var calls = new List<int>();
            var signal = new Signal<int, int, int>();
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(1), null, 10);
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(2), null, 8);
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(3), null, 9);
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(4), null, 10);
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(5), null, 7);
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(6), null, 11);
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(7), null, 8);
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(8), null, 9);
            signal.AddListener((ignore1, ignore2, ignore3) => calls.Add(9), null, 9);
            signal.Call(0, 0, 0);
            
            CollectionAssert.AreEqual(new[]{6, 1, 4, 3, 8, 9, 2, 7, 5}, calls);
        }
        
        [Test]
        public void ClearRemovesAllListeners_Params() {
            var wasCalled = false;
            var signal = new Signal<int, int, int>();
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled = true);
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled = true);
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled = true);
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled = true);
            signal.Clear();
            signal.Call(0, 0, 0);
            
            Assert.False(wasCalled);
        }
        
        [Test]
        public void ListenersShouldBeRemovedByContext_Params() {
            var wasCalled = false;
            var wasFailed = false;
            var signal = new Signal<int, int, int>();
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled = true, "test1");
            signal.AddListener((ignore1, ignore2, ignore3) => wasFailed = true, "test2");
            signal.AddListener((ignore1, ignore2, ignore3) => wasCalled = true, "test1");
            signal.AddListener((ignore1, ignore2, ignore3) => wasFailed = true, "test2");
            signal.RemoveListenerByContext("test2");
            signal.Call(0, 0, 0);
            
            Assert.True(wasCalled);
            Assert.False(wasFailed);
        }
        
    }
}