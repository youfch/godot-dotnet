extends "res://TestBase.gd"


func _ready():
	var instance: TestVirtualMethods = $Instance

	# Virtual method.
	var event = InputEventKey.new()
	event.key_label = KEY_H
	event.unicode = 72
	get_viewport().push_input(event)
	assert_equal(instance.get_virtual_value(), ["_input: H", 72])

	# Test a virtual method defined in GDExtension and implemented in script.
	assert_equal(instance.test_virtual_implemented_in_script("Virtual", 939), "Implemented")
	assert_equal(instance.value, ["Virtual", 939])

	exit_with_status()
