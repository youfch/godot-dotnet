extends Node


var test_failures := 0


func __get_stack_frame():
	var me = get_script()
	for s in get_stack():
		if s.source == me.resource_path:
			return s
	return null


func __assert_fail(message: String) -> void:
	test_failures += 1
	var s = __get_stack_frame()
	if s != null:
		print("FAILURE: In function %s() at %s:%s" % [s.function, s.source, s.line])
	else:
		print("FAILURE (run with --debug to get more information!)")
	print(" └─ %s" % [message])
	print("GODOT_TEST_RESULT::FAIL: %s" % [message])


func assert_equal(actual, expected):
	if actual != expected:
		__assert_fail("Expected '%s' but got '%s'" % [expected, actual])


func assert_not_equal(actual, expected):
	if actual == expected:
		__assert_fail("Expected '%s' NOT to equal '%s'" % [expected, actual])


func assert_true(v):
	assert_equal(v, true)


func assert_false(v):
	assert_equal(v, false)


func exit_with_status() -> void:
	var success: bool = (test_failures == 0)

	if success:
		print("GODOT_TEST_RESULT::PASS: All asserts passed!")

	get_tree().quit(0 if success else 1)
