using System;
using System.IO;
using BTCore.Runtime;
using BTCore.Runtime.Blackboards;
using BTCore.Runtime.Unity;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BTCore.Editor
{
    public class BTEditorWindow : EditorWindow
    {
        public static BTEditorWindow Instance;
        public BTView BTView { get; private set; }
        
        private NodeInspectorView _nodeInspectorView;
        private ToolbarMenu _toolbarMenu;
        private BlackboardView _blackboardView;

        private Button _undoButton;
        private Button _redoButton;

        private BTUndoRedo _undoRedo;
        
        public Blackboard Blackboard => _blackboardView.ExportData();
        
        [MenuItem("Tools/BehaviorTree/BTEditorWindow")]
        public static void OpenWindow()
        {
            var wnd = GetWindow<BTEditorWindow>();
            wnd.titleContent = new GUIContent("BTEditorWindow");
            wnd.minSize = new Vector2(800, 600);
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line) {
            var assetPath = AssetDatabase.GetAssetPath(instanceID);
            if (!assetPath.StartsWith(BTEditorDef.DataDir) || !assetPath.EndsWith(BTEditorDef.DataExt)) {
                return false;
            }

            var btData = (BTData) null;
            try {
                var json = File.ReadAllText(assetPath);
                btData = JsonConvert.DeserializeObject<BTData>(json, BTDef.SerializerSettingsAuto);
            }
            catch (Exception e) {
                Debug.LogError($"打开BT编辑器失败，请检查对应资源文件! 路径: {assetPath} ex: {e}");
                return false;
            }

            var wnd = GetWindow<BTEditorWindow>();
            wnd.titleContent = new GUIContent("BTEditorWindow");
            wnd.minSize = new Vector2(800, 600);
            wnd.SelectNewTree(btData);
            
            return true;
        }
        
        public void CreateGUI() {
            Instance = this;

            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;
        
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(BTEditorDef.BTEditorWindowUxmlPath);
            visualTree.CloneTree(root);

            BTView = root.Q<BTView>();
            _nodeInspectorView = root.Q<NodeInspectorView>();
            _toolbarMenu = root.Q<ToolbarMenu>();
            _blackboardView = root.Q<BlackboardView>();
            _undoButton = root.Q<Button>("undo-button");
            _redoButton = root.Q<Button>("redo-button");

            _undoButton.clicked -= OnUndo;
            _undoButton.clicked += OnUndo;
            _undoButton.SetEnabled(false);
            
            _redoButton.clicked -= OnRedo;
            _redoButton.clicked += OnRedo;
            _redoButton.SetEnabled(false);
            
            _toolbarMenu.RegisterCallback<MouseEnterEvent>(OnEnterToolbarMenu);
            
            _blackboardView.OnKeyListChanged = OnBlackboardKeyChanged;
            BTView.OnNodeSelected = OnNodeSelected;
            
            _undoRedo = new BTUndoRedo();
            SelectNewTree(new BTData());

            if (Application.isPlaying) {
                OnSelectionChange();
            }
        }

        private void OnUndo() {
            _undoRedo.Undo();
            UpdateUndoRedoButtonState();
        }

        private void OnRedo() {
            _undoRedo.Redo();
            UpdateUndoRedoButtonState();
        }

        public void AddCommand(ICommand command) {
            _undoRedo.AddCommand(command);
            UpdateUndoRedoButtonState();
        }

        private void UpdateUndoRedoButtonState() {
            _undoButton.SetEnabled(_undoRedo.CanUndo);
            _redoButton.SetEnabled(_undoRedo.CanRedo);
        }
        
        private void OnBlackboardKeyChanged() {
            _nodeInspectorView.UpdateNodeBindValues();
        }

        private void OnNodeSelected(BTNodeView nodeView) {
            _nodeInspectorView.UpdateSelection(nodeView);
        }

        public void ClearNodeSelectedInspector() {
            _nodeInspectorView.ClearSelection();
        }

        private void OnEnterToolbarMenu(MouseEnterEvent evt) {
            _toolbarMenu.menu.MenuItems().Clear();
            foreach (var filePath in Directory.GetFiles(BTEditorDef.DataDir, "*.json")) {
                var fileName = Path.GetFileName(filePath);
                _toolbarMenu.menu.AppendAction($"{fileName}", _ => {
                    var btData = (BTData) null;
                    try {
                        var json = File.ReadAllText(filePath);
                        btData = JsonConvert.DeserializeObject<BTData>(json, BTDef.SerializerSettingsAuto);
                    }
                    catch (Exception ex) {
                        Debug.LogError($"反序列BT数据失败，path: {filePath} ex: {ex}");
                        return;
                    }
                    
                    SelectNewTree(btData);
                });
            }
            
            _toolbarMenu.menu.AppendSeparator();
            
            // 保存当前行为树配置数据
            _toolbarMenu.menu.AppendAction("Save", _ => {
                var treeNodeData = BTView.ExportData();
                
                // 入口子节点为空
                if (string.IsNullOrEmpty(treeNodeData.EntryNode.ChildGuid)) {
                    ShowNotification("BT入口不能为空");
                    return;
                }
                
                var path = EditorUtility.SaveFilePanel("另存为", BTEditorDef.DataDir, BTEditorDef.DefaultFileName,
                    BTEditorDef.DataExt);
                if (string.IsNullOrEmpty(path)) {
                    return;
                }

                try {
                    var btData = new BTData {
                        TreeNodeData = BTView.ExportData(),
                        Blackboard = Blackboard
                    };
                    var json = JsonConvert.SerializeObject(btData, BTDef.SerializerSettingsAll);
                    File.WriteAllText(path, json);
                    AssetDatabase.Refresh();
                }
                catch (Exception e) {
                    ShowNotification("BT数据保存失败");
                    Debug.LogError($"BT树数据序列化失败，ex: {e}");
                    return;
                }
                
                ShowNotification("BT数据保存成功");
                Debug.Log($"BT数据保存成功，路径：{path}");
            });
        }

        private void OnInspectorUpdate() {
            if (Application.isPlaying) {
                BTView.UpdateNodesStyle();
            }
        }

        private void OnEnable() {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange stateChange) {
            if (stateChange == PlayModeStateChange.ExitingPlayMode) {
                SelectNewTree(new BTData());
            }
        }

        private void OnSelectionChange() {
            if (BTView == null) {
                return;
            }

            var btData = (BTData) null;
            
            // 1. 优先判断是否选中运行时的BT
            if (Selection.activeGameObject != null) {
                var btRunner = Selection.activeGameObject.GetComponent<BehaviorTree>();
                if (btRunner != null && btRunner.BTData != null) {
                    btData = btRunner.BTData;
                }
            }
            
            // 2. 再判断选中的资源可以反序列化为BT数据
            if (btData == null && Selection.activeObject != null) {
                var path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (!path.StartsWith(BTEditorDef.DataDir) || !path.EndsWith(BTEditorDef.DataExt)) {
                    SelectNewTree(new BTData());
                    return;
                }
            
                try {
                    var json = File.ReadAllText(path);
                    btData = JsonConvert.DeserializeObject<BTData>(json, BTDef.SerializerSettingsAuto);
                }
                catch (Exception e) {
                    ShowNotification("导入BT数据失败");
                    Debug.LogError($"导入BT数据失败，path: {path} ex: {e}");
                }
            }
            
            SelectNewTree(btData ?? new BTData());
        }

        public void ShowNotification(string message, double fadeoutWait = 4.0) {
            ShowNotification(new GUIContent(message), fadeoutWait);
        }
        
        public void SelectNewTree(BTData btData) {
            if (BTView == null) {
                return;
            }
            
            // 每次导入新的BT数据时，需要清空undo、redo保存数据
            if (_undoRedo != null) {
                _undoRedo.Clear();
                UpdateUndoRedoButtonState();
            }

            BTView.ImportData(btData.TreeNodeData);
            _blackboardView.ImportData(btData.Blackboard);
        }
    }
}