extends "res://TestBase.gd"


func _ready():
	# Call static methods.
	assert_equal(TestStaticMethods.test_static(9, 100), 109)
	TestStaticMethods.test_static_void()
	assert_equal(TestStaticMethods.get_void_value(), ["static_func_void", 2])

	exit_with_status()
