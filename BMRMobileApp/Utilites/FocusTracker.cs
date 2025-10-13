using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
    public   class FocusTracker
    {
        public   VisualElement? CurrentFocused { get; private set; }

        public   void Register(VisualElement element)
        {
            element.Focused += (s, e) => CurrentFocused = (VisualElement?)s;
            element.Unfocused += (s, e) => {
                if (CurrentFocused == s)
                    CurrentFocused = null;
            };
        }
        public   void Unregister(VisualElement element, EventHandler<FocusEventArgs> OnFocused, EventHandler<FocusEventArgs> OnUnfocused)
        {
            element.Focused -= OnFocused;
            element.Unfocused -= OnUnfocused;

            if (CurrentFocused == element)
                CurrentFocused = null;
        }

         
    }

}
