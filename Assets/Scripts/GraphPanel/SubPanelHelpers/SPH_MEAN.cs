using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Prisms.Assignment
{
    public class SPH_MEAN : SubPanelHelper
    {
        public GraphPanel graphPanel;

        public Slider meanSlider;
        public  TextMeshProUGUI meanLabel;
        
        public override void InitializeHelper()
        {
            base.InitializeHelper();
            if (graphPanel == null)
                return;

            float mean = graphPanel.controller.CalculateMean();

            meanLabel.text = string.Format("Mean: {0}", mean.ToString("F2"));
            meanSlider.value = ((float)mean / (float)(graphPanel.MaxVal * 1.1f)); //10% wiggle room, just like NodeBarView
        }
    }
}