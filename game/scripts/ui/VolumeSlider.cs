using Godot;

public partial class VolumeSlider : HSlider
{
	[Export]
	public string BusName = "Master";

    private int busIndex;

	public override void _Ready()
	{
        ValueChanged += SetVolume;

        busIndex = AudioServer.GetBusIndex(BusName);
        float currentVolume = AudioServer.GetBusVolumeDb(busIndex);

        Value = Mathf.Pow(10, currentVolume / 20);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        ValueChanged -= SetVolume;
    }

    public void SetVolume(double sliderValue)
    {
        AudioServer.SetBusVolumeDb(busIndex, (float)Mathf.Log(sliderValue) * 20);
    }
}
