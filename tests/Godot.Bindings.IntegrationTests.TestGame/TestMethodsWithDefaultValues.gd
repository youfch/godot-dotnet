extends "res://TestBase.gd"


func _ready():
	var instance: TestMethodsWithDefaultValues = $Instance

	# Method calls with default values.
	assert_equal(instance.def_args(), 300)
	assert_equal(instance.def_args(50), 250)
	assert_equal(instance.def_args(50, 100), 150)

	exit_with_status()
