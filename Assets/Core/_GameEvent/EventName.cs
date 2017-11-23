//全局事件定义在EventName文件里，模块之内的事件在类里面创建一个LocalEvent枚举类型

public enum EventName {

    GameStart, //加载场景前
    GameLoop,

    ShowStartView,
    EnterGame,
}
