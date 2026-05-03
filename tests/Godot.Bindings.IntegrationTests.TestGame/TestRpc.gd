extends "res://TestBase.gd"


func _ready():
	var instance: TestRpc = $Instance

	# RPCs.
	assert_equal(instance.return_last_rpc_arg(), 0)
	instance.test_rpc(42)
	assert_equal(instance.return_last_rpc_arg(), 42)
	instance.test_send_rpc(100)
	assert_equal(instance.return_last_rpc_arg(), 100)

	exit_with_status()
