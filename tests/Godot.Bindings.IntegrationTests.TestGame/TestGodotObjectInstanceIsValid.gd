extends "res://TestBase.gd"


func _ready():
	var instance: TestGodotObjectInstanceIsValid = $Instance

	# Test checking if objects are valid.
	var object_of_questionable_validity = Object.new()
	assert_equal(instance.test_object_is_valid(object_of_questionable_validity), true)
	object_of_questionable_validity.free()
	assert_equal(instance.test_object_is_valid(object_of_questionable_validity), false)

	exit_with_status()
