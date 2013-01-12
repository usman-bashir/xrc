using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace xrc.Script
{
    public class ScriptExpression : IScriptExpression
    {
        readonly DynamicExpresso.ExpressionDefinition _expression;

        public ScriptExpression(DynamicExpresso.ExpressionDefinition expression)
        {
            _expression = expression;
        }

        public override string ToString()
        {
            return _expression.ToString();
        }

        public Type ReturnType
        {
            get { return _expression.ReturnType; }
        }

        public DynamicExpresso.ExpressionDefinition ExpressionDefinition
        {
            get { return _expression; }
        }
    }
}
