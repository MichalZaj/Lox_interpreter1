using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public abstract class Stmt
    {
        public interface IVisitor<R>
        {
            R VisitBlockStmt(Block stmt);
           // R VisitClassStmt(Class stmt);
            R VisitExpressionStmt(Expression stmt);
            //R VisitFunctionStmt(Function stmt);
            R VisitIfStmt(If stmt);
            R VisitPrintStmt(Print stmt);
            //R VisitReturnStmt(Return stmt);
            R VisitVarStmt(Var stmt);
            R VisitWhileStmt(While stmt);
        }

        // Nested Stmt classes here...
        public class Expression : Stmt
        {
            public Expression(Expr expression)
            {
                this.InnerExpression = expression;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitExpressionStmt(this);
            }

            public readonly Expr InnerExpression;
        }
        public class Print : Stmt
        {
            public Print(Expr expression)
            {
                this.InnerExpression = expression;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitPrintStmt(this);
            }

            public readonly Expr InnerExpression;
        }
        public class Var : Stmt
        {
            public Var(Token name, Expr initializer)
            {
                this.Name = name;
                this.Initializer = initializer;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitVarStmt(this);
            }

            public readonly Token Name;
            public readonly Expr Initializer;
        }
        public class Block : Stmt
        {
            public Block(List<Stmt> statements)
            {
                this.Statements = statements;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBlockStmt(this);
            }

            public readonly List<Stmt> Statements;
        }
        public class If : Stmt
        {
            public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
            {
                this.Condition = condition;
                this.ThenBranch = thenBranch;
                this.ElseBranch = elseBranch;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitIfStmt(this);
            }

            public readonly Expr Condition;
            public readonly Stmt ThenBranch;
            public readonly Stmt ElseBranch;
        }
        public class While : Stmt
        {
            public While(Expr condition, Stmt body)
            {
                this.Condition = condition;
                this.Body = body;
            }
            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitWhileStmt(this);
            }

            public readonly Expr Condition;
            public readonly Stmt Body;
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }

}
