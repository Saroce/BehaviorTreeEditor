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
        [ShowInInspector]
        [LabelText("TypeName")]
        [LabelWidth(100)]
        [OnValueChanged("OnFieldValueChanged")]
        [ValidateInput("StringValidator", "$GetInfoMessage", ContinuousValidationCheck = true)]
        protected string TypeName;

        [ShowInInspector]
        [OnValueChanged("OnFieldValueChanged")]
        protected Dictionary<string, string> Properties = new Dictionary<string, string>();

        private IExternalNode _externalNode;
        
        private bool StringValidator(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        protected abstract string GetInfoMessage();
        
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
        }

        public override void Reset() {
            TypeName = null;
            Properties = new Dictionary<string, string>();
            _externalNode = null;
        }
    }
}