namespace GinjaSoft.TreeStuff
{
  using System;


  public class LongestPath
  {
    //
    // Public types
    //

    // This type contains information about the paths that exist within a given tree
    public class NodeInfo
    {
      // The value of the tree's root node
      public uint RootValue { get; set; }

      // The length of the longest path of nodes (up through the root node) with consecutively increasing values 
      public uint MaxIncPathLenToRoot { get; set; }

      // The length of the longest path of nodes (up through the root node) with consecutively decreasing values
      public uint MaxDecPathLenToRoot { get; set; }

      // The length of the longest path of nodes with either consecutively increasing or decreasing values.  This path
      // does not have to include the root node and also doesn't have to extend exclusively up the tree towards the
      // root.  The path can start from a given node, proceed upwards through other nodes and then downwards again
      // through child nodes.  This is the value of interest.
      public uint MaxPathLen { get; set; }
    }


    //
    // This function is the solution to the interview question
    //

    public static uint GetMaxPathLength(Node<uint> tree)
    {
      var nodeInfo = GetNodeInfo(tree);
      return nodeInfo.MaxPathLen;
    }


    //
    // Implementation
    //

    internal static NodeInfo GetNodeInfo(Node<uint> tree)
    {
      // Unfortunately I can't pass ProcessNode directly to DepthFirstPostOrder.  C# won't do the implicit cast from
      // method group to Func and so a local variable is required.  Sigh.
      Func<Node<uint>, NodeInfo, NodeInfo, NodeInfo> fn = ProcessNode;
      return BinaryTreeTraversals.PostOrderDepthFirst(tree, fn);
    }


    private static NodeInfo ProcessNode(Node<uint> root, NodeInfo left, NodeInfo right)
    {
      // Base result for a leaf node
      var returnValue = new NodeInfo {
        RootValue = root.Value,
        MaxIncPathLenToRoot = 1,
        MaxDecPathLenToRoot = 1,
        MaxPathLen = 1
      };

      // If there are no children to process then we are done
      if(left == null && right == null) return returnValue;

      // Process children ...

      if(left != null) {
        // Update the min/max PathLenToRoot based on the current root value and left child root value
        if(root.Value > left.RootValue) returnValue.MaxIncPathLenToRoot = left.MaxIncPathLenToRoot + 1;
        else if(root.Value < left.RootValue) returnValue.MaxDecPathLenToRoot = left.MaxDecPathLenToRoot + 1;
      }

      if(right != null) {
        // Update the min/max PathLenToRoot based on the current root value and right child root value
        if(root.Value > right.RootValue) {
          var pathLen = right.MaxIncPathLenToRoot + 1;
          returnValue.MaxIncPathLenToRoot = Math.Max(returnValue.MaxIncPathLenToRoot, pathLen);
        }
        else if(root.Value < right.RootValue) {
          var pathLen = right.MaxDecPathLenToRoot + 1;
          returnValue.MaxDecPathLenToRoot = Math.Max(returnValue.MaxDecPathLenToRoot, pathLen);
        }
      }

      // The initial new MaxPathLen is the max of the children ...
      var leftMaxPathLen = left != null ? left.MaxPathLen : 0;
      var rightMaxPathLen = right != null ? right.MaxPathLen : 0;
      returnValue.MaxPathLen = Math.Max(leftMaxPathLen, rightMaxPathLen);

      // Take either of the two new max path lengths through the root if larger ...
      returnValue.MaxPathLen = Math.Max(returnValue.MaxPathLen, returnValue.MaxIncPathLenToRoot);
      returnValue.MaxPathLen = Math.Max(returnValue.MaxPathLen, returnValue.MaxDecPathLenToRoot);

      // Now check for paths that go up and down through the root ...
      if(left != null && right != null) {
        if(root.Value > left.RootValue && root.Value < right.RootValue) {
          var upDownPathLen = left.MaxIncPathLenToRoot + right.MaxDecPathLenToRoot + 1;
          returnValue.MaxPathLen = Math.Max(returnValue.MaxPathLen, upDownPathLen);
        }
        else if(root.Value < left.RootValue && root.Value > right.RootValue) {
          var upDownPathLen = left.MaxDecPathLenToRoot + right.MaxIncPathLenToRoot + 1;
          returnValue.MaxPathLen = Math.Max(returnValue.MaxPathLen, upDownPathLen);
        }
      }

      return returnValue;
    }
  }
}