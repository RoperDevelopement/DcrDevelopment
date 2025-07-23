/*
 * User: Sam Brinly
 * Date: 2/4/2013
 */
using System;
using System.Reflection;
using System.Windows.Forms;

///http://www.codeproject.com/Tips/447319/Resolve-DesignMode-for-a-user-control

namespace EdocsUSA.Utilities.Extensions
{
	public static class ControlExtensions
	{
		public static bool IsInDesignMode(this Control control)
		{ return ResolveDesignMode(control); }
		
		private static bool ResolveDesignMode(Control control)
		{
			PropertyInfo designModeProperty;
			bool designMode;
			
			//Get the protected property
			designModeProperty = control.GetType().GetProperty("DesignMode", BindingFlags.Instance | BindingFlags.NonPublic);
			
			//Get the control's design mode property
			designMode = (bool)designModeProperty.GetValue(control, null);
			
			//Test the parent if it exists
			if (control.Parent != null) 
			{ designMode = designMode | ResolveDesignMode(control.Parent); }
			
			return designMode;
		}
	}
}
