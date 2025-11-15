//------------------------------------------------------------
//        File:  BTEditorDef.cs
//       Brief:  BTEditorDef
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-29
//============================================================

using System.Reflection;
using Newtonsoft.Json;

namespace BTCore.Editor
{
    public static class BTEditorDef
    {
        // BT窗口结构文件路径
        public static readonly string BTEditorWindowUxmlPath = "Assets/BTCore/Editor/UIBuilder/BTEditorWindow.uxml";
        // BT节点结构文件路径
        public static readonly string BTNodeViewUxmlPath = "Assets/BTCore/Editor/UIBuilder/BTNodeView.uxml";
        // 树形窗口视图样式路径
        public static readonly string BTViewStylePath = "Assets/BTCore/Editor/UIBuilder/BTViewStyle.uss";
        // 存放序列化后的BT数据目录
        public static readonly string DataDir = "Assets/BTCore/Data";
        
        public static BindingFlags BindValueFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    }
}
