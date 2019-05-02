namespace GinjaSoft.TreeStuff
{
  using System;


  public static class BinaryTreeTraversals
  {
    public static TResult PostOrderDepthFirst<T, TResult>(Node<T> root,
                                                          Func<Node<T>, TResult, TResult, TResult> fn)
    {
      TResult leftResult = default;
      TResult rightResult = default;

      if(root.Left != null) leftResult = PostOrderDepthFirst(root.Left, fn);
      if(root.Right != null) rightResult = PostOrderDepthFirst(root.Right, fn);

      return fn(root, leftResult, rightResult);
    }
  }
}