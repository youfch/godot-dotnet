extends "res://TestBase.gd"


func _ready():
	# Array.
	assert_equal(TestCollections.test_array(), [1, 2])
	assert_equal(TestCollections.test_tarray(), [Vector2(1, 2), Vector2(2, 3)])

	var array: Array[int] = [1, 2, 3]
	assert_equal(TestCollections.test_tarray_arg(array), 6)

	# Dictionary.
	assert_equal(TestCollections.test_dictionary(), {"hello": "world", "foo": "bar"})
	assert_equal(TestCollections.test_tdictionary(), {Vector2(1, 2): Vector2i(2, 3)})

	var dictionary: Dictionary[String, int] = {"1": 1, "2": 2, "3": 3}
	assert_equal(TestCollections.test_tdictionary_arg(dictionary), 6)

	# PackedArray.
	assert_equal(TestCollections.test_vector_ops(), 105)

	exit_with_status()
