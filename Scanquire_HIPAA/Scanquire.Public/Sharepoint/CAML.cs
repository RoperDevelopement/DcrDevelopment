using System;

namespace Scanquire.Public.Sharepoint
{
	public static class CAML
	{
		public static string And(string p1, string p2)
		{ return string.Format("<And>{0}{1}</And>", p1, p2); }
		
		public static string Or(string p1, string p2)
		{ return string.Format("<Or>{0}{1}</Or>", p1, p2); }
		
		public static string Gt(string fieldName, string fieldType, string value)
		{ return string.Format("<Gt><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Gt>", SPHelper.UnspaceValue(fieldName), fieldType, value); }
		
		public static string Lt(string fieldName, string fieldType, string value)
		{ return string.Format("<Lt><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Lt>", SPHelper.UnspaceValue(fieldName), fieldType, value); }
		
		public static string Geq(string fieldName, string fieldType, string value)
		{ return string.Format("<Geq><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Geq>", SPHelper.UnspaceValue(fieldName), fieldType, value); }
		
		public static string Leq(string fieldName, string fieldType, string value)
		{ return string.Format("<Leq><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Leq>", SPHelper.UnspaceValue(fieldName), fieldType, value); }
		
		public static string Eq(string fieldName, string fieldType, string value)
		{ return string.Format("<Eq><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Eq>", SPHelper.UnspaceValue(fieldName), fieldType, value); }
		
		public static string Neq(string fieldName, string fieldType, string value)
		{ return string.Format("<Neq><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Neq>", SPHelper.UnspaceValue(fieldName), fieldType, value); }
		
		public static string IsNull(string fieldName)
		{ return string.Format("<IsNull><FieldRef Name='{0}'/></IsNull>", SPHelper.UnspaceValue(fieldName)); }
		
		public static string Contains(string fieldName, string fieldType, string value)
		{ return string.Format("<Contains><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Contains>", SPHelper.UnspaceValue(fieldName), fieldType, value); }
		
		public static string BeginsWith(string fieldName, string fieldType, string value)
		{ return string.Format("<BeginsWith><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></BeginsWith>", SPHelper.UnspaceValue(fieldName), fieldType, value); }
		
		public static string Query(string whereClause, string groupByClause, string orderByClause)
		{ return string.Format("<Query><Where>{0}</Where><GroupBy>{1}</GroupBy><OrderBy>{2}</OrderBy></Query>", whereClause, groupByClause, orderByClause); }
		
		
	}
}
