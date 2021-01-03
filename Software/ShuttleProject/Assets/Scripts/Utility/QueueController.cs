using System.Collections.Generic;
using MMIStandard;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class QueueController : MonoBehaviour
    {
        /// <summary>
        ///     View to display instructions
        /// </summary>
        public GameObject scrollViewContent;
        
        /// <summary>
        ///     Queue of instructions
        /// </summary>
        private readonly List<MInstruction> _mInstructions = new List<MInstruction>();

        /// <summary>
        ///     Add instructions to queue
        /// </summary>
        /// <param name="instruction">Instruction to add</param>
        /// <param name="instructionText">Text to display</param>
        public void AddItem(MInstruction instruction, string instructionText)
        {
            _mInstructions.Add(instruction);
            
            //Instantiate game object to display text 
            var text = Instantiate(Resources.Load("UI/ScrollViewInstruction"), scrollViewContent.transform) as GameObject;
            if (!(text is null)) text.GetComponent<TextMeshProUGUI>().text = instructionText;
        }

        /// <summary>
        ///     Add instructions to queue
        /// </summary>
        /// <param name="instructions">Instructions to add</param>
        /// <param name="instructionText">Text to display</param>
        public void AddItem(IEnumerable<MInstruction> instructions, string instructionText)
        {
            _mInstructions.AddRange(instructions);
            
            //Instantiate game object to display text 
            var text = Instantiate(Resources.Load("UI/ScrollViewInstruction"), scrollViewContent.transform) as GameObject;
            if (!(text is null)) text.GetComponent<TextMeshProUGUI>().text = instructionText;
        }

        /// <summary>
        ///     Removes last item in queue and update view
        /// </summary>
        public void RemoveLastItem()
        {
            if (_mInstructions.Count <= 0) return;
            _mInstructions.RemoveAt(_mInstructions.Count - 1);
            Destroy(scrollViewContent.transform.GetChild(_mInstructions.Count).gameObject);
        }

        /// <summary>
        ///     Reset queue and view
        /// </summary>
        public void Clear()
        {
            foreach (Transform child in scrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }
            _mInstructions.Clear();
        }

        /// <summary>
        ///     Get current instruction queue
        /// </summary>
        /// <returns>Instruction queue</returns>
        public List<MInstruction> GETQueue()
        {
            return _mInstructions;
        }
        
        
        
        
    }
}