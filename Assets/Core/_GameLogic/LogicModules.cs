//逻辑控制器的初始化

public class LogicModules :Singleton<LogicModules> {

    public override void Initialize()
    {
        StartController.Instance.Initialize();
        MainBackgroundController.Instance.Initialize();
        LevelController.Instance.Initialize();
        PlayerModel.Instance.Initialize();
        GameController.Instance.Initialize();
    }

    public override void UnInitialize()
    {
        StartController.Instance.UnInitialize();
        MainBackgroundController.Instance.UnInitialize();
        LevelController.Instance.UnInitialize();
        PlayerModel.Instance.UnInitialize();
        GameController.Instance.UnInitialize();
    }



}
