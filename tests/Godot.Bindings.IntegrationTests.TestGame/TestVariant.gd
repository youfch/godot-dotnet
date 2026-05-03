extends "res://TestBase.gd"


func _ready():
	# Test conversions to and from Variant.
	assert_equal(TestVariant.test_variant_vector2i_conversion(Vector2i(1, 1)), Vector2i(1, 1))
	assert_equal(TestVariant.test_variant_vector2i_conversion(Vector2(1.0, 1.0)), Vector2i(1, 1))
	assert_equal(TestVariant.test_variant_int_conversion(10), 10)
	assert_equal(TestVariant.test_variant_int_conversion(10.0), 10)
	assert_equal(TestVariant.test_variant_float_conversion(10.0), 10.0)
	assert_equal(TestVariant.test_variant_float_conversion(10), 10.0)

	# Test variant iterator.
	assert_equal(TestVariant.test_variant_iterator([10, 20, 30]), [15, 25, 35])
	assert_equal(TestVariant.test_variant_iterator(null), "iter_init: not valid")

	exit_with_status()
