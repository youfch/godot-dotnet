extends "res://TestBase.gd"


var custom_signal_emitted = null


func _on_custom_signal(signal_name: String, value: int) -> void:
	custom_signal_emitted = [signal_name, value]


func _ready():
	var instance: TestSignals = $Instance

	# Signal.
	instance.emit_custom_signal("Button", 42)
	assert_equal(custom_signal_emitted, ["Button", 42])

	# Call simple method.
	instance.simple_func()
	assert_equal(custom_signal_emitted, ["simple_func", 3])

	# Callable.
	instance.callable_bind()
	assert_equal(custom_signal_emitted, ["bound", 11])

	exit_with_status()
