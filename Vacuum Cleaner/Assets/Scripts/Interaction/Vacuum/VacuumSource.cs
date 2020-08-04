// This script should be located at the head of the vacuum cleaner.
namespace Assets.Scripts.Interaction.Vacuum
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Assets.Scripts.Audio;
    using Assets.Scripts.Input;

    [RequireComponent(typeof(SphereCollider))]
    public class VacuumSource : MonoBehaviour, IPower, IVacuumControls
    {
        // Public:
        public bool powered = false, isSucking = false, isBlowing = false; // NOTE: These are currently public for testing purposes
        [Range(5.0f, 200.0f)]public float vacuumPowerLevel = 100.0f;
        [Tooltip("In meters")] public float minimumPower = 3.0f;
        [Tooltip("In degrees")] public float effectiveAngle = 95.0f;
        [Tooltip("In meters")] public float maximumDistance = 15.0f;
        [Tooltip("In meters. The distance between the vacuum source and object to suck it into the machine")] public float eatDistance = 0.8f;
        public List<GameObject> eatenObjects; // Public for testing purposes, make private later

        [Space]public GameObject suckParticle;
        public GameObject blowParticle;
        public GameObject vacuumParticle;

        public GameObject audioComponentObject;

        // Private:
        private SphereCollider _interactionSphere;
        private int _layerMask;
        private IAudio _audioComponent;


        private void Start()
        {
            Initiate();
        }

        private void Initiate()
        {
            _interactionSphere = GetComponent<SphereCollider>();
            _interactionSphere.isTrigger = true;
            _interactionSphere.center = transform.position;
            _interactionSphere.radius = maximumDistance;

            _layerMask = LayerMask.GetMask("Player");
            _layerMask |= LayerMask.GetMask("Ignore Raycast");
            _layerMask = ~_layerMask;

            if (audioComponentObject != null)
                _audioComponent = audioComponentObject.GetComponent<IAudio>();

            eatenObjects = new List<GameObject>();
        }

        private void OnTriggerStay(Collider col)
        {
            if (!powered)
                return;

            if (isSucking)
                DoSuck(col.gameObject);
            else if (isBlowing)
                DoBlow(col.gameObject);
        }

        private void ToggleSuck(bool isOn)
        {
            if (isBlowing || !powered)
                return;

            suckParticle?.SetActive(isOn);
            AudioSuckHandler(isOn);

            isSucking = isOn;
        }

        private void ToggleBlow(bool isOn)
        {
            if (isSucking || !powered)
                return;

            blowParticle?.SetActive(isOn);
            AudioBlowHandler(isOn);

            isBlowing = isOn;
        }

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
                        eatenObjects.Add(tempObj);
                        tempObj.SetActive(false);
                        _audioComponent.Play(4);
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

        private void AudioSuckHandler(bool toggle)
        {
            if (toggle)
            {
                _audioComponent?.Play(3);
                _audioComponent?.Play(0);
                _audioComponent?.PlayWithDelay(1, 0.65f);
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
                _audioComponent?.Play(3);
                _audioComponent?.Play(0);
                _audioComponent?.PlayWithDelay(1, 0.65f);
            }
            else
            {
                _audioComponent?.Stop(1);
                _audioComponent?.Play(2);
            }
        }

        public void PowerOn()
        {
            powered = true;
            vacuumParticle?.SetActive(true);
        }

        public void PowerOff()
        {
            powered = false;
            vacuumParticle?.SetActive(false);
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
    }
}