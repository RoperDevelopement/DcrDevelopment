using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
    public   class FocusTrackerService
    {
        private readonly List<WeakReference<VisualElement>> _trackedElements = new();
        public VisualElement? CurrentFocused { get; private set; }

        public void Register(VisualElement element)
        {
            _trackedElements.Add(new WeakReference<VisualElement>(element));

            element.Focused += OnFocused;
            element.Unfocused += OnUnfocused;
        }

        public void Unregister(VisualElement element)
        {
            element.Focused -= OnFocused;
            element.Unfocused -= OnUnfocused;

            _trackedElements.RemoveAll(wr =>
            {
                return wr.TryGetTarget(out var target) && target == element;
            });

            if (CurrentFocused == element)
                CurrentFocused = null;
        }

        private void OnFocused(object? sender, FocusEventArgs e)
        {
            if (sender is VisualElement ve)
                CurrentFocused = ve;
        }

        private void OnUnfocused(object? sender, FocusEventArgs e)
        {
            if (sender == CurrentFocused)
                CurrentFocused = null;
        }

        public void UnregisterAll()
        {
            foreach (var wr in _trackedElements)
            {
                if (wr.TryGetTarget(out var element))
                    Unregister(element);
            }

            _trackedElements.Clear();
        }
    }

}
