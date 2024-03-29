﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public class Resolver : Expr.IVisitor<object>, Stmt.IVisitor<object>
    {
        private readonly Interpreter interpreter;
        private readonly Stack<Dictionary<string, bool>> _scopes = new Stack<Dictionary<string, bool>>();
        private FunctionType _currentFunction = FunctionType.NONE;
        public Resolver(Interpreter interpreter)
        {
            this.interpreter = interpreter;
        }
        private enum FunctionType
        {
            NONE,
            FUNCTION,
            METHOD,
            INITIALIZER
        }
        private enum ClassType
        {
            NONE,
            CLASS,
            SUBCLASS
        }

        private ClassType currentClass = ClassType.NONE;
        public object VisitBlockStmt(Stmt.Block stmt)
        {
            BeginScope();
            Resolve(stmt.Statements);
            EndScope();
            return null;
        }
        public object VisitClassStmt(Stmt.Class stmt)
        {
            ClassType enclosingClass = currentClass;
            currentClass = ClassType.CLASS;
            Declare(stmt.Name);
            Define(stmt.Name);

            BeginScope();
            _scopes.Peek().Add("this", true);

            foreach (Stmt.Function method in stmt.Methods)
            {
                FunctionType declaration = FunctionType.METHOD;
                if (method.Name.Lexeme.Equals("init"))
                {
                    declaration = FunctionType.INITIALIZER;
                }

                ResolveFunction(method, declaration);
            }
            EndScope();

            currentClass = enclosingClass;

            return null;
        }

        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Resolve(stmt.InnerExpression);
            return null;
        }
        public object VisitFunctionStmt(Stmt.Function stmt)
        {
            Declare(stmt.Name);
            Define(stmt.Name);

            ResolveFunction(stmt, FunctionType.FUNCTION);
            return null;
        }
        public object VisitIfStmt(Stmt.If stmt)
        {
            Resolve(stmt.Condition);
            Resolve(stmt.ThenBranch);
            if (stmt.ElseBranch != null) Resolve(stmt.ElseBranch);
            return null;
        }
        public object VisitPrintStmt(Stmt.Print stmt)
        {
            Resolve(stmt.InnerExpression);
            return null;
        }
        public object VisitReturnStmt(Stmt.Return stmt)
        {
            if (_currentFunction == FunctionType.NONE)
            {
                Lox.Error(stmt.Keyword, "Cannot return a value from an initializer.");
            }

            if (stmt.Value != null)
            {
                Resolve(stmt.Value);
            }
            return null;
        }
        public object VisitWhileStmt(Stmt.While stmt)
        {
            Resolve(stmt.Condition);
            Resolve(stmt.Body);
            return null;
        }

        public object VisitVarStmt(Stmt.Var stmt)
        {
            Declare(stmt.Name);
            if (stmt.Initializer != null)
            {
                Resolve(stmt.Initializer);
            }
            Define(stmt.Name);
            return null;
        }
        public object VisitAssignExpr(Expr.Assign expr)
        {
            Resolve(expr.Value);
            ResolveLocal(expr, expr.Name);
            return null;
        }
        public object VisitBinaryExpr(Expr.Binary expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);
            return null;
        }
        public object VisitCallExpr(Expr.Call expr)
        {
            Resolve(expr.Callee);

            foreach (var argument in expr.Arguments)
            {
                Resolve(argument);
            }

            return null;
        }
        public object VisitGetExpr(Expr.Get expr)
        {
            Resolve(expr.Object);
            return null;
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            Resolve(expr.Expression);
            return null;
        }
        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return null;
        }
        public object VisitLogicalExpr(Expr.Logical expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);
            return null;
        }
        public object VisitSetExpr(Expr.Set expr)
        {
            Resolve(expr.Value);
            Resolve(expr.Object);
            return null;
        }
        public object VisitThisExpr(Expr.This expr)
        {
            if (currentClass == ClassType.NONE)
            {
                Lox.Error(expr.Keyword.Line, "Can't use 'this' outside of a class.");
                return null;
            }
            ResolveLocal(expr, expr.Keyword);
            return null;
        }


        public object VisitUnaryExpr(Expr.Unary expr)
        {
            Resolve(expr.Right);
            return null;
        }
        public object VisitVariableExpr(Expr.Variable expr)
        {
            if (_scopes.Count > 0 && _scopes.Peek().TryGetValue(expr.Name.Lexeme, out var isDefined) && !isDefined)
            {
                Lox.Error(expr.Name,
                    "Cannot read local variable in its own initializer.");
            }

            ResolveLocal(expr, expr.Name);
            return null;
        }
        private void Declare(Token name)
        {
            if (_scopes.Count == 0) return;

            Dictionary<string, bool> scope = _scopes.Peek();
            if (scope.ContainsKey(name.Lexeme))
            {
                Lox.Error(name,
                    "Already a variable with this name in this scope.");
            }

            scope[name.Lexeme] = false;
        }
        private void Define(Token name)
        {
            if (_scopes.Count == 0) return;
            _scopes.Peek()[name.Lexeme] = true;
        }
        private void BeginScope()
        {
            _scopes.Push(new Dictionary<string, bool>());
        }
        private void EndScope()
        {
            _scopes.Pop();
        }
        public void Resolve(List<Stmt> statements)
        {
            foreach (Stmt statement in statements)
            {
                Resolve(statement);
            }
        }
        private void Resolve(Stmt stmt)
        {
            stmt.Accept(this);
        }
        private void Resolve(Expr expr)
        {
            expr.Accept(this);
        }
        private void ResolveLocal(Expr expr, Token name)
        {
            var scopes = _scopes.ToArray();

            for (var i = 0; i < scopes.Length; i++)
            {
                if (scopes[i].ContainsKey(name.Lexeme))
                {
                    interpreter.Resolve(expr, i);
                    return;
                }
            }

        }
        private void ResolveFunction(Stmt.Function function, FunctionType type)
        {
            FunctionType enclosingFunction = _currentFunction;
            _currentFunction = type;

            BeginScope();
            foreach (Token param in function.Params)
            {
                Declare(param);
                Define(param);
            }
            Resolve(function.Body);
            EndScope();
            _currentFunction = enclosingFunction;
        }

    }

}
