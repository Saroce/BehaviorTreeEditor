﻿//------------------------------------------------------------
//        File:  BTNodeView.cs
//       Brief:  BTNodeView
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-05
//============================================================

using System;
using BTCore.Runtime;
using BTCore.Runtime.Composites;
using BTCore.Runtime.Conditions;
using BTCore.Runtime.Decorators;
using Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Action = BTCore.Runtime.Actions.Action;

namespace BTCore.Editor
{
    public class BTNodeView : Node, IDataSerializableEditor<BTNode>
    {
        public Action<BTNodeView> OnNodeSelected;

        public BTNode Node { get; private set; }
        public Port Input { get; private set; }
        public Port Output { get; private set; }

        private readonly BTView _btView;
        private NodePos _recordPos;
        private string _oldData;

        public BTNodeView NodeParent {
            get {
                foreach (var edge in Input.connections) {
                    return edge.output.node as BTNodeView;
                }
                
                return null;
            }
        }
        
        public BTNodeView(BTView btView) : base(BTEditorDef.BTNodeViewUxmlPath) {
            _btView = btView;
        }

        public void ImportData(BTNode data) {
            Node = data;
            title = data.Name;
            viewDataKey = data.Guid;

            style.left = data.PosX;
            style.top = data.PosY;

            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();
        }

        public BTNode ExportData() {
            return null;
        }
        
        /// <summary>
        /// 创建节点的输入端口
        /// </summary>
        private void CreateInputPorts() {
            // 行为树入口节点没有输入端口，其他节点的输入端口都是单连接
            if (Node is not EntryNode) {
                Input = new NodePort(Direction.Input, Port.Capacity.Single);
            }

            if (Input != null) {
                Input.portName = string.Empty;
                // 为了使节点上下Port(动态生成)对齐, 需要将其内部的VisualElement与Label垂直分布(默认水平) 
                Input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(Input);
            }
        }

        /// <summary>
        /// 创建节点的输出端口
        /// </summary>
        private void CreateOutputPorts() {
            /* 1. 行为节点，条件节点没有输出端口
             * 2. 复合节点输出端口为多连接
             * 3. 其他节点的输出端口都为单连接 */
            switch (Node) {
                case Composite: {
                    Output = new NodePort(Direction.Output, Port.Capacity.Multi);
                    break;
                }
                case Decorator:
                case EntryNode: {
                    Output = new NodePort(Direction.Output, Port.Capacity.Single);
                    break;
                }
            }

            if (Output != null) {
                Output.portName = string.Empty;
                // 为了使节点上下Port(动态生成)对齐, 需要将其内部的VisualElement与Label垂直分布(默认水平) 
                Output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(Output);
            }
        }

        /// <summary>
        /// 根据节点类型添加不同的样式，主要是显示不同的input部分颜色
        /// </summary>
        private void SetupClasses() {
            switch (Node) {
                case Composite: 
                    AddToClassList("composite");
                    break;
                case Decorator: 
                    AddToClassList("decorator");
                    break;
                case Condition:
                    AddToClassList("condition");
                    break;
                case Action:  
                    AddToClassList("action");
                    break;
                case EntryNode:
                    AddToClassList("entry");
                    break;
            }
        }

        public override void SetPosition(Rect newPos) {
            base.SetPosition(newPos);

            // 更新坐标数据之前，先记录下
            if (string.IsNullOrEmpty(_oldData)) {
                _oldData = JsonConvert.SerializeObject(_btView.ExportData(), BTDef.SerializerSettingsAll);
            }
            
            Node.PosX = newPos.x;
            Node.PosY = newPos.y;
        }

        
        public override void OnSelected() {
            base.OnSelected();
            
            _oldData = null;
            OnNodeSelected?.Invoke(this);
            _recordPos = new NodePos(Node.PosX, Node.PosY);
        }

        /// <summary>
        /// 没找到拖拽结束事件回调0.0，这里暂时以取消选中后，判断节点位置是否发生变化
        /// </summary>
        public override void OnUnselected() {
            base.OnUnselected();

            var curPos = new NodePos(Node.PosX, Node.PosY);
            if (!_recordPos.IsChanged(curPos) || string.IsNullOrEmpty(_oldData)) {
                return;
            }
            
            var newData = JsonConvert.SerializeObject(_btView.ExportData(), BTDef.SerializerSettingsAll);
            var command = new NodeDataCommand(_btView, _oldData, newData);
            BTEditorWindow.Instance.AddCommand(command);
        }

        public void SortChildren() {
            // 组合节点下面的子节点在被任意移动时，需要根据子节点当前位置进行重行排序
            if (Node is Composite composite) {
                composite.ChildrenGuids.Sort(ChildNodeComparer);
            }
        }

        private int ChildNodeComparer(string leftGuid, string rightGuid) {
            var treeNodeData = _btView.ExportData();
            if (treeNodeData == null) {
                return 0;
            }

            var leftNode = treeNodeData.GetNodeByGuid(leftGuid);
            var rightNode = treeNodeData.GetNodeByGuid(rightGuid);

            if (leftNode == null) {
                return 1;
            }

            if (rightGuid == null) {
                return -1;
            }

            // 比较节点横坐标值排序
            return leftNode.PosX.CompareTo(rightNode.PosX);
        }

        /// <summary>
        /// 运行时，根据节点状态不同，应用不同的样式显示
        /// </summary>
        public void UpdateStyleByState() {
            if (!Application.isPlaying) {
                return;
            }
            
            RemoveFromClassList("inactive");
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");

            switch (Node.State) {
                case NodeState.Inactive:
                    AddToClassList("inactive");
                    break;
                case NodeState.Running:
                    AddToClassList("running");
                    break;
                case NodeState.Success:
                    AddToClassList("success");
                    break;
                case NodeState.Failure:
                    AddToClassList("failure");
                    break;
            }
        }
        
        private struct NodePos
        {
            private readonly float _x;
            private readonly float _y;

            public NodePos(float x, float y) {
                _x = x;
                _y = y;
            }

            public bool IsChanged(NodePos other) {
                return Mathf.Abs(_x - other._x) > 0.1f || Mathf.Abs(_y - other._y) > 0.1f;
            }
        }
    }
}
