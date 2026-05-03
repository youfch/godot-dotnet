extends "res://TestBase.gd"


func _ready():
	# String marshalling with unicode characters.
	assert_equal(TestStrings.test_string_unicode_in("ABCĎE"), true)
	assert_equal(TestStrings.test_string_unicode_out(), "ABCĎE")

	exit_with_status()
