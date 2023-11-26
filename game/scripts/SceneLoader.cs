using Godot;

public partial class SceneLoader : Node
{
    [Export]
    private Node mainNode;

    [Export]
    private PackedScene mainMenuPackedScene;

    [Export]
    private PackedScene gamePackedScene;

    [Export]
    private Node mainMenu;

    [Export]
    private HighScoreManager highScoreManager;

    private Node game;

    public override void _Ready()
	{
	}

    public void LoadMainMenu()
    {
        Node instance = mainMenuPackedScene.Instantiate();
        mainNode.AddChild(instance);

        mainMenu = instance;
        ((MainMenu)mainMenu).Instantiate(this, true, highScoreManager);

        game.QueueFree();
    }

    public void LoadLevel()
    {
        Node instance = gamePackedScene.Instantiate();
        mainNode.AddChild(instance);

        game = instance;
        ((Level)game).Instantiate(this, highScoreManager);

        mainMenu.QueueFree();
    }
}
