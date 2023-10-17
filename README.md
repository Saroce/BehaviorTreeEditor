# BehaviorTreeEditor
## 目录结构
- BTCore
    - Data：BT数据导出目录，主要为json格式。
    - Editor：编辑器部分，引用到Odin插件做编辑处理。
    - Runtime：注意Runtime部分需要支持脱离Unity环境运行，因此不能引用到任何Unity相关API和特性，只用到了一个第三方json序列化库**Newtonsoft.Json**方便序列化处理数据，你也可以替换为其他序列化保存方式。
    - Runtime.Unity：需要引用Unity部分的代码请放入这个目录，目前暂未实现主体逻辑，待扩展。
## 使用说明
基于UIToolkit实现行为树数据编辑，支持赋值粘贴，撤销回退，运行时预览等操作。当需要支持BT.Runtime部分脱离Unity环境运行，用于服务端或者帧同步框架上时，可以直接拷贝源码，或者导出BTRuntime.dll到外部工程，引用上Newtonsoft.Json即可。节点的Inspector面板和黑板面板都是用Odin插件实现编辑效果，支持节点变量绑定黑板数据，实现逻辑可以看BTCore.Editor部分代码。
## 效果图
![编辑效果图](Images/1.png)
![运行时预览效果图](Images/2.png)
## 已知问题
- 目前BTData部分只保存了树的入口节点，因此在编辑期间创建的节点没有连线到BT树的主体部分的话，数据不能够被保存！
## 参考
- https://github.com/thekiwicoder0/UnityBehaviourTreeEditor
