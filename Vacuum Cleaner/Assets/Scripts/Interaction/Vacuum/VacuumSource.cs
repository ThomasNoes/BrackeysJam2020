// This script should be located at the head of the vacuum cleaner.
namespace Assets.Scripts.Interaction.Vacuum
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    using Assets.Scripts.Audio;
    using Assets.Scripts.Input;

    [RequireComponent(typeof(SphereCollider))]
    public class VacuumSource : MonoBehaviour, IPower, IVacuumControls
    {
        // Public:
        public bool powered = false, isSucking = false, isBlowing = false; // NOTE: These are currently public for testing purposes
        [Range(5.0f, 1500.0f)]public float vacuumPowerLevel = 300.0f;
        [Tooltip("In meters")] public float minimumPower = 3.0f;
        [Tooltip("In degrees")] public float effectiveAngle = 95.0f;
        [Tooltip("In meters")] public float maximumDistance = 15.0f;
        [Tooltip("In meters. The distance between the vacuum source and object to suck it into the machine")] public float eatDistance = 0.8f;

        [Space]public GameObject suckParticle;
        public GameObject blowParticle, vacuumParticle;

        // Ref for components:
        public GameObject audioComponentObject;
        public GameObject indicatorComponentObject;

        // Events
        public event Action eatEvent;

        // Private:
        private SphereCollider _interactionSphere;
        private int _layerMask;
        private bool _suckPending, _blowPending, _effectsActive;
        private List<GameObject> _eatenObjects;

        // Interface refs:
        private IAudio _audioComponent;
        private IIndicator _indicator;

        #region Start & Initiate

        private void Start()
        {
            Initiate();
        }

        private void Initiate()
        {
            _interactionSphere = GetComponent<SphereCollider>();
            _interactionSphere.isTrigger = true;
            _interactionSphere.radius = maximumDistance;

            _layerMask = LayerMask.GetMask("Player");
            _layerMask |= LayerMask.GetMask("Ignore Raycast");
            _layerMask = ~_layerMask;

            if (audioComponentObject != null)
                _audioComponent = audioComponentObject.GetComponent<IAudio>();

            if (indicatorComponentObject != null)
                _indicator = indicatorComponentObject.GetComponent<IIndicator>();

            _eatenObjects = new List<GameObject>();
        }

        #endregion

        private void OnTriggerStay(Collider col)
        {
            if (!powered)
            {
                SetToDisable();
                return;
            }

            if (isSucking)
                DoSuck(col.gameObject);
            else if (isBlowing)
                DoBlow(col.gameObject);
        }

        #region Toggle

        private void ToggleSuck(bool isOn)
        {
            _suckPending = isOn;

            if (isBlowing)
                return;

            if (isOn)
                _audioComponent?.Play(3);

            if (!powered)
                return;

            ToggleSuckEffects(isOn);
            isSucking = isOn;

            if (!isOn && _blowPending)
            {
                isBlowing = true;
                ToggleBlowEffects(true);
            }
        }

        private void ToggleBlow(bool isOn)
        {
            _blowPending = isOn;

            if (isSucking)
                return;

            if (isOn)
                _audioComponent?.Play(3);

            if (!powered)
                return;

            ToggleBlowEffects(isOn);
            isBlowing = isOn;

            if (!isOn && _suckPending)
            {
                isSucking = true;
                ToggleSuckEffects(true);
            }
        }

        #endregion

        private void DoSuck(GameObject obj)
        {
            ISuckable suckable = obj.GetComponent<ISuckable>();
            IEatable eatable = obj.GetComponent<IEatable>();

            var tempDistance = Vector3.Distance(transform.position, obj.transform.position);

            if (eatable != null)
            {
                if (tempDistance < eatDistance)
                {
                    GameObject tempObj = eatable.Eat();

                    if (tempObj != null)
                    {
                        _eatenObjects.Add(tempObj);
                        tempObj.SetActive(false);
                        _audioComponent?.Play(4); // Play eat sound
                        eatEvent?.Invoke(); // Invoke eat event
                        return;
                    }
                }
            }

            if (suckable != null)
            {
                if (InRangeCheck(gameObject, obj))
                {
                    var tempSuckForce = vacuumPowerLevel - tempDistance;

                    if (tempSuckForce < minimumPower)
                        tempSuckForce = minimumPower;

                    suckable.Suck(transform.position, tempSuckForce);
                }
            }
        }

        private void DoBlow(GameObject obj)
        {
            IBlowable blowable = obj.GetComponent<IBlowable>();

            if (blowable != null)
            {
                var tempDistance = Vector3.Distance(transform.position, obj.transform.position);

                if (InRangeCheck(gameObject, obj))
                {
                    var tempBlowForce = vacuumPowerLevel - tempDistance;

                    if (tempBlowForce < minimumPower)
                        tempBlowForce = minimumPower;

                    blowable.Blow(transform.position, tempBlowForce);
                }
            }
        }

        private bool InRangeCheck(GameObject from, GameObject to)
        {

            Vector3 dir = to.transform.position - from.transform.position;
            float vacuumToSuckableAngle = Vector3.Angle(from.transform.forward, dir);

            if (vacuumToSuckableAngle < effectiveAngle / 2)
            {
                RaycastHit hit;

                if (Physics.Raycast(from.transform.position, dir, out hit, maximumDistance + 2.0f, _layerMask))
                {
                    if (hit.collider.gameObject == to)
                        return true;
                }
            }

            return false;
        }

        #region Effect Controllers

        private void ToggleSuckEffects(bool isOn)
        {
            if (_effectsActive && isOn)
                return;

            suckParticle?.SetActive(isOn);
            AudioSuckHandler(isOn);

            _effectsActive = isOn;
        }

        private void ToggleBlowEffects(bool isOn)
        {
            if (_effectsActive && isOn)
                return;

            blowParticle?.SetActive(isOn);
            AudioBlowHandler(isOn);

            _effectsActive = isOn;
        }

        #endregion

        #region AudioHandlers
        private void AudioSuckHandler(bool toggle)
        {
            if (toggle)
            {
                _audioComponent?.Play(0);
                _audioComponent?.PlayWithDelay(1, 0.71f);
            }
            else
            {
                _audioComponent?.Stop(1);
                _audioComponent?.Play(2);
            }
        }

        private void AudioBlowHandler(bool toggle)
        {
            if (toggle)
            {
                _audioComponent?.Play(5);
                _audioComponent?.PlayWithDelay(6, 0.55f);
            }
            else
            {
                _audioComponent?.Stop(6);
                _audioComponent?.Play(7);
            }
        }
        #endregion

        #region Control Functions

        public void PowerOn()
        {
            powered = true;
            vacuumParticle?.SetActive(true);
            _indicator?.IndicatorOn();
        }

        public void PowerOff()
        {
            powered = false;
            vacuumParticle?.SetActive(false);
            _indicator?.IndicatorOff();
        }

        public void StartSuck()
        {
            ToggleSuck(true);
        }

        public void StopSuck()
        {
            ToggleSuck(false);
        }

        public void StartBlow()
        {
            ToggleBlow(true);
        }

        public void StopBlow()
        {
            ToggleBlow(false);
        }

        private void SetToDisable()
        {
            if (_blowPending)
                _blowPending = false;
            if (_suckPending)
                _suckPending = false;
            if (_effectsActive)
            {
                AudioBlowHandler(false);
                AudioSuckHandler(false);
                ToggleBlowEffects(false);
                ToggleSuckEffects(false);
                _effectsActive = false;
            }
            if (isBlowing)
                isBlowing = false;
            if (isSucking)
                isSucking = false;
        }

        #endregion
    }
}