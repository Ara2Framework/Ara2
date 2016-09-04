using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ara2.Grid
{
    public class OrderByRemover : ExpressionVisitor
    {
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType != typeof(Enumerable) && node.Method.DeclaringType != typeof(Queryable))
                return base.VisitMethodCall(node);

            if (node.Method.Name != "OrderBy" && node.Method.Name != "OrderByDescending" && node.Method.Name != "ThenBy" && node.Method.Name != "ThenByDescending")
                return base.VisitMethodCall(node);

            //eliminate the method call from the expression tree by returning the object of the call.
            return base.Visit(node.Arguments[0]);
        }
    }
}
