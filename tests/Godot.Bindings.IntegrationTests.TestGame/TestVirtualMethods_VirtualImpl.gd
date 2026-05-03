extends TestVirtualMethods


var value = null


func _do_something_virtual(p_name, p_value):
	value = [p_name, p_value]
	return "Implemented"
