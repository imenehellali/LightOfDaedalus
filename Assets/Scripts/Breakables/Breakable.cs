using System;
using System.Collections.Generic;
using UnityEngine;

namespace Breakables
{
    public class Breakable : IRayInteractable
    {
        [SerializeField] private float m_explosionForce = 300f;
        [SerializeField] private float m_explosionRadius = 10f;
        [SerializeField] private float m_upwardsModifier = 1f;
        [SerializeField] private GameObject m_brokenPrefab;

        [SerializeField] private List<ParticleSystem> m_particleSystems;
        [SerializeField] private AudioClip m_soundEffect;

        private Color color;
        private Material mat;

        private void Awake()
        {
            m_particleSystems.ForEach(p => p.Play());
            m_particleSystems.ForEach(p => p.enableEmission = false);
            mat = GetComponent<Renderer>().material;
           
            color = ColorPalette.Instance.GetColorData.GetValueOrDefault(m_color.ToString()).GetColor();

            mat.SetColor("_EmissionColor", ColorPalette.Instance.GetColorData.GetValueOrDefault(m_color.ToString()).GetColor());
        }

        //private void Update()
        //{
        //    // Change emission strength periodically
        //    Color col = color * (0.5f + Mathf.PingPong(Time.time * 2.5f, 3));

        //    mat.SetColor("_EmissionColor", col);
        //}

        /// <summary> This variable represents the remaining required interaction time in seconds /// </summary>
        private void LateUpdate()
        {
            m_interacting = false;
        }

        #region methods

        /// <summary>
        /// If hit with the correct color, subtract from the remaining interaction time. If 0 is reached, initiate destruction.
        /// </summary>
        /// <param name="colorCode"></param>
        public override void OnHit(string colorCode, RaycastHit hit)
        {
            if (!CheckIncomingColor(colorCode))
            {
                return;
            }

            m_interacting = true;
            m_interactionTime -= Time.deltaTime;

            // Particle system
            m_particleSystems.ForEach(p => p.transform.position = hit.point + hit.normal * 0.05f);
            m_particleSystems.ForEach(p => p.transform.forward = hit.normal);
            m_particleSystems.ForEach(p => p.enableEmission = true);

            if (m_interactionTime < 0 && !m_interactionFinished)
            {
                m_interactionFinished = true;
                var replacement = Instantiate(m_brokenPrefab, transform.position, transform.rotation);
                //replacement.transform.localScale = transform.localScale;
                var rigidbodies = replacement.GetComponentsInChildren<Rigidbody>();
                AudioSource.PlayClipAtPoint(m_soundEffect, hit.point, 3f);

                foreach (var rb in rigidbodies)
                {
                    rb.AddExplosionForce(m_explosionForce, hit.point, m_explosionRadius, m_upwardsModifier);
                }

                Destroy(gameObject);
            }
        }

        public override void OnMiss()
        {
            m_particleSystems.ForEach(p => p.enableEmission = false);
        }


        protected override void ResetRayInteractable()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region getters & setters

        protected override void SetColor()
        {
            //throw new System.NotImplementedException();
        }

        #endregion
    }
}