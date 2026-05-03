extends "res://TestBase.gd"


func _ready():
	var instance: TestMethods = $Instance

	# Pass custom reference.
	assert_equal(instance.custom_ref_func(null), -1)
	var ref1 = TestMethods_ExampleRef.new()
	ref1.id = 27
	assert_equal(instance.custom_ref_func(ref1), 27)

	# Pass core reference.
	assert_equal(instance.image_ref_func(null), "invalid")
	var image = Image.new()
	assert_equal(instance.image_ref_func(image), "valid")

	# Return values.
	assert_equal(instance.return_something("some string"), "some string42")
	assert_equal(instance.return_something_const(), get_viewport())
	var null_ref = instance.return_empty_ref()
	assert_equal(null_ref, null)
	var ret_ref = instance.return_extended_ref()
	assert_not_equal(ret_ref.get_instance_id(), 0)
	assert_equal(ret_ref.get_id(), 0)
	assert_equal(instance.get_v4(), Vector4(1.2, 3.4, 5.6, 7.8))
	assert_equal(instance.test_node_argument(instance), instance)
	var var_ref = TestMethods_ExampleRef.new()
	assert_not_equal(instance.extended_ref_checks(var_ref).get_instance_id(), var_ref.get_instance_id())

	# Test that ptrcalls from GDExtension to the engine are correctly encoding Object and RefCounted.
	var new_node = Node.new()
	instance.test_add_child(new_node)
	assert_equal(new_node.get_parent(), instance)
	var new_tileset = TileSet.new()
	var new_tilemap = TileMap.new()
	instance.test_set_tileset(new_tilemap, new_tileset)
	assert_equal(new_tilemap.tile_set, new_tileset)
	new_tilemap.queue_free()

	# Test object call.
	var test_obj = TestClass.new()
	assert_equal(instance.test_object_call(test_obj), "hello world")

	exit_with_status()


class TestClass:
	func test(p_msg: String) -> String:
		return p_msg + " world"
