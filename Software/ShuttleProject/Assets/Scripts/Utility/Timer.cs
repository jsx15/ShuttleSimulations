using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using Movement;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class Timer : MonoBehaviour
    {
        /// <summary>
        ///     The counter
        /// </summary>
        public GameObject counter;

        private TextMeshProUGUI _textMesh;

        /// <summary>
        ///     TestAvatarBehaviour
        /// </summary>
        public TestAvatarBehavior testAvatarBehavior;

        /// <summary>
        ///     Instruction
        /// </summary>
        public Instruction instruction;

        /// <summary>
        ///     Stopwatch
        /// </summary>
        private Stopwatch _zeit;

        private void Start()
        {
            _textMesh = counter.GetComponent<TextMeshProUGUI>();
            testAvatarBehavior.QueueFinished += StopTimer;
            instruction.QueueStart += StartTimer;
            _zeit = new Stopwatch();
        }

        /// <summary>
        ///     Start timer
        /// </summary>
        private void StartTimer()
        {
            _zeit.Reset();
            _zeit.Start();
            StartCoroutine(SetText());
        }

        /// <summary>
        ///     Update counter on GUI
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetText()
        {
            while (_zeit.IsRunning)
            {
                var delta = _zeit.Elapsed;
                _textMesh.text = delta.ToString("mm\\:ss");
                yield return new WaitForSeconds(1);
            }
        }

        /// <summary>
        ///     Stop timer and update GUI
        /// </summary>
        private void StopTimer()
        {
            StopCoroutine(SetText());
            _zeit.Stop();
            var delta = _zeit.Elapsed;
            _textMesh.text = delta.ToString("mm\\:ss");
        }
        
        
    }
}