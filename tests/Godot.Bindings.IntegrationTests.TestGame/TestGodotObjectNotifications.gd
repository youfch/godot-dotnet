extends "res://TestBase.gd"


func _ready():
	# Test that notifications happen on both parent and child classes.
	var child = $Child
	assert_equal(child.get_value1(), 11)
	assert_equal(child.get_value2(), 33)
	child.notification(NOTIFICATION_ENTER_TREE, true)
	assert_equal(child.get_value1(), 11)
	assert_equal(child.get_value2(), 22)

	exit_with_status()
