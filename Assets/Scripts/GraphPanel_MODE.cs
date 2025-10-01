using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prisms.Assignment
{
    public class GraphPanel_MODE : GraphPanel
    {
        protected override GraphDataModel.GraphDataElement[] GetTransformedDataModel()
        {
            List<GraphDataModel.GraphDataElement> transformedData = new List<GraphDataModel.GraphDataElement>();
            Dictionary<int, int> dataModeMap = new Dictionary<int, int>();
            for(int i = 0; i < controller.graphDataModel.data.Length; i++) 
            {
                int key = controller.graphDataModel.data[i].value;
                int count = 0;
                if (dataModeMap.TryGetValue(key, out count)) 
                {
                    dataModeMap[key] = count+1;
                }
                else 
                {
                    dataModeMap.Add(key, 1);
                }
            }
            foreach(int key in dataModeMap.Keys) 
            {
                GraphDataModel.GraphDataElement elmt;
                elmt.index = key;
                elmt.value = dataModeMap[key];
                transformedData.Add(elmt);
            }
            return transformedData.ToArray();
        }
    }
}
