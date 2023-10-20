//------------------------------------------------------------
//        File:  ExternalNodeInspector.cs
//       Brief:  ExternalNodeInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using System;
using System.Collections.Generic;
using BTCore.Runtime;
using BTCore.Runtime.Externals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BTCore.Editor.Inspectors
{
    public abstract class ExternalNodeInspector : BTNodeInspector
    {
        [TitleGroup("$GetTitleName")]
        [ShowInInspector]
        [LabelText("TypeName")]
        [LabelWidth(100)]
        [OnValueChanged("OnFieldValueChanged")]
        [ValidateInput("StringValidator", "$GetInfoMessage", ContinuousValidationCheck = true)]
        protected string TypeName;

        [TitleGroup("$GetTitleName")]
        [ShowInInspector]
        [OnValueChanged("OnFieldValueChanged")]
        protected Dictionary<string, string> Properties = new Dictionary<string, string>();

        private IExternalNode _externalNode;
        
        private bool StringValidator(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        protected abstract string GetInfoMessage();

        protected abstract string GetTitleName();
        
        public override void ImportData(BTNode data) {
            if (data is not IExternalNode externalNode) {
                return;
            }

            _externalNode = externalNode;
            TypeName = externalNode.TypeName;
            Properties = externalNode.Properties;
        }

        public override BTNode ExportData() {
            return _externalNode as BTNode;
        }

        protected override void OnFieldValueChanged() {
            if (_externalNode == null) {
                return;
            }

            _externalNode.TypeName = TypeName;
            _externalNode.Properties = Properties;
            // 更新Name字段，同步更新对应NodeView的title显示
            if (_externalNode is BTNode node) {
                node.Name = TypeName;
                OnNodeViewUpdate?.Invoke();
            }
        }

        public override void Reset() {
            TypeName = null;
            Properties = new Dictionary<string, string>();
            _externalNode = null;
        }
    }
}