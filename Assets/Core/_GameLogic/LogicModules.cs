//逻辑控制器的初始化

public class LogicModules :Singleton<LogicModules> {

    public override void Initialize()
    {
        StartController.Instance.Initialize();
    }

    public override void UnInitialize()
    {
        StartController.Instance.UnInitialize();
    }



}
