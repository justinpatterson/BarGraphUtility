using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Prisms.Assignment
{
    public class SPH_MEDIAN : SubPanelHelper
    {
        public GraphPanel graphPanel;
        public Slider meanSlider;
        public TextMeshProUGUI meanLabel;

        public override void InitializeHelper()
        {
            base.InitializeHelper();
            if (graphPanel == null)
                return;

            float median = graphPanel.controller.CalculateMedian();

            meanLabel.text = string.Format("Median: {0}", median.ToString("F2"));
            meanSlider.value = ((float)median / (float)(graphPanel.MaxVal * 1.1f)); //10% wiggle room, just like NodeBarView
        }
    }
}
