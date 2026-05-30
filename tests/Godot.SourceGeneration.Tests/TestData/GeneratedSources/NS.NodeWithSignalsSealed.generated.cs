#nullable enable

namespace NS;

partial class @NodeWithSignalsSealed
{
    public new partial class MethodName : global::Godot.Node.MethodName
    {
    }
    public new partial class ConstantName : global::Godot.Node.ConstantName
    {
    }
    public new partial class PropertyName : global::Godot.Node.PropertyName
    {
    }
    public new partial class SignalName : global::Godot.Node.SignalName
    {
        public static global::Godot.StringName @MySignal { get; } = global::Godot.StringName.CreateStaticFromAscii("MySignal"u8);
        public static global::Godot.StringName @MyNamedSignal { get; } = global::Godot.StringName.CreateStaticFromAscii("my_named_signal"u8);
        public static global::Godot.StringName @MySignalWithParameters { get; } = global::Godot.StringName.CreateStaticFromAscii("MySignalWithParameters"u8);
        public static global::Godot.StringName @MySignalWithNamedParameters { get; } = global::Godot.StringName.CreateStaticFromAscii("MySignalWithNamedParameters"u8);
        public static global::Godot.StringName @MySignalWithOptionalParameters { get; } = global::Godot.StringName.CreateStaticFromAscii("MySignalWithOptionalParameters"u8);
    }
    public event MySignalEventHandler @MySignal
    {
        add => Connect(SignalName.@MySignal, global::Godot.Callable.From(value.Invoke));
        remove => Disconnect(SignalName.@MySignal, global::Godot.Callable.From(value.Invoke));
    }
    private void EmitSignalMySignal()
    {
        EmitSignal(SignalName.@MySignal, []);
    }
    public event MyNamedSignalEventHandler @MyNamedSignal
    {
        add => Connect(SignalName.@MyNamedSignal, global::Godot.Callable.From(value.Invoke));
        remove => Disconnect(SignalName.@MyNamedSignal, global::Godot.Callable.From(value.Invoke));
    }
    private void EmitSignalMyNamedSignal()
    {
        EmitSignal(SignalName.@MyNamedSignal, []);
    }
    public event MySignalWithParametersEventHandler @MySignalWithParameters
    {
        add => Connect(SignalName.@MySignalWithParameters, global::Godot.Callable.From<int, float, string, global::NS.MyEnum>(value.Invoke));
        remove => Disconnect(SignalName.@MySignalWithParameters, global::Godot.Callable.From<int, float, string, global::NS.MyEnum>(value.Invoke));
    }
    private void EmitSignalMySignalWithParameters(int @a, float @b, string @c, global::NS.MyEnum @d)
    {
        EmitSignal(SignalName.@MySignalWithParameters, [@a, @b, @c, (long)@d]);
    }
    public event MySignalWithNamedParametersEventHandler @MySignalWithNamedParameters
    {
        add => Connect(SignalName.@MySignalWithNamedParameters, global::Godot.Callable.From<int, string>(value.Invoke));
        remove => Disconnect(SignalName.@MySignalWithNamedParameters, global::Godot.Callable.From<int, string>(value.Invoke));
    }
    private void EmitSignalMySignalWithNamedParameters(int @myNumber, string @myString)
    {
        EmitSignal(SignalName.@MySignalWithNamedParameters, [@myNumber, @myString]);
    }
    public event MySignalWithOptionalParametersEventHandler @MySignalWithOptionalParameters
    {
        add => Connect(SignalName.@MySignalWithOptionalParameters, global::Godot.Callable.From<int, int>(value.Invoke));
        remove => Disconnect(SignalName.@MySignalWithOptionalParameters, global::Godot.Callable.From<int, int>(value.Invoke));
    }
    private void EmitSignalMySignalWithOptionalParameters(int @requiredParameter, int @optionalParameter = 42)
    {
        EmitSignal(SignalName.@MySignalWithOptionalParameters, [@requiredParameter, @optionalParameter]);
    }
#pragma warning disable CS0108 // Method might already be defined higher in the hierarchy, that's not an issue.
    internal static void BindMembers(global::Godot.Bridge.ClassRegistrationContext context)
#pragma warning restore CS0108 // Method might already be defined higher in the hierarchy, that's not an issue.
    {
        context.BindConstructor(() => new global::NS.NodeWithSignalsSealed());
        context.BindSignal(new global::Godot.Bridge.SignalDefinition(SignalName.@MySignal));
        context.BindSignal(new global::Godot.Bridge.SignalDefinition(SignalName.@MyNamedSignal));
        context.BindSignal(new global::Godot.Bridge.SignalDefinition(SignalName.@MySignalWithParameters)
        {
            Parameters =
            {
                new global::Godot.Bridge.ParameterDefinition(global::Godot.StringName.CreateStaticFromAscii("a"u8), global::Godot.VariantType.Int, global::Godot.Bridge.VariantTypeMetadata.Int32)
                {
                    Usage = global::Godot.PropertyUsageFlags.Default,
                },
                new global::Godot.Bridge.ParameterDefinition(global::Godot.StringName.CreateStaticFromAscii("b"u8), global::Godot.VariantType.Float, global::Godot.Bridge.VariantTypeMetadata.Single)
                {
                    Usage = global::Godot.PropertyUsageFlags.Default,
                },
                new global::Godot.Bridge.ParameterDefinition(global::Godot.StringName.CreateStaticFromAscii("c"u8), global::Godot.VariantType.String)
                {
                    Usage = global::Godot.PropertyUsageFlags.Default,
                },
                new global::Godot.Bridge.ParameterDefinition(global::Godot.StringName.CreateStaticFromAscii("d"u8), global::Godot.VariantType.Int)
                {
                    Hint = global::Godot.PropertyHint.Enum,
                    HintString = "A,B,C",
                    Usage = global::Godot.PropertyUsageFlags.Default,
                },
            },
        });
        context.BindSignal(new global::Godot.Bridge.SignalDefinition(SignalName.@MySignalWithNamedParameters)
        {
            Parameters =
            {
                new global::Godot.Bridge.ParameterDefinition(global::Godot.StringName.CreateStaticFromAscii("my_number"u8), global::Godot.VariantType.Int, global::Godot.Bridge.VariantTypeMetadata.Int32)
                {
                    Usage = global::Godot.PropertyUsageFlags.Default,
                },
                new global::Godot.Bridge.ParameterDefinition(global::Godot.StringName.CreateStaticFromAscii("my_string"u8), global::Godot.VariantType.String)
                {
                    Usage = global::Godot.PropertyUsageFlags.Default,
                },
            },
        });
        context.BindSignal(new global::Godot.Bridge.SignalDefinition(SignalName.@MySignalWithOptionalParameters)
        {
            Parameters =
            {
                new global::Godot.Bridge.ParameterDefinition(global::Godot.StringName.CreateStaticFromAscii("requiredParameter"u8), global::Godot.VariantType.Int, global::Godot.Bridge.VariantTypeMetadata.Int32)
                {
                    Usage = global::Godot.PropertyUsageFlags.Default,
                },
                new global::Godot.Bridge.ParameterDefinition(global::Godot.StringName.CreateStaticFromAscii("optionalParameter"u8), global::Godot.VariantType.Int, global::Godot.Bridge.VariantTypeMetadata.Int32, 42)
                {
                    Usage = global::Godot.PropertyUsageFlags.Default,
                },
            },
        });
    }
}
