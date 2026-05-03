extends "res://TestBase.gd"


func _ready():
	var instance: TestGodotObjectConstruction = $Instance

	# Timing of set instance binding.
	assert_equal(instance.is_object_bindings_set_by_parent_constructor(), true)

	# Check NOTIFICATION_POST_INITIALIZED, both when created from GDScript and C#.
	var example_ref = TestGodotObjectConstruction_ExampleRef.new()
	assert_equal(example_ref.was_post_initialized(), true)
	assert_equal(TestGodotObjectConstruction.test_post_initialize(), true)

	exit_with_status()
