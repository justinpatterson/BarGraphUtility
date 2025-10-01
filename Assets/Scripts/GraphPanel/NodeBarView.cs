using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Prisms.Assignment
{
    public class NodeBarView : MonoBehaviour
    {
        public GraphDataModel.GraphDataElement dataElement;
        public TextMeshProUGUI indexLabel;
        public TextMeshProUGUI valueLabel;
        public Slider slider;

        public void RefreshView(int max = 100) 
        {
            slider.value = ((float)dataElement.value / (float) (max * 1.1f) ); //10% wiggle room
            indexLabel.text = dataElement.index.ToString(); 
            valueLabel.text = dataElement.value.ToString(); 
        }
    }
}