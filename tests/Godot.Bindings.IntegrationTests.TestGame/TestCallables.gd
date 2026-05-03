extends "res://TestBase.gd"


func _ready():
	var instance: TestCallables = $Instance

	# mp_callable() with void method.
	var mp_callable: Callable = instance.test_callable_mp()
	assert_equal(mp_callable.is_valid(), true)
	assert_equal(mp_callable.get_argument_count(), 3)
	mp_callable.call(instance, "void", 36)
	assert_equal(instance.get_void_value(), ["unbound_method1: TestCallables - void", 36])

	# Check that it works with is_connected().
	assert_equal(instance.renamed.is_connected(mp_callable), false)
	instance.renamed.connect(mp_callable)
	assert_equal(instance.renamed.is_connected(mp_callable), true)
	# Make sure a new instanceect is still treated as equivalent.
	assert_equal(instance.renamed.is_connected(instance.test_callable_mp()), true)
	assert_equal(mp_callable.hash(), instance.test_callable_mp().hash())
	instance.renamed.disconnect(mp_callable)
	assert_equal(instance.renamed.is_connected(mp_callable), false)

	# mp_callable() with return value.
	var mp_callable_ret: Callable = instance.test_callable_mp_ret()
	assert_equal(mp_callable_ret.get_argument_count(), 3)
	assert_equal(mp_callable_ret.call(instance, "test", 77), "unbound_method2: TestCallables - test - 77")

	# mp_callable_static() with void method.
	var mp_callable_static: Callable = instance.test_callable_mp_static()
	assert_equal(mp_callable_static.get_argument_count(), 3)
	mp_callable_static.call(instance, "static", 83)
	assert_equal(instance.get_void_value(), ["unbound_static_method1: TestCallables - static", 83])

	# Check that it works with is_connected().
	assert_equal(instance.renamed.is_connected(mp_callable_static), false)
	instance.renamed.connect(mp_callable_static)
	assert_equal(instance.renamed.is_connected(mp_callable_static), true)
	# Make sure a new instanceect is still treated as equivalent.
	assert_equal(instance.renamed.is_connected(instance.test_callable_mp_static()), true)
	assert_equal(mp_callable_static.hash(), instance.test_callable_mp_static().hash())
	instance.renamed.disconnect(mp_callable_static)
	assert_equal(instance.renamed.is_connected(mp_callable_static), false)

	# mp_callable_static() with return value.
	var mp_callable_static_ret: Callable = instance.test_callable_mp_static_ret()
	assert_equal(mp_callable_static_ret.get_argument_count(), 3)
	assert_equal(mp_callable_static_ret.call(instance, "static-ret", 84), "unbound_static_method2: TestCallables - static-ret - 84")

	# CallableCustom.
	var custom_callable: Callable = instance.test_custom_callable();
	assert_equal(custom_callable.is_custom(), true);
	assert_equal(custom_callable.is_valid(), true);
	assert_equal(custom_callable.call(), "Hi")
	assert_equal(custom_callable.hash(), 27);
	assert_equal(custom_callable.get_object(), null);
	assert_equal(custom_callable.get_method(), "");
	assert_equal(custom_callable.get_argument_count(), 2)
	assert_equal(str(custom_callable), "<MyCallableCustom>");

	exit_with_status()
