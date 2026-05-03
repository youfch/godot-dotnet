using System;
using System.Collections.Generic;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass(Tool = true)]
public partial class TestProperty : Control
{
    public partial class PropertyName
    {
        public static readonly StringName PropertyFromList = "property_from_list";
    }

    private readonly Vector2[] _dprop = new Vector2[3];
    private Vector3 _propertyFromList;

    [PropertyGroup("Test group", "group_")]
    [PropertySubgroup("Test subgroup", "group_subgroup_")]
    [BindProperty(Name = "group_subgroup_custom_position")]
    public Vector2 GroupSubgroupCustomPosition { get; set; }

    protected internal override bool _Get(StringName property, out Variant value)
    {
        string propertyName = property.ToString();

        if (propertyName.StartsWith("dproperty_", StringComparison.Ordinal))
        {
            var indexSpan = propertyName.AsSpan("dproperty_".Length);
            if (int.TryParse(indexSpan, out int index))
            {
                value = _dprop[index];
                return true;
            }
        }

        if (property == PropertyName.PropertyFromList)
        {
            value = _propertyFromList;
            return true;
        }

        value = default;
        return false;
    }

    protected internal override bool _Set(StringName property, Variant value)
    {
        string propertyName = property.ToString();

        if (propertyName.StartsWith("dproperty_", StringComparison.Ordinal))
        {
            var indexSpan = propertyName.AsSpan("dproperty_".Length);
            if (int.TryParse(indexSpan, out int index))
            {
                _dprop[index] = value.AsVector2();
                return true;
            }
        }

        if (property == PropertyName.PropertyFromList)
        {
            _propertyFromList = value.AsVector3();
            return true;
        }

        return false;
    }

    protected internal override void _GetPropertyList(IList<PropertyInfo> properties)
    {
        properties.Add(new PropertyInfo(PropertyName.PropertyFromList, VariantType.Vector3));

        for (int i = 0; i < 3; i++)
        {
            properties.Add(new PropertyInfo(new StringName($"dproperty_{i}"), VariantType.Vector2));
        }
    }

    protected internal override void _ValidateProperty(PropertyInfo property)
    {
        // Test hiding the "mouse_filter" property from the editor.
        if (property.Name == Control.PropertyName.MouseFilter)
        {
            property.Usage = PropertyUsageFlags.NoEditor;
        }
    }
}
