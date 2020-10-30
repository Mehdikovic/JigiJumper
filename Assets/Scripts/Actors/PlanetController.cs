﻿using JigiJumper.Data;
using System;
using UnityEngine;


namespace JigiJumper.Actors
{
    public class PlanetController : MonoBehaviour
    {
        //todo delete this line
        [SerializeField] private PlanetType planetType = PlanetType.Medium;
        [SerializeField] PlanetData _planetData = null;
        

        [SerializeField] private Transform _pivot = null;

        Transform _circuit;
        bool _isVisited = false;


        public event Action<PlanetDataStructure> OnSpawnedInitialization;
        public event Action OnJumperEnter;
        public event Action OnJumperExit;
        public event Action OnPlanetDespawned;


        private void Awake()
        {
            _circuit = _pivot.GetChild(0);
        }

        public bool isVisited { get => _isVisited; set => _isVisited = value; }
        
        public Transform GetPivot() => _pivot;

        public Transform GetPivotCircuit() => _circuit;

        public void InvokeOnComponentInitialization()
        {
            //todo get info from probablility
            PlanetDataStructure data = _planetData.GetPlanetData(planetType);
            
            SetCircuitRadius(data.curcuitPosY);
            OnSpawnedInitialization?.Invoke(data);
        }

        public void InvokeOnJumperEnter()
        {
            OnJumperEnter?.Invoke();
        }

        public void InvokeOnJumperExit()
        {
            OnJumperExit?.Invoke();
        }

        public void InvokeOnPlanetDespawned()
        {
            _isVisited = false;

            OnPlanetDespawned?.Invoke();
        }

        private void SetCircuitRadius(float curcuitPosY)
        {
            _circuit.localPosition = new Vector3(0, curcuitPosY, 0);
        }
    }
}