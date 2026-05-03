extends "res://TestBase.gd"


func _ready():
	# Test a class with a unicode name.
	var przykład = TestGodotObjectUnicodeNamePrzykład.new()
	assert_equal(przykład.get_the_word(), "słowo to przykład")

	exit_with_status()
