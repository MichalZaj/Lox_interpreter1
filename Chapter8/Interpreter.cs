using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<object>
    {
        private EnvironmentLox environmentLox = new EnvironmentLox();
        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError error)
            {
                Lox.RuntimeError(error);
            }
        }
        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }
        public object VisitUnaryExpr(Expr.Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.BANG:
                    return !IsTruthy(right);

                case TokenType.MINUS:
                    return -(double)right;
            }

            // Unreachable.
            return null;
        }
        public object VisitVariableExpr(Expr.Variable expr)
        {
            return environmentLox.Get(expr.Name);
        }

        private void CheckNumberOperand(Token @operator, object operand)
        {
            if (operand is double)
            {
                return;
            }
            throw new RuntimeError(@operator, "Operand must be a number.");
        }
        private void CheckNumberOperands(Token @operator, object left, object right)
        {
            if (left is double && right is double)
                return;

            throw new RuntimeError(@operator, "Operands must be numbers.");
        }
        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.Expression);
        }
        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }
        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }
        void ExecuteBlock(List<Stmt> statements, EnvironmentLox environment)
        {
            EnvironmentLox previous = this.environmentLox;
            try
            {
                this.environmentLox = environment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environmentLox = previous;
            }
        }

        public object VisitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.Statements, new EnvironmentLox(environmentLox));
            return null;
        }
        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.InnerExpression);
            return null;
        }
        public object VisitPrintStmt(Stmt.Print stmt)
        {
            object value = Evaluate(stmt.InnerExpression);
            Console.WriteLine(Stringify(value));
            return null;
        }
        public object VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }

            environmentLox.Define(stmt.Name.Lexeme, value);
            return null;
        }
        public object VisitAssignExpr(Expr.Assign expr)
        {
            object value = Evaluate(expr.Value);
            environmentLox.Assign(expr.Name, value);
            return value;
        }
        public object VisitBinaryExpr(Expr.Binary expr)
        {
            object left = Evaluate(expr.Left);
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }
                    if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }
                    throw new RuntimeError(expr.Operator,
                        "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
            }

            // Unreachable.
            return null;
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }
        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;

            return a.Equals(b);
        }
        private string Stringify(object obj)
        {
            if (obj == null)
                return "nil";

            if (obj is double)
            {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return obj.ToString();
        }

    }

}
