extends "res://TestBase.gd"


func _ready():
	# Test that we can access an engine singleton.
	assert_equal(TestSingletons.test_use_engine_singleton(), OS.get_name())

	exit_with_status()
