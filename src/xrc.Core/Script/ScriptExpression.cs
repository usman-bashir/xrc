﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace xrc.Script
{
    public class ScriptExpression : IScriptExpression
    {
        readonly DynamicExpresso.Function _expression;

        public ScriptExpression(DynamicExpresso.Function expression)
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

        public DynamicExpresso.Function ExpressionDefinition
        {
            get { return _expression; }
        }
    }
}
