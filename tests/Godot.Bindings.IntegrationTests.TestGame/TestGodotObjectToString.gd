extends "res://TestBase.gd"


func _ready():
	var instance_with: TestGodotObjectToString = $InstanceWith
	var instance_without: TestGodotObjectNoToString = $InstanceWithout

	# To string.
	assert_equal(instance_with.to_string(), '[ GDExtension::Example <--> Instance ID:%s ]' % instance_with.get_instance_id())
	assert_equal(instance_without.to_string(), '<TestGodotObjectNoToString#%s>' % instance_without.get_instance_id())

	exit_with_status()
