using System.Collections.Generic;
using MMIStandard;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class QueueController : MonoBehaviour
    {
        public GameObject scrollViewContent;
        
        private readonly List<MInstruction> _mInstructions = new List<MInstruction>();

        public void AddItem(MInstruction instruction, string instructionText)
        {
            _mInstructions.Add(instruction);
            var text = Instantiate(Resources.Load("UI/ScrollViewInstruction"), scrollViewContent.transform) as GameObject;
            if (!(text is null)) text.GetComponent<TextMeshProUGUI>().text = instructionText;
        }

        public void AddItem(List<MInstruction> instructions, string instructionText)
        {
            _mInstructions.AddRange(instructions);
            var text = Instantiate(Resources.Load("UI/ScrollViewInstruction"), scrollViewContent.transform) as GameObject;
            if (!(text is null)) text.GetComponent<TextMeshProUGUI>().text = instructionText;
        }

        public void RemoveLastItem()
        {
            _mInstructions.RemoveAt(_mInstructions.Count - 1);
            Destroy(scrollViewContent.transform.GetChild(_mInstructions.Count).gameObject);
        }

        public void Clear()
        {
            foreach (Transform child in scrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }
            _mInstructions.Clear();
        }

        public List<MInstruction> GETQueue()
        {
            return _mInstructions;
        }
        
        
        
        
    }
}