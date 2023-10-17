//------------------------------------------------------------
//        File:  BTEditor.cs
//       Brief:  BTEditor
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-02
//============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using BTCore.Runtime;
using BTCore.Runtime.Composites;
using BTCore.Runtime.Decorators;
using Core.Lite.Extensions;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace BTCore.Editor
{
    public class BTView : GraphView, IDataSerializableEditor<TreeNodeData>
    {
        public new class UxmlFactory : UxmlFactory<BTView, UxmlTraits> { }

        private TreeNodeData _treeNodeData;

        public Action<BTNodeView> OnNodeSelected;

        private readonly Vector2 _pasteNodeOffset = new Vector2(50f, 50f);
        private readonly Vector2 _entryPos = new Vector2(225f, 150f);
        private bool _isRemoveOnly = false;

        // private List<BTData> _undoStack = new List<BTData>();
        // private const int UNDO_MAX_COUNT = 15;

        public BTView() {
            Insert(0, new GridBackground());
            
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            RegisterCopyAndPasteEvent();
            
            // 添加BTView样式，主要是格子背景展示
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(BTEditorDef.BTViewStylePath);
            styleSheets.Add(styleSheet);
        }

        private void RegisterCopyAndPasteEvent() {
            serializeGraphElements = elements => {
                var copyPasteData = new CopyPasteData();
                copyPasteData.AddGraphElements(elements);
                return JsonConvert.SerializeObject(copyPasteData);
            };

            unserializeAndPaste = (_, data) => {
                var copyPasteData = JsonConvert.DeserializeObject<CopyPasteData>(data);
                if (copyPasteData == null) {
                    return;
                }
                
                ClearSelection();
                
                // 找到所有复制的节点
                var nodesToCopy = new List<BTNodeView>();
                copyPasteData.Guids.ForEach(guid => {
                    nodesToCopy.Add(FindNodeView(guid));
                });

                // 找到待复制节点之间的连线关系
                var edgesToCreate = new List<EdgeToCreate>();
                foreach (var nodeView in nodesToCopy) {
                    var nodeParent = nodeView.NodeParent;
                    if (nodeParent == null || !nodesToCopy.Contains(nodeParent)) {
                        continue;
                    }

                    edgesToCreate.Add(new EdgeToCreate() {
                        ParentGuid = nodeParent.Node.Guid,
                        ChildGuid = nodeView.Node.Guid
                    });
                }
                
                // 记录旧节点Guid -> 新节点Guid映射关系
                var guidsMapping = new Dictionary<string, string>();
                // 根据待复制节点列表，创建新的节点数据和节点视图
                foreach (var nodeView in nodesToCopy) {
                    var newPos = new Vector2(nodeView.Node.PosX, nodeView.Node.PosY) + _pasteNodeOffset;
                    var newNode = CreateNode(nodeView.Node.GetType(), newPos);
                    var newNodeView = CreateNodeView(newNode);
                    AddToSelection(newNodeView);
                    
                    guidsMapping.Add(nodeView.Node.Guid, newNode.Guid);
                }

                // 建立新节点之间的数据跟视图的连接关系
                foreach (var toCreate in edgesToCreate) {
                    var newParent = FindNodeView(guidsMapping[toCreate.ParentGuid]);
                    var newChild = FindNodeView(guidsMapping[toCreate.ChildGuid]);
                    if (newParent == null || newChild == null) {
                        continue;
                    }
                    
                    AddChild(newParent.Node, newChild.Node);
                    AddChildView(newParent, newChild);
                }
            };
        }
        
        public void ImportData(TreeNodeData treeNodeData) {
            _treeNodeData = treeNodeData;
            
            ClearGraphs();
            BTEditorWindow.Instance.ClearNodeSelectedInspector();
            
            // 默认没有会创建对应的入口节点
            _treeNodeData.EntryNode ??= CreateNode<EntryNode>(_entryPos) as EntryNode;

            var treeNodes = treeNodeData.GetNodes();
            // 根据保存的数据创建对应节点
            treeNodes.ForEach(node => CreateNodeView(node));
            // 根据节点数据关系创建连线
            treeNodes.ForEach(node => {
                var childrenGuids = node.GetChildrenGuids();
                childrenGuids.ForEach(guid => {
                    var parentView = FindNodeView(node.Guid);
                    var childView = FindNodeView(guid);
                    if (parentView != null && childView != null) {
                        ConnectNode(parentView, childView);
                    }
                    
                });
            });
        }

        private void ClearGraphs() {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
        }

        private BTNodeView FindNodeView(string guid) {
            return GetNodeByGuid(guid) as BTNodeView;
        }

        public TreeNodeData ExportData() {
            return _treeNodeData;
        }

        /// <summary>
        /// 右键打开，节点搜索框
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            NodeSearchWindow.Show(evt.mousePosition, null);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
            if (_isRemoveOnly) {
                return graphViewChange;
            }
            
            graphViewChange.elementsToRemove?.ForEach(ele => {
                // 连线被删除，更新BT数据部分之间的连接关系
                if (ele is Edge edge) {
                    if (edge.output.node is BTNodeView parentView && edge.input.node is BTNodeView childView) {
                        RemoveChild(parentView.Node, childView.Node);
                    }
                }
                // 节点被删除
                if (ele is BTNodeView nodeView) {
                    var foundNode = _treeNodeData.GetNodeByGuid(nodeView.Node.Guid);
                    if (foundNode != null) {
                        _treeNodeData.RemoveNode(foundNode);
                    }
                }
            });
            
            // 连线被创建，更新BT数据部分之间的连接关系
            graphViewChange.edgesToCreate?.ForEach(edge => {
                if (edge.output.node is BTNodeView parentView && edge.input.node is BTNodeView childView) {
                    AddChild(parentView.Node, childView.Node);
                }
            });

            // 节点被移动，对于复合节点，需要重新排序Children的顺序关系
            if (graphViewChange.movedElements != null) {
                nodes.ForEach(node => {
                    if (node is BTNodeView nodeView) {
                        nodeView.SortChildren();
                    }
                });
            }
            
            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            return ports.Where(port => port.node != startPort.node && port.direction != startPort.direction).ToList();
        }

        public void CreteNode(Type type, Vector2 pos, BTNodeView sourceNode, bool isAsParent) {
            var nodeView = (BTNodeView) null;
            var node = CreateNode(type, pos);
            
            // sourceNode作为子节点
            if (sourceNode != null && !isAsParent) {
                // 刪除此节点在数据部分作为孩子的关系
                foreach (var connection in sourceNode.Input.connections) {
                    if (connection.output.node is BTNodeView parentView) {
                        RemoveChild(parentView.Node, sourceNode.Node);
                    }
                }
                // 重新构建数据关系
                AddChild(node, sourceNode.Node);
                // 更新对应的节点视图
                nodeView = CreateNodeView(node);
                AddChildView(nodeView, sourceNode);
            }
            else {
                if (sourceNode != null) {
                    AddChild(sourceNode.Node, node);
                }
                
                // 更新对应的节点视图
                nodeView = CreateNodeView(node);
                if (sourceNode != null) {
                    AddChildView(sourceNode, nodeView);
                }
            }

            SelectNode(nodeView);
        }


        private BTNode CreateNode<T>(Vector2 pos) where T : BTNode {
            return CreateNode(typeof(T), pos);
        }

        private BTNode CreateNode(Type type, Vector2 pos) {
            if (Activator.CreateInstance(type) is not BTNode node) {
                return null;
            }

            node.Name = type.Name;
            node.Guid = Guid.NewGuid().ToString();
            node.PosX = pos.x;
            node.PosY = pos.y;
            
            _treeNodeData.AddNode(node);
            return node;
        }
        
        private BTNodeView CreateNodeView(BTNode btNode) {
            var nodeView = new BTNodeView(this);
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.ImportData(btNode);
            
            if (btNode is EntryNode) {
                nodeView.capabilities &= ~Capabilities.Deletable;
                nodeView.capabilities &= ~Capabilities.Movable;
            }

            AddElement(nodeView);
            return nodeView;
        }

        private void AddChildView(BTNodeView parentView, BTNodeView childView) {
            // 父节点输出端口为单连接时，需要先删除已存在的连线
            if (parentView.Output.capacity == Port.Capacity.Single) {
                RemoveEdgesOnly(parentView.Output.connections);
            }
            // 删除子节点输入端口已存在的连线
            RemoveEdgesOnly(childView.Input.connections);
            // 连接两个节点
            ConnectNode(parentView, childView);
        }

        private void RemoveEdgesOnly(IEnumerable<Edge> toRemove) {
            _isRemoveOnly = true;
            DeleteElements(toRemove);
            _isRemoveOnly = false;
        }

        private void ConnectNode(BTNodeView parentView, BTNodeView childView) {
            var edge = parentView.Output.ConnectTo(childView.Input);
            AddElement(edge);
        }
        
        public void SelectNode(BTNodeView nodeView) {
            ClearSelection();
            AddToSelection(nodeView);
        }

        public void UpdateNodesStyle() {
            nodes.ForEach(node => {
                var nodeView = node as BTNodeView;
                nodeView?.UpdateStyleByState();
            });
        }

        private void AddChild(BTNode parentNode, BTNode childNode) {
            var oldData = _treeNodeData.DeepCopy();
            
            switch (parentNode) {
                case Composite composite: {
                    composite.ChildrenGuids.Add(childNode.Guid);
                    break;
                }
                case Decorator decorator: {
                    decorator.ChildGuid = childNode.Guid;
                    break;
                }
                case EntryNode entryNode: {
                    entryNode.ChildGuid = childNode.Guid;
                    break;
                }
            }

            var newData = _treeNodeData.DeepCopy();
            var command = new NodeDataCommand(this, oldData, newData);
            BTEditorWindow.Instance.AddCommand(command);
        }

        private void RemoveChild(BTNode parentNode, BTNode childNode) {
            var oldData = _treeNodeData.DeepCopy();
            
            switch (parentNode) {
                case Composite composite: {
                    composite.ChildrenGuids.Remove(childNode.Guid);
                    break;
                }
                case Decorator decorator: {
                    decorator.ChildGuid = null;
                    break;
                }
                case EntryNode entryNode: {
                    entryNode.ChildGuid = null;
                    break;
                }
            }
            
            var newData = _treeNodeData.DeepCopy();
            var command = new NodeDataCommand(this, oldData, newData);
            BTEditorWindow.Instance.AddCommand(command);
        }

        private class CopyPasteData
        {
            public readonly List<string> Guids = new List<string>();

            public void AddGraphElements(IEnumerable<GraphElement> toCopy) {
                foreach (var element in toCopy) {
                    if (element is BTNodeView { Node: not EntryNode } nodeView) {
                        Guids.Add(nodeView.Node.Guid);
                    }
                }
            }
        }
        
        /// <summary>
        /// 只记录有连接关系节点的Guid即可
        /// </summary>
        private class EdgeToCreate
        {
            public string ParentGuid;
            public string ChildGuid;
        }
    }
}
