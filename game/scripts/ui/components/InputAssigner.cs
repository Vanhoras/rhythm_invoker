using Godot;

public partial class InputAssigner : Button
{

	[Export]
	public string action = "button";

	[Export]
	public Label keyLabel;

	private bool isRemapping;

	public override void _Ready()
	{
		DisplayKey();
		Pressed += OnPressed;
    }

	private void DisplayKey()
	{
        keyLabel.Text = InputMap.ActionGetEvents(action)[0].AsText().TrimSuffix(" (Physical)");
	}

	public void OnPressed()
	{
        keyLabel.Text = "PRESS KEY TO BIND";
		isRemapping = true;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!isRemapping) return;
        if (!(
			@event is InputEventKey || 
			(@event is InputEventMouseButton && @event.IsPressed()))) return;

		InputMap.ActionEraseEvents(action);
		InputMap.ActionAddEvent(action, @event);
		DisplayKey();
		isRemapping = false;

		AcceptEvent();
    }
}
