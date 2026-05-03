extends "res://TestBase.gd"


func _ready():
	# Constants.
	assert_equal(TestConstants.FIRST, 0)
	assert_equal(TestConstants.ANSWER_TO_EVERYTHING, 42)
	assert_equal(TestConstants.CONSTANT_WITHOUT_ENUM, 314)

	# BitFields.
	assert_equal(TestConstants.FLAG_ONE, 1)
	assert_equal(TestConstants.FLAG_TWO, 2)
	assert_equal(TestConstants.test_bitfield(0), 0)
	assert_equal(TestConstants.test_bitfield(TestConstants.FLAG_ONE | TestConstants.FLAG_TWO), 3)

	exit_with_status()
