using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Prisms.Assignment
{
    public class UIPanel : MonoBehaviour
    {
        protected bool _isActive = false;
        public bool IsActive { get { return _isActive; } }
        public CanvasGroup canvasGroup;
        public UnityEvent OpenEvent;
        public UnityEvent CloseEvent;



        public virtual void SetPanelActivity(bool isActive, bool force = false) 
        {
            if (_isActive == isActive && !force) return;

            if (isActive) 
                OpenPanel();
            else 
                ClosePanel();
        }
        protected virtual void OpenPanel() 
        {
            _isActive = true;
            if (canvasGroup!= null)
            {
                canvasGroup.interactable = true;
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }
            OpenEvent?.Invoke();
        }
        protected virtual void ClosePanel() 
        {
            _isActive = false;
            if (canvasGroup!= null)
            {
                canvasGroup.interactable = false;
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
            }
            CloseEvent?.Invoke();
        }
    }
}