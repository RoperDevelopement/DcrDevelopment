/*
 * User: Sam Brinly
 * Date: 2/2/2013
 */
using System;

namespace EdocsUSA.Utilities
{
	public class SettingsValue<T>
	{
		public T DefaultValue { get; protected set; }
		
		public SettingsManager.SettingsScope SettingsScope { get; protected set; }
		
		public string SettingsFileName { get; protected set; }
		
		public string SettingsKey { get; protected set; }
		
		public bool ValueAssigned { get; protected set; }
		
		public bool ValueAssignedFromDefault { get; protected set; }
		
		private T _Value;
		public T Value
		{
			get
			{
				if (ValueAssigned == false)
				{
					bool found;
					_Value = SettingsManager.ReadKey(SettingsFileName, SettingsKey, DefaultValue, out found);
					if (found == false) ValueAssignedFromDefault = true;
					ValueAssigned = true;
				}
				return _Value;
			}
			set
			{
				_Value = value;
				ValueAssigned = true;
				SettingsManager.WriteKey(SettingsScope, SettingsFileName, SettingsKey, Value);
			}
		}
		
		public SettingsValue(SettingsManager.SettingsScope scope, string fileName, string key, T defaultValue)
		{
			SettingsScope = scope;
			SettingsFileName = fileName;
			SettingsKey = key;
			DefaultValue = defaultValue;
		}
	}
	/*
	public class Defaultable<T>
	{
		public readonly T DefaultValue;
		
		public bool ValueAssigned { get; protected set; }
		public bool ValueAssignedFromDefault { get; protected set; }
		
		private T _Value;
		public T Value
		{
			get
			{
				if (ValueAssigned == false) 
				{
					_Value = DefaultValue;
					ValueAssigned = true;
					ValueAssignedFromDefault = true;
				}
				return _Value;
			}
			set
			{
				_Value = value;
				ValueAssigned = true;
			}
		}
		
		public Defaultable(T defaultValue)
		{ DefaultValue = defaultValue; }
		
		public Defaultable(T defaultVaue, T value) : this(defaultValue)
		{ Value = value; }
	}	*/
}