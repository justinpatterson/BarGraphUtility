using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prisms.Assignment
{
    public class SubPanelHelper : MonoBehaviour
    {
        //NOTE - I could have included a ref to GraphPanel / GraphController at this level since both subhelpers use it, but I feel that goes against the spirit of the abstraction of a generic subpanelhelper
        public virtual void InitializeHelper() { }
    }
}