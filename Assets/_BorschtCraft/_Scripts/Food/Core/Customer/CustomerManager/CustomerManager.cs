using UnityEngine;
using Zenject;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

namespace BorschtCraft.Food
{
    public class CustomerManager : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private int _maxCustomers = 5;
        [SerializeField] private float _spawnDelay = 2f;
        [SerializeField] private float _maxWaitTime = 15f;

        [Inject] private CustomerSpawner _spawner;
        [Inject] private SignalBus _signalBus;

        private readonly List<CustomerController> _activeCustomers = new();
        private Coroutine _spawnRoutine;

        public void Initialize()
        {
            _signalBus.Subscribe<CustomerDeliverySignal>(OnDelivery);
            _spawnRoutine = StartCoroutine(SpawnLoop());
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<CustomerDeliverySignal>(OnDelivery);
            if (this != null && _spawnRoutine != null)
                StopCoroutine(_spawnRoutine);
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                if (_activeCustomers.Count < _maxCustomers)
                {
                    var controller = _spawner.SpawnCustomer();
                    _activeCustomers.Add(controller);

                    StartCoroutine(HandleCustomerTimeout(controller, _maxWaitTime));
                }

                yield return new WaitForSeconds(_spawnDelay);
            }
        }

        private IEnumerator HandleCustomerTimeout(CustomerController controller, float timeout)
        {
            yield return new WaitForSeconds(timeout);

            if (_activeCustomers.Contains(controller))
            {
                controller.LeaveUnhappy();
                _activeCustomers.Remove(controller);
            }
        }

        private void OnDelivery(CustomerDeliverySignal signal)
        {
            var match = _activeCustomers.Find(c => c.HasMatchingOrder(signal));
            Logger.LogInfo(this, $"{nameof(OnDelivery)} received. About to find match for {signal.Item.GetType()}. Match is {match?.name}");
            var orderComplete = false;
            if (match != null)
            {
                orderComplete = match.TrySatisfyOrder(signal);
                Logger.LogInfo(this, $"{nameof(OnDelivery)}. Trying to satisfy order for {match.name}. Is satisfying: {orderComplete}");
                if (orderComplete)
                {
                    Logger.LogInfo(this, $"{nameof(OnDelivery)}. Order completed for {match.name}");
                    match.LeaveSatisfied();
                    _activeCustomers.Remove(match);
                }
            }
        }
    }
}
