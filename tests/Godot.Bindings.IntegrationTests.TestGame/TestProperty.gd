extends "res://TestBase.gd"


func _ready():
	var instance: TestProperty = $Instance

	# Properties.
	assert_equal(instance.group_subgroup_custom_position, Vector2(0, 0))
	instance.group_subgroup_custom_position = Vector2(50, 50)
	assert_equal(instance.group_subgroup_custom_position, Vector2(50, 50))

	# Property list.
	instance.property_from_list = Vector3(100, 200, 300)
	assert_equal(instance.property_from_list, Vector3(100, 200, 300))
	var prop_list = instance.get_property_list()
	for prop_info in prop_list:
		if prop_info['name'] == 'mouse_filter':
			assert_equal(prop_info['usage'], PROPERTY_USAGE_NO_EDITOR)

	exit_with_status()
