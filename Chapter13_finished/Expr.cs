using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public abstract class Expr
    {
        public interface IVisitor<R>
        {
            R VisitAssignExpr(Assign expr);
            R VisitBinaryExpr(Binary expr);
            R VisitCallExpr(Call expr);
            R VisitGetExpr(Get expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitLiteralExpr(Literal expr);
            R VisitLogicalExpr(Logical expr);
            R VisitSetExpr(Set expr);
            R VisitSuperExpr(Super expr);
            R VisitThisExpr(This expr);
            R VisitUnaryExpr(Unary expr);
            R VisitVariableExpr(Variable expr);
        }

        public class Assign : Expr
        {
            public Assign(Token name, Expr value)
            {
                this.Name = name;
                this.Value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitAssignExpr(this);
            }

            public readonly Token Name;
            public readonly Expr Value;
        }

        public class Binary : Expr
        {
            public Binary(Expr left, Token operatorToken, Expr right)
            {
                this.Left = left;
                this.Operator = operatorToken;
                this.Right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }

            public readonly Expr Left;
            public readonly Token Operator;
            public readonly Expr Right;
        }

        public class Call : Expr
        {
            public Call(Expr callee, Token paren, List<Expr> arguments)
            {
                this.Callee = callee;
                this.Paren = paren;
                this.Arguments = arguments;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitCallExpr(this);
            }

            public readonly Expr Callee;
            public readonly Token Paren;
            public readonly List<Expr> Arguments;
        }

        public class Get : Expr
        {
            public Get(Expr obj, Token name)
            {
                this.Object = obj;
                this.Name = name;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGetExpr(this);
            }

            public readonly Expr Object;
            public readonly Token Name;
        }

        public class Grouping : Expr
        {
            public Grouping(Expr expression)
            {
                this.Expression = expression;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }

            public readonly Expr Expression;
        }

        public class Literal : Expr
        {
            public Literal(object value)
            {
                this.Value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }

            public readonly object Value;
        }

        public class Logical : Expr
        {
            public Logical(Expr left, Token operatorToken, Expr right)
            {
                this.Left = left;
                this.Operator = operatorToken;
                this.Right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLogicalExpr(this);
            }

            public readonly Expr Left;
            public readonly Token Operator;
            public readonly Expr Right;
        }

        public class Set : Expr
        {
            public Set(Expr obj, Token name, Expr value)
            {
                this.Object = obj;
                this.Name = name;
                this.Value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitSetExpr(this);
            }

            public readonly Expr Object;
            public readonly Token Name;
            public readonly Expr Value;
        }

        public class Super : Expr
        {
            public Super(Token keyword, Token method)
            {
                this.Keyword = keyword;
                this.Method = method;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitSuperExpr(this);
            }

            public readonly Token Keyword;
            public readonly Token Method;
        }

        public class This : Expr
        {
            public This(Token keyword)
            {
                this.Keyword = keyword;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitThisExpr(this);
            }

            public readonly Token Keyword;
        }

        public class Unary : Expr
        {
            public Unary(Token operatorToken, Expr right)
            {
                this.Operator = operatorToken;
                this.Right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }

            public readonly Token Operator;
            public readonly Expr Right;
        }

        public class Variable : Expr
        {
            public Variable(Token name)
            {
                this.Name = name;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitVariableExpr(this);
            }

            public readonly Token Name;
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
