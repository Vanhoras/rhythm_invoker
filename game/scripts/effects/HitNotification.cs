using Godot;

public partial class HitNotification : Node
{
    [Export]
    private Conductor conductor;
    
	[Export]
	private Label okLabel;

    [Export]
    private Label perfectLabel;

    [Export]
    private Label missedLabel;

	[Export]
	private float showHitNotificationInBeats;
	private float showHitNotificationTime;

	private bool showHitNotification;

	private float elapsedTimeSinceShow;

    public override void _Ready()
	{
		showHitNotificationTime = (float) (showHitNotificationInBeats * conductor.Song.SPB);

        okLabel.Hide();
        perfectLabel.Hide();
        missedLabel.Hide();
    }

	public override void _Process(double delta)
	{
		if (!showHitNotification) return;

		elapsedTimeSinceShow += (float) delta;

		if (elapsedTimeSinceShow >= showHitNotificationTime)
		{
			showHitNotification = false;
            okLabel.Hide();
            perfectLabel.Hide();
            missedLabel.Hide();
        }
    }

	public void ShowHitNotification(HitType hitType)
	{
		showHitNotification = true;
		elapsedTimeSinceShow = 0;

		switch (hitType)
		{
			case HitType.MISSED:
				okLabel.Hide();
				perfectLabel.Hide();
				missedLabel.Show();
				break;
            case HitType.OK:
                okLabel.Show();
                perfectLabel.Hide();
                missedLabel.Hide();
                break;
            case HitType.PERFECT:
                okLabel.Hide();
                perfectLabel.Show();
                missedLabel.Hide();
                break;
        }
    }
}
