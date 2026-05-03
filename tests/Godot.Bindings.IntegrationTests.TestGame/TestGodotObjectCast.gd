extends "res://TestBase.gd"


func _ready():
	var instance: TestGodotObjectCast = $Instance

	var control = Control.new()
	var sprite = Sprite2D.new()
	var example_ref = TestGodotObjectCast_ExampleRef.new()

	assert_equal(TestGodotObjectCast.test_object_is_node(control), true)
	assert_equal(TestGodotObjectCast.test_object_is_control(control), true)
	assert_equal(TestGodotObjectCast.test_object_is_self(control), false)

	assert_equal(TestGodotObjectCast.test_object_is_node(instance), true)
	assert_equal(TestGodotObjectCast.test_object_is_control(instance), true)
	assert_equal(TestGodotObjectCast.test_object_is_self(instance), true)

	assert_equal(TestGodotObjectCast.test_object_is_node(sprite), true)
	assert_equal(TestGodotObjectCast.test_object_is_control(sprite), false)
	assert_equal(TestGodotObjectCast.test_object_is_self(sprite), false)

	assert_equal(TestGodotObjectCast.test_object_is_node(example_ref), false)
	assert_equal(TestGodotObjectCast.test_object_is_control(example_ref), false)
	assert_equal(TestGodotObjectCast.test_object_is_self(example_ref), false)

	control.queue_free()
	sprite.queue_free()

	# Test that passing null for objects works as expected too.
	var example_null: TestGodotObjectCast = null
	assert_equal(TestGodotObjectCast.test_object_is_node(example_null), false)

	exit_with_status()
