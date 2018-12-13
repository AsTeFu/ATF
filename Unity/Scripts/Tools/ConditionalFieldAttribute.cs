using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ConditionalFieldAttribute : PropertyAttribute
{
	public string PropertyToCheck;

	public object[] CompareValue;
    //public object CompareValue2;

    public ConditionalFieldAttribute(string propertyToCheck, params object[] compareValue) {
        PropertyToCheck = propertyToCheck;
        CompareValue = compareValue;
    }
}