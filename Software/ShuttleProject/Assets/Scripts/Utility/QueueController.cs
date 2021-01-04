using System;
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
        private readonly List<QueueElement> _queueList = new List<QueueElement>();

        /// <summary>
        ///     Add instructions to queue
        /// </summary>
        /// <param name="instruction">Instruction to add</param>
        /// <param name="instructionText">Text to display</param>
        public void AddItem(MInstruction instruction, string instructionText)
        {
            _queueList.Add(new QueueElement(instruction));

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
            _queueList.Add(new QueueElement(instructions));

            //Instantiate game object to display text 
            var text = Instantiate(Resources.Load("UI/ScrollViewInstruction"), scrollViewContent.transform) as GameObject;
            if (!(text is null)) text.GetComponent<TextMeshProUGUI>().text = instructionText;
        }

        /// <summary>
        ///     Removes last item in queue and update view
        /// </summary>
        public void RemoveLastItem()
        {
            if (_queueList.Count <= 0) return;
            Destroy(scrollViewContent.transform.GetChild(scrollViewContent.transform.childCount-1).gameObject);
            _queueList.RemoveAt(_queueList.Count - 1);

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
            _queueList.Clear();
        }

        /// <summary>
        ///     Get current instruction queue
        /// </summary>
        /// <returns>Instruction queue</returns>
        public List<MInstruction> GETQueue()
        {
            List<MInstruction> list = new List<MInstruction>();
            foreach (var x in _queueList)
            {
                list.AddRange(x.GETList());                
            }

            return list;
        }

        private class QueueElement
        {
            private readonly IEnumerable<MInstruction> _instructions;

            public QueueElement(IEnumerable<MInstruction> list)
            {
                _instructions = list;
            }
            public QueueElement(MInstruction instruction)
            {
                _instructions = new List<MInstruction> {instruction};
            }

            public IEnumerable<MInstruction> GETList()
            {
                return _instructions;
            }
            
        }
        
        
        
        
    }
}