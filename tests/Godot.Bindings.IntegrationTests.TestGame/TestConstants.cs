using System;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestConstants : GodotObject
{
    [BindEnum]
    public enum Constants
    {
        [BindConstant(Name = "FIRST")]
        First,
        [BindConstant(Name = "ANSWER_TO_EVERYTHING")]
        AnswerToEverything = 42,
    }

    [BindEnum, Flags]
    public enum FlagsConstants
    {
        [BindConstant(Name = "FLAG_ONE")]
        FlagOne = 1,
        [BindConstant(Name = "FLAG_TWO")]
        FlagTwo = 2,
    }

    [BindConstant(Name = "CONSTANT_WITHOUT_ENUM")]
    public const int ConstantWithoutEnum = 314;

    [BindMethod(Name = "test_bitfield")]
    public static FlagsConstants TestBitfield(FlagsConstants flags)
    {
        return flags;
    }
}
