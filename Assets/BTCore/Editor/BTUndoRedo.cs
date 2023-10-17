//------------------------------------------------------------
//        File:  BTUndoRedo.cs
//       Brief:  BTUndoRedo
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-15
//============================================================

using System.Collections.Generic;
using BTCore.Runtime;

namespace BTCore.Editor
{
    public interface ICommand
    {
        void Execute();
        
        void Undo();
    }

    public class NodeDataCommand : ICommand
    {
        private readonly TreeNodeData _oldData;
        private readonly TreeNodeData _newData;
        private readonly BTView _btView;
        
        public NodeDataCommand(BTView btView, TreeNodeData oldData, TreeNodeData newData) {
            _btView = btView;
            _oldData = oldData;
            _newData = newData;
        }
        
        public void Execute() {
            _btView.ImportData(_newData);
        }

        public void Undo() {
            _btView.ImportData(_oldData);
        }
    }
    
    public class BTUndoRedo
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void AddCommand(ICommand command) {
            _undoStack.Push(command);
            _redoStack.Clear();
        }
        
        public void Undo() {
            if (_undoStack.Count <= 0) {
                return;
            }

            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }

        public void Redo() {
            if (_redoStack.Count <= 0) {
                return;
            }

            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }

        public void Clear() {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}
